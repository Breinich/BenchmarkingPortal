using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

public class GetNotFinishedBenchmarksQueryHandler : IRequestHandler<GetNotFinishedBenchmarksQuery, IEnumerable<BenchmarkHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetNotFinishedBenchmarksQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<BenchmarkHeader>> Handle(GetNotFinishedBenchmarksQuery request, CancellationToken cancellationToken) =>
    
        await _context.Benchmarks.Where(b => !b.Status.Equals(Status.Finished)).Select(b => new BenchmarkHeader(b)).ToListAsync(cancellationToken);
    
}