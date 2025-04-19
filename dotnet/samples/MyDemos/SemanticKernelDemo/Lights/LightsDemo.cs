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

namespace SemanticKernelDemo.Lights
{
    internal class LightsDemo 
    {
        public static async Task Run(string[] args)
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            // Populate values from your OpenAI deployment
            var modelId = "deepseek-v3";
            //var endpoint = "https://api.lkeap.cloud.tencent.com/v1/";
            var endpoint = "https://api.lkeap.cloud.tencent.com";
            //var apiKey = "sk-L7hpFlDbWTfRYVVTFHVsukGrS11BzRExwjYcTZCzeHs0AQyi";
            var apiKey = config["TencentDeepSeek:ApiKey"];

            //var config = ConfigExtensions.FromConfig<OpenAIConfig>("OneApiSpark");

            var openAICustomHandler = new OpenAICustomHandler(endpoint);
            using HttpClient client = new(openAICustomHandler);

            // Create a kernel with Azure OpenAI chat completion
            var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion(modelId, apiKey, httpClient: client);

            // Add enterprise components
            builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

            // Build the kernel
            Kernel kernel = builder.Build();
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // Add a plugin (the LightsPlugin class is defined below)
            kernel.Plugins.AddFromType<LightsPlugin>("Lights");

            // Enable planning
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
                //MaxTokens = 4096,
                //Temperature = 0.7,
                //TopP = 1,
            };

            // Create a history store the conversation
            var history = new ChatHistory();

            // Initiate a back-and-forth chat
            string? userInput;
            do
            {
                // Collect user input
                Console.Write("User > ");
                userInput = Console.ReadLine();

                // Add user input
                history.AddUserMessage(userInput);

                // Get the response from the AI
                var result = await chatCompletionService.GetChatMessageContentAsync(
                    history,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: kernel).ConfigureAwait(false);

                // Print the results
                Console.WriteLine("Assistant > " + result);

                // Add the message from the agent to the chat history
                history.AddMessage(result.Role, result.Content ?? string.Empty);
            } while (userInput is not null);
        }
    }
}
