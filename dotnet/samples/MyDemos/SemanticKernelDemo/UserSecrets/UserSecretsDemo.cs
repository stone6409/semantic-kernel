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

namespace SemanticKernelDemo.UserSecrets
{
    internal class UserSecretsDemo
    {
        public static async Task Run(string[] args)
        {
            // 加载配置文件
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddUserSecrets<Program>() // 加载 secrets.json
            //    .Build();

            var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            var modelId = configuration["TencentDeepSeek:ModelId"];
            var endpoint = configuration["TencentDeepSeek:Endpoint"];
            var apiKey = configuration["TencentDeepSeek:ApiKey"];

            //var config = ConfigExtensions.FromConfig<OpenAIConfig>("OneApiSpark");

            var openAICustomHandler = new OpenAICustomHandler(endpoint);
            using HttpClient client = new(openAICustomHandler);

            // Create a kernel with Azure OpenAI chat completion
            var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion(modelId, apiKey, httpClient: client);

            // Add enterprise components
            //builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

            // Build the kernel
            Kernel kernel = builder.Build();
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // 接收用户入参
            string request = Console.ReadLine()!;
            // create prompt to the chat service
            string prompt = "这个请求的意图是什么? {{$request}}";

            // Create a kernel arguments object and add the  request
            var kernelArguments = new KernelArguments
            {
                { "request", request }
            };
            var streamingKernelContentsAsync = kernel.InvokePromptStreamingAsync(prompt, kernelArguments);
            await foreach (var content in streamingKernelContentsAsync)
            {
                Console.WriteLine(content);
            }
        }
    }
}
