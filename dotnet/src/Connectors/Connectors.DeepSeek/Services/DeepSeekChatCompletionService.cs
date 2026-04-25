// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek chat completion service.
/// </summary>
public sealed class DeepSeekChatCompletionService : IChatCompletionService
{
    private readonly HttpClient _httpClient;
    private readonly string _modelId;
    private readonly string _apiKey;
    private readonly string _endpoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeepSeekChatCompletionService"/> class.
    /// </summary>
    /// <param name="modelId">The model ID to use.</param>
    /// <param name="apiKey">The API key to use.</param>
    /// <param name="endpoint">The endpoint URL to use.</param>
    /// <param name="httpClient">The HTTP client to use.</param>
    public DeepSeekChatCompletionService(
        string modelId,
        string apiKey,
        string endpoint = "https://api.deepseek.com/v1/chat/completions",
        HttpClient? httpClient = null)
    {
        this._modelId = modelId;
        this._apiKey = apiKey;
        this._endpoint = endpoint;
        this._httpClient = httpClient ?? new HttpClient();
        this._httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var deepSeekExecutionSettings = executionSettings as DeepSeekPromptExecutionSettings ?? new DeepSeekPromptExecutionSettings();
        var request = this.CreateRequest(chatHistory, deepSeekExecutionSettings);

        var response = await this._httpClient.PostAsync(
            this._endpoint,
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
            cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var deepSeekResponse = JsonSerializer.Deserialize<DeepSeekChatCompletionResponse>(responseContent);

        if (deepSeekResponse?.Choices?.Count > 0)
        {
            var choice = deepSeekResponse.Choices[0];
            var content = choice.Message.Content;
            var reasoningContent = choice.Message.ReasoningContent;
            var toolCalls = this.ConvertToolCalls(choice.Message.ToolCalls);
            var authorRole = new AuthorRole(choice.Message.Role);

            return new List<ChatMessageContent>
            {
                new DeepSeekChatMessageContent(
                    authorRole,
                    content,
                    this._modelId,
                    reasoningContent,
                    toolCalls)
            };
        }

        return [];
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var deepSeekExecutionSettings = executionSettings as DeepSeekPromptExecutionSettings ?? new DeepSeekPromptExecutionSettings();
        var request = this.CreateRequest(chatHistory, deepSeekExecutionSettings);
        request.Stream = true;

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, this._endpoint);
        httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var response = await this._httpClient.SendAsync(
            httpRequest,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var reader = new System.IO.StreamReader(stream);
        var buffer = new StringBuilder();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("data: ", StringComparison.Ordinal))
            {
                var data = line.Substring(7);
                if (data == "[DONE]")
                {
                    break;
                }

                DeepSeekStreamingChatCompletionResponse? streamingResponse = null;
                try
                {
                    streamingResponse = JsonSerializer.Deserialize<DeepSeekStreamingChatCompletionResponse>(data);
                }
                catch (JsonException)
                {
                    continue;
                }

                if (streamingResponse?.Choices?.Count > 0)
                {
                    var choice = streamingResponse.Choices[0];
                    var content = choice.Delta.Content;
                    var reasoningContent = choice.Delta.ReasoningContent;
                    var finishReason = choice.FinishReason;

                    if (!string.IsNullOrEmpty(content) || !string.IsNullOrEmpty(reasoningContent))
                    {
                        yield return new DeepSeekStreamingChatMessageContent(
                            null,
                            content,
                            reasoningContent,
                            finishReason,
                            choice.Index,
                            this._modelId);
                    }
                }
            }
        }
    }

    /// <inheritdoc/>
    public async Task<ChatMessageContent> GetChatMessageContentAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var contents = await this.GetChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken).ConfigureAwait(false);
        return contents.FirstOrDefault() ?? throw new SKException("No response from DeepSeek API");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var message in this.GetStreamingChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken))
        {
            yield return message;
        }
    }

    /// <inheritdoc/>
    public Task<string> GetChatCompletionAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);
        return this.GetChatCompletionAsync(chatHistory, executionSettings, kernel, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<string> GetChatCompletionAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var content = await this.GetChatMessageContentAsync(chatHistory, executionSettings, kernel, cancellationToken).ConfigureAwait(false);
        return content.Content ?? string.Empty;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> GetStreamingChatCompletionAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);

        await foreach (var message in this.GetStreamingChatMessageContentAsync(chatHistory, executionSettings, kernel, cancellationToken))
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                yield return message.Content;
            }
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> GetStreamingChatCompletionAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var message in this.GetStreamingChatMessageContentAsync(chatHistory, executionSettings, kernel, cancellationToken))
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                yield return message.Content;
            }
        }
    }

    private DeepSeekChatCompletionRequest CreateRequest(ChatHistory chatHistory, DeepSeekPromptExecutionSettings settings)
    {
        var messages = new List<DeepSeekChatMessage>();
        foreach (var message in chatHistory)
        {
            var role = message.Role.ToString().ToLower();
            messages.Add(new DeepSeekChatMessage
            {
                Role = role,
                Content = message.Content,
                ReasoningContent = message.Metadata?.TryGetValue("ReasoningContent", out var reasoningContent) == true ? reasoningContent.ToString() : null
            });
        }

        return new DeepSeekChatCompletionRequest
        {
            Model = this._modelId,
            Messages = messages,
            MaxTokens = settings.MaxTokens,
            Temperature = settings.Temperature,
            TopP = settings.TopP,
            ReasoningEffort = settings.ReasoningEffort,
            Tools = settings.Tools?.Select(t => new DeepSeekTool
            {
                Type = "function",
                Function = new DeepSeekFunction
                {
                    Name = t.Name,
                    Description = t.Description,
                    Parameters = t.Parameters
                }
            }).ToList()
        };
    }

    private List<DeepSeekToolCall> ConvertToolCalls(List<DeepSeekToolCallResponse>? toolCalls)
    {
        var result = new List<DeepSeekToolCall>();
        if (toolCalls != null)
        {
            foreach (var toolCall in toolCalls)
            {
                if (toolCall.Type == "function")
                {
                    result.Add(new DeepSeekToolCall(
                        toolCall.Id,
                        toolCall.Function.Name,
                        toolCall.Function.Arguments));
                }
            }
        }
        return result;
    }
}