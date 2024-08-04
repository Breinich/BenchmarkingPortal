using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetAllComputerGroupsWithStatsQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetAllComputerGroupsWithStatsQueryHandler(BenchmarkingDbContext context) :
    IRequestHandler<GetAllComputerGroupsWithStatsQuery, IEnumerable<ComputerGroupHeader>>
{
    public async Task<IEnumerable<ComputerGroupHeader>> Handle(GetAllComputerGroupsWithStatsQuery request,
        CancellationToken cancellationToken)
    {
        var computerGroups = await context.ComputerGroups
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