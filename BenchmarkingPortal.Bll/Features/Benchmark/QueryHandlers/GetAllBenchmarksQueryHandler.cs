using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

/// <summary>
/// Defines a handler for the <see cref="GetAllBenchmarksQuery" /> request.
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetAllBenchmarksQueryHandler : IRequestHandler<GetAllBenchmarksQuery, IEnumerable<BenchmarkHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllBenchmarksQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BenchmarkHeader>> Handle(GetAllBenchmarksQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Benchmarks.Where(b => b.Status.Equals(Status.Finished) == request.Finished)
            .Select(b => new BenchmarkHeader(b)).ToListAsync(cancellationToken);
    }
}