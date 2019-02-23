using System;
using Grpc.Core;

namespace CalculatorService
{
    public class Program
    {
        private const int Port = 50051;

        public static void Main(string[] args)
        {
            var server = new Server
            {
                Services = {Calculator.CalculatorService.BindService(new CalculatorServiceImpl())},
                Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)}
            };
            server.Start();

            Console.WriteLine($"Server listening on port {Port}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}