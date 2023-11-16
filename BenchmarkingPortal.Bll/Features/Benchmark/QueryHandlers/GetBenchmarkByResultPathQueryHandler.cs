using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

public class GetBenchmarkByResultPathQueryHandler : IRequestHandler<GetBenchmarkByResultPathQuery, BenchmarkHeader?>
{
    private readonly BenchmarkingDbContext _dbContext;
    
    public GetBenchmarkByResultPathQueryHandler(BenchmarkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<BenchmarkHeader?> Handle(GetBenchmarkByResultPathQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Benchmarks.Where(x => x.ResultPath == request.ResultPath)
            .Select(b => new BenchmarkHeader(b)).FirstOrDefaultAsync(cancellationToken);
    }
}