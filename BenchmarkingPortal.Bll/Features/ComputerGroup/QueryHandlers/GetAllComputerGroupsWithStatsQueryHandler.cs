using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.QueryHandlers;

public class GetAllComputerGroupsWithStatsQueryHandler : IRequestHandler<GetAllComputerGroupsWithStatsQuery, IEnumerable<ComputerGroupHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllComputerGroupsWithStatsQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<ComputerGroupHeader>> Handle(GetAllComputerGroupsWithStatsQuery request, CancellationToken cancellationToken)
    {
        var computerGroups = await _context.ComputerGroups
            .Include(x => x.Workers)
            .Include(x => x.Benchmarks)
            .Select(x => new ComputerGroupHeader(x)
            {
                BenchmarkCount = x.Benchmarks.Count,
                WorkerCount = x.Workers.Count
            })
            .ToListAsync(cancellationToken);
        
        return computerGroups;
    }
}