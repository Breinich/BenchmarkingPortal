using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

public class GetResultPathQuery : IRequest<string>
{
    public int BenchmarkId { get; init; }
}