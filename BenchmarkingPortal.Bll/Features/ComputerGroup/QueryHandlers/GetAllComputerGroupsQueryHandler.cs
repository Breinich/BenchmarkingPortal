using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetAllComputerGroupsQuery" /> to get all computer groups.
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetAllComputerGroupsQueryHandler(BenchmarkingDbContext context) :
    IRequestHandler<GetAllComputerGroupsQuery, IEnumerable<ComputerGroupHeader>>
{
    public async Task<IEnumerable<ComputerGroupHeader>> Handle(GetAllComputerGroupsQuery request,
        CancellationToken cancellationToken)
    {
        return await context.ComputerGroups.Select(cG => new ComputerGroupHeader
        {
            Id = cG.Id,
            Description = cG.Description
        }).ToListAsync(cancellationToken);
    }
}