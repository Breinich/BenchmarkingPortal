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
public class GetAllComputerGroupsQueryHandler : 
    IRequestHandler<GetAllComputerGroupsQuery, IEnumerable<ComputerGroupHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllComputerGroupsQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ComputerGroupHeader>> Handle(GetAllComputerGroupsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.ComputerGroups.Select(cG => new ComputerGroupHeader
        {
            Id = cG.Id,
            Description = cG.Description
        }).ToListAsync(cancellationToken);
    }
}