using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

/// <summary>
/// Query to get all benchmarks
/// </summary>
public class GetAllBenchmarksQuery : IRequest<IEnumerable<BenchmarkHeader>>
{
    public bool Finished { get; init; }
}