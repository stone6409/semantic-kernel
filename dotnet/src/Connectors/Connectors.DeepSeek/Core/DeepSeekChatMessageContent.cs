// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek specialized chat message content
/// </summary>
public sealed class DeepSeekChatMessageContent : ChatMessageContent
{
    /// <summary>
    /// Gets the metadata key for the tool id.
    /// </summary>
    public static string ToolIdProperty => "ChatCompletionsToolCall.Id";

    /// <summary>
    /// Gets the metadata key for the list of tool calls.
    /// </summary>
    internal static string FunctionToolCallsProperty => "ChatResponseMessage.FunctionToolCalls";

    /// <summary>
    /// Gets the reasoning content from the model.
    /// </summary>
    public string? ReasoningContent { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeepSeekChatMessageContent"/> class.
    /// </summary>
    internal DeepSeekChatMessageContent(
        AuthorRole role,
        string? content,
        string modelId,
        string? reasoningContent = null,
        IReadOnlyList<DeepSeekToolCall>? toolCalls = null,
        IReadOnlyDictionary<string, object?>? metadata = null)
        : base(role, content, modelId, content, System.Text.Encoding.UTF8, CreateMetadataDictionary(toolCalls, metadata))
    {
        this.ReasoningContent = reasoningContent;
        this.ToolCalls = toolCalls ?? [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeepSeekChatMessageContent"/> class.
    /// </summary>
    internal DeepSeekChatMessageContent(
        AuthorRole role,
        ChatMessageContentItemCollection items,
        string modelId,
        string? reasoningContent = null,
        IReadOnlyList<DeepSeekToolCall>? toolCalls = null,
        IReadOnlyDictionary<string, object?>? metadata = null)
        : base(role, items, modelId, items, System.Text.Encoding.UTF8, CreateMetadataDictionary(toolCalls, metadata))
    {
        this.ReasoningContent = reasoningContent;
        this.ToolCalls = toolCalls ?? [];
    }

    /// <summary>
    /// A list of the tools called by the model.
    /// </summary>
    public IReadOnlyList<DeepSeekToolCall> ToolCalls { get; }

    /// <summary>
    /// Retrieve the resulting function from the chat result.
    /// </summary>
    /// <returns>The <see cref="DeepSeekFunctionToolCall"/>, or null if no function was returned by the model.</returns>
    public IReadOnlyList<DeepSeekFunctionToolCall> GetDeepSeekFunctionToolCalls()
    {
        List<DeepSeekFunctionToolCall>? functionToolCallList = null;

        foreach (var toolCall in this.ToolCalls)
        {
            if (toolCall.Kind == DeepSeekToolCallKind.Function)
            {
                (functionToolCallList ??= []).Add(new DeepSeekFunctionToolCall(toolCall));
            }
        }

        if (functionToolCallList is not null)
        {
            return functionToolCallList;
        }

        return [];
    }

    private static IReadOnlyDictionary<string, object?>? CreateMetadataDictionary(
        IReadOnlyList<DeepSeekToolCall>? toolCalls,
        IReadOnlyDictionary<string, object?>? original)
    {
        // We only need to augment the metadata if there are any tool calls.
        if (toolCalls?.Count > 0)
        {
            Dictionary<string, object?> newDictionary;
            if (original is null)
            {
                // There's no existing metadata to clone; just allocate a new dictionary.
                newDictionary = new Dictionary<string, object?>(1);
            }
            else if (original is IDictionary<string, object?> origIDictionary)
            {
                // Efficiently clone the old dictionary to a new one.
                newDictionary = new Dictionary<string, object?>(origIDictionary);
            }
            else
            {
                // There's metadata to clone but we have to do so one item at a time.
                newDictionary = new Dictionary<string, object?>(original.Count + 1);
                foreach (var kvp in original)
                {
                    newDictionary[kvp.Key] = kvp.Value;
                }
            }

            // Add the additional entry.
            newDictionary.Add(FunctionToolCallsProperty, toolCalls.Where(ctc => ctc.Kind == DeepSeekToolCallKind.Function).ToList());

            return newDictionary;
        }

        return original;
    }
}
