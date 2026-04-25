// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek streaming chat completion response.
/// </summary>
public class DeepSeekStreamingChatCompletionResponse
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
    public List<DeepSeekStreamingChatCompletionChoice> Choices { get; set; }
}

/// <summary>
/// DeepSeek streaming chat completion choice.
/// </summary>
public class DeepSeekStreamingChatCompletionChoice
{
    /// <summary>
    /// Gets or sets the index of the choice.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the delta message returned by the model.
    /// </summary>
    [JsonPropertyName("delta")]
    public DeepSeekChatMessage Delta { get; set; }

    /// <summary>
    /// Gets or sets the finish reason.
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
}
