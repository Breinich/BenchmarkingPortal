using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetBenchmarkByResultPathQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetBenchmarkByResultPathQueryHandler(BenchmarkingDbContext dbContext)
    : IRequestHandler<GetBenchmarkByResultPathQuery, BenchmarkHeader?>
{
    public async Task<BenchmarkHeader?> Handle(GetBenchmarkByResultPathQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Benchmarks.Where(x => x.ResultPath == request.ResultPath)
            .Select(b => new BenchmarkHeader(b)).FirstOrDefaultAsync(cancellationToken);
    }
}