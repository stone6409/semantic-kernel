// Copyright (c) Microsoft. All rights reserved.

using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Microsoft.SemanticKernel.Connectors.DeepSeek;

/// <summary>
/// Provides extension methods for adding DeepSeek services to the kernel builder.
/// </summary>
public static class DeepSeekKernelBuilderExtensions
{
    /// <summary>
    /// Adds DeepSeek chat completion service to the kernel builder.
    /// </summary>
    /// <param name="builder">The kernel builder.</param>
    /// <param name="modelId">The model ID to use.</param>
    /// <param name="apiKey">The API key to use.</param>
    /// <param name="endpoint">The endpoint URL to use.</param>
    /// <param name="httpClient">The HTTP client to use.</param>
    /// <returns>The kernel builder.</returns>
    public static KernelBuilder AddDeepSeekChatCompletion(
        this KernelBuilder builder,
        string modelId,
        string apiKey,
        string endpoint = "https://api.deepseek.com/v1/chat/completions",
        HttpClient? httpClient = null)
    {
        builder.Services.AddDeepSeekChatCompletion(modelId, apiKey, endpoint, httpClient);
        return builder;
    }
}

/// <summary>
/// Provides extension methods for adding DeepSeek services to the service collection.
/// </summary>
public static class DeepSeekServiceCollectionExtensions
{
    /// <summary>
    /// Adds DeepSeek chat completion service to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="modelId">The model ID to use.</param>
    /// <param name="apiKey">The API key to use.</param>
    /// <param name="endpoint">The endpoint URL to use.</param>
    /// <param name="httpClient">The HTTP client to use.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDeepSeekChatCompletion(
        this IServiceCollection services,
        string modelId,
        string apiKey,
        string endpoint = "https://api.deepseek.com/v1/chat/completions",
        HttpClient? httpClient = null)
    {
        services.AddKeyedSingleton<IChatCompletionService>(modelId, (sp, key) =>
        {
            var client = httpClient ?? sp.GetService<HttpClient>();
            return new DeepSeekChatCompletionService(modelId, apiKey, endpoint, client);
        });

        return services;
    }
}
