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
    }
}