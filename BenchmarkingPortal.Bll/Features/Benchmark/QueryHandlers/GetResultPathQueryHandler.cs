using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

public class GetResultPathQueryHandler : IRequestHandler<GetResultPathQuery, string>
{
    private readonly BenchmarkingDbContext _context;

    public GetResultPathQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetResultPathQuery request, CancellationToken cancellationToken)
    {
        var path = await _context.Benchmarks.Where(b => b.Id == request.BenchmarkId && b.Status == Status.Finished)
            .Select(b => b.Result).FirstOrDefaultAsync(cancellationToken);

        if (path == null)
        {
            throw new ArgumentException(
                "Either the benchmark doesn't exist, or the benchmark hasn't been finished yet.");
        }

        return path;
    }
}