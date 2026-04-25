// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek specialized streaming chat message content.
/// </summary>
public sealed class DeepSeekStreamingChatMessageContent : StreamingChatMessageContent
{
    /// <summary>
    /// Gets the reasoning content from the streaming update.
    /// </summary>
    public string? ReasoningContent { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeepSeekStreamingChatMessageContent"/> class.
    /// </summary>
    /// <param name="authorRole">Author role of the message.</param>
    /// <param name="content">Content of the message.</param>
    /// <param name="reasoningContent">Reasoning content of the message.</param>
    /// <param name="finishReason">Finish reason of the completion.</param>
    /// <param name="choiceIndex">Index of the choice.</param>
    /// <param name="modelId">The model ID used to generate the content.</param>
    /// <param name="metadata">Additional metadata.</param>
    public DeepSeekStreamingChatMessageContent(
        AuthorRole? authorRole,
        string? content,
        string? reasoningContent,
        string? finishReason,
        int choiceIndex,
        string? modelId,
        IReadOnlyDictionary<string, object?>? metadata = null)
        : base(
            authorRole,
            content,
            null,
            choiceIndex,
            modelId,
            Encoding.UTF8,
            metadata)
    {
        this.ReasoningContent = reasoningContent;
        this.FinishReason = finishReason;
    }
}
