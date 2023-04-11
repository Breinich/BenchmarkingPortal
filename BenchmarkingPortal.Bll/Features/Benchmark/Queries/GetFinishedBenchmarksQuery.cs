using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

public class GetFinishedBenchmarksQuery : IRequest<IEnumerable<BenchmarkHeader>>
{
    public int UserId { get; set; }
}