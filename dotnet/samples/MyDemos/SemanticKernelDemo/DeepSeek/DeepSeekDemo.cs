using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemanticKernelDemo.Lights;

namespace SemanticKernelDemo.DeepSeek
{
    internal class DeepSeekDemo
    {
        public static async Task Run(string[] args)
        {
            // Populate values from your OpenAI deployment
            var modelId = "deepseek-v3";
            //var endpoint = "https://api.lkeap.cloud.tencent.com/v1/";
            var endpoint = "https://api.lkeap.cloud.tencent.com";
            var apiKey = "sk-L7hpFlDbWTfRYVVTFHVsukGrS11BzRExwjYcTZCzeHs0AQyi";

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            OpenAIChatCompletionService chatCompletionService = new OpenAIChatCompletionService(
                 modelId,
                 new Uri(endpoint),
                apiKey
            );
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var chatHistory = new ChatHistory();
            chatHistory.AddUserMessage("Hello, how are you?");

            var reply = await chatCompletionService.GetChatMessageContentAsync(chatHistory).ConfigureAwait(false);
            Console.WriteLine(reply);
        }
    }
}
