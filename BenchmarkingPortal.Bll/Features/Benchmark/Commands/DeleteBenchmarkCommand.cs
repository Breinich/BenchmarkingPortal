using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

/// <summary>
/// The command for deleting a benchmark.
/// </summary>
public class DeleteBenchmarkCommand : IRequest
{
    public int Id { get; init; }

    public string InvokerName { get; init; } = null!;
}