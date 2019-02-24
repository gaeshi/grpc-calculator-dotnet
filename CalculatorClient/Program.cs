using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Calculator;

namespace CalculatorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new CalculatorService.CalculatorServiceClient(channel);

            SumDemo(client);
            await PrimeNumberDecompositionDemo(client);
            await ComputeAverageDemo(client);

            await Shutdown(channel);
        }

        private static void SumDemo(CalculatorService.CalculatorServiceClient client)
        {
            var sumRequest = new SumRequest {FirstNumber = 3, SecondNumber = 5};
            var reply = client.Sum(sumRequest);
            Console.WriteLine($"{sumRequest.FirstNumber} + {sumRequest.SecondNumber} = {reply.Result}");
        }

        private static async Task PrimeNumberDecompositionDemo(CalculatorService.CalculatorServiceClient client)
        {
            var call = client.PrimeNumberDecomposition(
                new PrimeNumberDecompositionRequest {Number = int.MaxValue - 1});
            var responseStream = call.ResponseStream;

            while (await responseStream.MoveNext())
            {
                Console.WriteLine(responseStream.Current.PrimeFactor);
            }
        }

        private static async Task ComputeAverageDemo(CalculatorService.CalculatorServiceClient client)
        {
            var call = client.ComputeAverage();
            int[] numbers = {95832, 72649, 56268, 25876, 5155, 7552, 40899, 75955, 75854, 75380};
            foreach (var number in numbers)
            {
                await call.RequestStream.WriteAsync(new ComputeAverageRequest {Number = number});
            }

            await call.RequestStream.CompleteAsync();

            var callResponseAsync = await call.ResponseAsync;
            Console.WriteLine($"Average is {callResponseAsync.Average}");
        }

        private static async Task Shutdown(Channel channel)
        {
            await channel.ShutdownAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}