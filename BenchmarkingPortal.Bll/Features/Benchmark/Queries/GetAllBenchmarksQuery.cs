using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

public class GetAllBenchmarksQuery : IRequest<IEnumerable<BenchmarkHeader>>
{
    public bool Finished { get; init; }
}