// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek chat completion request.
/// </summary>
public class DeepSeekChatCompletionRequest
{
    /// <summary>
    /// Gets or sets the model ID to use.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    /// Gets or sets the messages to send to the model.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<DeepSeekChatMessage> Messages { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens to generate.
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Gets or sets the temperature to use for sampling.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    /// Gets or sets the top-p value to use for sampling.
    /// </summary>
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }

    /// <summary>
    /// Gets or sets the reasoning effort level.
    /// </summary>
    [JsonPropertyName("reasoning_effort")]
    public string? ReasoningEffort { get; set; }

    /// <summary>
    /// Gets or sets the tools available to the model.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<DeepSeekTool>? Tools { get; set; }

    /// <summary>
    /// Gets or sets whether to stream the response.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
}

/// <summary>
/// DeepSeek chat completion response.
/// </summary>
public class DeepSeekChatCompletionResponse
{
    /// <summary>
    /// Gets or sets the ID of the response.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the object type.
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    /// Gets or sets the created timestamp.
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; set; }

    /// <summary>
    /// Gets or sets the model ID used.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    /// Gets or sets the choices returned by the model.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<DeepSeekChatCompletionChoice> Choices { get; set; }

    /// <summary>
    /// Gets or sets the usage information.
    /// </summary>
    [JsonPropertyName("usage")]
    public DeepSeekUsage Usage { get; set; }
}

/// <summary>
/// DeepSeek chat completion choice.
/// </summary>
public class DeepSeekChatCompletionChoice
{
    /// <summary>
    /// Gets or sets the index of the choice.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the message returned by the model.
    /// </summary>
    [JsonPropertyName("message")]
    public DeepSeekChatMessageResponse Message { get; set; }

    /// <summary>
    /// Gets or sets the finish reason.
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

/// <summary>
/// DeepSeek chat message.
/// </summary>
public class DeepSeekChatMessage
{
    /// <summary>
    /// Gets or sets the role of the message sender.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the reasoning content of the message.
    /// </summary>
    [JsonPropertyName("reasoning_content")]
    public string? ReasoningContent { get; set; }
}

/// <summary>
/// DeepSeek chat message response.
/// </summary>
public class DeepSeekChatMessageResponse : DeepSeekChatMessage
{
    /// <summary>
    /// Gets or sets the tool calls made by the model.
    /// </summary>
    [JsonPropertyName("tool_calls")]
    public List<DeepSeekToolCallResponse>? ToolCalls { get; set; }
}

/// <summary>
/// DeepSeek tool.
/// </summary>
public class DeepSeekTool
{
    /// <summary>
    /// Gets or sets the type of the tool.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the function associated with the tool.
    /// </summary>
    [JsonPropertyName("function")]
    public DeepSeekFunction Function { get; set; }
}

/// <summary>
/// DeepSeek function.
/// </summary>
public class DeepSeekFunction
{
    /// <summary>
    /// Gets or sets the name of the function.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the function.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the parameters of the function.
    /// </summary>
    [JsonPropertyName("parameters")]
    public object Parameters { get; set; }
}

/// <summary>
/// DeepSeek tool call response.
/// </summary>
public class DeepSeekToolCallResponse
{
    /// <summary>
    /// Gets or sets the ID of the tool call.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the tool call.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the function call information.
    /// </summary>
    [JsonPropertyName("function")]
    public DeepSeekFunctionCallResponse Function { get; set; }
}

/// <summary>
/// DeepSeek function call response.
/// </summary>
public class DeepSeekFunctionCallResponse
{
    /// <summary>
    /// Gets or sets the name of the function to call.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the arguments to pass to the function.
    /// </summary>
    [JsonPropertyName("arguments")]
    public Dictionary<string, object> Arguments { get; set; }
}

/// <summary>
/// DeepSeek usage information.
/// </summary>
public class DeepSeekUsage
{
    /// <summary>
    /// Gets or sets the number of prompt tokens used.
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    /// <summary>
    /// Gets or sets the number of completion tokens used.
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    /// <summary>
    /// Gets or sets the total number of tokens used.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
