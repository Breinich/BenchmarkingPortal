using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

public class DeleteBenchmarkCommand : IRequest
{
    public int Id { get; init; }

    public string InvokerName { get; init; } = null!;
}