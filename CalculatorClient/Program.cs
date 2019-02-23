using System;
using Grpc.Core;
using Calculator;

namespace CalculatorClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new CalculatorService.CalculatorServiceClient(channel);

            var sumRequest = new SumRequest {FirstNumber = 3, SecondNumber = 5};
            var reply = client.Sum(sumRequest);

            Console.WriteLine($"{sumRequest.FirstNumber} + {sumRequest.SecondNumber} = {reply.Result}");

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}