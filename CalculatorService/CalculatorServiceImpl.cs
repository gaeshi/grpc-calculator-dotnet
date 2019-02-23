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
    }
}