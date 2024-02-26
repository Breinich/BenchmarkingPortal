using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

/// <summary>
/// Query to get all benchmarks by result path.
/// </summary>
public class GetBenchmarkByResultPathQuery : IRequest<BenchmarkHeader?>
{
    public string ResultPath { get; init; } = null!;
}