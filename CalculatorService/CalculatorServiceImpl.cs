using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Calculator;
using Grpc.Core;

namespace CalculatorService
{
    public class CalculatorServiceImpl : Calculator.CalculatorService.CalculatorServiceBase
    {
        public override Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SumResponse {Result = request.FirstNumber + request.SecondNumber});
        }

        public override async Task PrimeNumberDecomposition(PrimeNumberDecompositionRequest request,
            IServerStreamWriter<PrimeNumberDecompositionResponse> responseStream,
            ServerCallContext context)
        {
            var number = request.Number;
            var divisor = 2;

            while (number > 1)
            {
                if (number % divisor == 0)
                {
                    number /= divisor;
                    await responseStream.WriteAsync(new PrimeNumberDecompositionResponse {PrimeFactor = divisor});
                }
                else
                {
                    divisor++;
                }
            }
        }

        public override async Task<ComputeAverageResponse> ComputeAverage(
            IAsyncStreamReader<ComputeAverageRequest> requestStream, ServerCallContext context)
        {
            decimal average = 0;
            var n = 0;
            const int maxN = int.MaxValue;

            while (await requestStream.MoveNext())
            {
                average = (average * n + requestStream.Current.Number) / (n + 1);
                if (n == maxN)
                {
                    throw new OverflowException($"The maximum number of input elements supported is {maxN}");
                }

                n += 1;
            }

            return new ComputeAverageResponse {Average = (double) average};
        }

        public override async Task FindMaximum(
            IAsyncStreamReader<FindMaximumRequest> requestStream,
            IServerStreamWriter<FindMaximumResponse> responseStream,
            ServerCallContext context)
        {
            var currentMaximum = int.MinValue;
            while (await requestStream.MoveNext())
            {
                if (currentMaximum >= requestStream.Current.Number)
                {
                    continue;
                }

                currentMaximum = requestStream.Current.Number;
                await responseStream.WriteAsync(new FindMaximumResponse {Number = currentMaximum});
            }
        }

        public override Task<SquareRootResponse> SquareRoot(SquareRootRequest request, ServerCallContext context)
        {
            var number = request.Number;
            if (number < 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The number being sent is not positive"),
                    $"Number received: {number}");
            }

            var root = Math.Sqrt(number);
            return Task.FromResult(new SquareRootResponse {NumberRoot = root});
        }
    }
}