using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

public class GetBenchmarkByResultPathQuery : IRequest<BenchmarkHeader?>
{
    public string ResultPath { get; init; } = null!;
}