using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

public class DeleteBenchmarkCommand : IRequest
{
    public int Id { get; set; }

    public string InvokerName { get; set; }
}