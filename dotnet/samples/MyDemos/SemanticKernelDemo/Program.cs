using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelDemo.DeepSeek;
using SemanticKernelDemo.Lights;
using SemanticKernelDemo.UserSecrets;
using System.Net;

namespace SemanticKernelDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            _ = DeepSeekDemo.Run(args);
            //_ = LightsDemo.Run(args);
            //_ = UserSecretsDemo.Run(args);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
