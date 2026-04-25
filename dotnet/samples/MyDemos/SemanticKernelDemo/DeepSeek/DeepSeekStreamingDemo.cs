// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.DeepSeek;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SemanticKernelDemo.DeepSeek;

/// <summary>
/// DeepSeek streaming demo showing reasoning content support.
/// </summary>
public class DeepSeekStreamingDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("======== DeepSeek Streaming Demo ========");

        string apiKey = "your-api-key-here";
        string modelId = "deepseek-v4-pro";

        var kernel = Kernel.CreateBuilder()
            .AddDeepSeekChatCompletion(
                modelId: modelId,
                apiKey: apiKey)
            .Build();

        var executionSettings = new DeepSeekPromptExecutionSettings
        {
            MaxTokens = 2000,
            ReasoningEffort = "high"
        };

        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage("9.11 和 9.8，哪个更大？请详细解释你的推理过程。");

        Console.WriteLine("User: 9.11 和 9.8，哪个更大？请详细解释你的推理过程。");
        Console.WriteLine();
        Console.WriteLine("Assistant (Streaming):");

        var fullContent = new System.Text.StringBuilder();
        var fullReasoningContent = new System.Text.StringBuilder();

        await foreach (var message in kernel.GetStreamingChatMessageContentAsync(
            chatHistory,
            executionSettings))
        {
            if (message is DeepSeekStreamingChatMessageContent deepSeekMessage)
            {
                if (!string.IsNullOrEmpty(deepSeekMessage.ReasoningContent))
                {
                    fullReasoningContent.Append(deepSeekMessage.ReasoningContent);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(deepSeekMessage.ReasoningContent);
                    Console.ResetColor();
                }

                if (!string.IsNullOrEmpty(message.Content))
                {
                    fullContent.Append(message.Content);
                    Console.Write(message.Content);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(message.Content))
                {
                    fullContent.Append(message.Content);
                    Console.Write(message.Content);
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("======== Full Reasoning Content ========");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(fullReasoningContent.ToString());
        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine("======== Final Answer ========");
        Console.WriteLine(fullContent.ToString());

        chatHistory.AddUserMessage("继续回答，草莓这个单词中有多少个R？");
        Console.WriteLine();
        Console.WriteLine("User: 草莓这个单词中有多少个R？");
        Console.WriteLine();
        Console.WriteLine("Assistant (Streaming):");

        fullContent.Clear();
        fullReasoningContent.Clear();

        await foreach (var message in kernel.GetStreamingChatMessageContentAsync(
            chatHistory,
            executionSettings))
        {
            if (message is DeepSeekStreamingChatMessageContent deepSeekMessage)
            {
                if (!string.IsNullOrEmpty(deepSeekMessage.ReasoningContent))
                {
                    fullReasoningContent.Append(deepSeekMessage.ReasoningContent);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(deepSeekMessage.ReasoningContent);
                    Console.ResetColor();
                }

                if (!string.IsNullOrEmpty(message.Content))
                {
                    fullContent.Append(message.Content);
                    Console.Write(message.Content);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(message.Content))
                {
                    fullContent.Append(message.Content);
                    Console.Write(message.Content);
                }
            }
        }

        Console.WriteLine();
    }

    public static async Task RunSimpleStreamingAsync()
    {
        Console.WriteLine("======== DeepSeek Simple Streaming Demo ========");

        string apiKey = "your-api-key-here";
        string modelId = "deepseek-v4-pro";

        var kernel = Kernel.CreateBuilder()
            .AddDeepSeekChatCompletion(
                modelId: modelId,
                apiKey: apiKey)
            .Build();

        var executionSettings = new DeepSeekPromptExecutionSettings
        {
            MaxTokens = 2000,
            ReasoningEffort = "high"
        };

        Console.WriteLine("User: 你好，请介绍一下你自己");
        Console.WriteLine();
        Console.WriteLine("Assistant (Streaming):");

        var fullContent = new System.Text.StringBuilder();

        await foreach (var content in kernel.GetStreamingChatCompletionAsync(
            "你好，请介绍一下你自己",
            executionSettings))
        {
            fullContent.Append(content);
            Console.Write(content);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Full response: {fullContent}");
    }

    public static async Task RunChatHistoryStreamingAsync()
    {
        Console.WriteLine("======== DeepSeek Chat History Streaming Demo ========");

        string apiKey = "your-api-key-here";
        string modelId = "deepseek-v4-pro";

        var kernel = Kernel.CreateBuilder()
            .AddDeepSeekChatCompletion(
                modelId: modelId,
                apiKey: apiKey)
            .Build();

        var executionSettings = new DeepSeekPromptExecutionSettings
        {
            MaxTokens = 2000,
            ReasoningEffort = "high"
        };

        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage("你好");

        Console.WriteLine("User: 你好");
        Console.WriteLine();
        Console.WriteLine("Assistant (Streaming):");

        var fullContent = new System.Text.StringBuilder();

        await foreach (var content in kernel.GetStreamingChatCompletionAsync(
            chatHistory,
            executionSettings))
        {
            fullContent.Append(content);
            Console.Write(content);
        }

        Console.WriteLine();
        Console.WriteLine();

        chatHistory.AddUserMessage("你能做什么？");
        Console.WriteLine("User: 你能做什么？");
        Console.WriteLine();
        Console.WriteLine("Assistant (Streaming):");

        fullContent.Clear();

        await foreach (var content in kernel.GetStreamingChatCompletionAsync(
            chatHistory,
            executionSettings))
        {
            fullContent.Append(content);
            Console.Write(content);
        }

        Console.WriteLine();
    }
}