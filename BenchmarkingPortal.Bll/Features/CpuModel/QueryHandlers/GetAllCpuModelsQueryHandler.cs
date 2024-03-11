using BenchmarkingPortal.Bll.Features.CpuModel.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.CpuModel.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetAllCpuModelsQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetAllCpuModelsQueryHandler : IRequestHandler<GetAllCpuModelsQuery, IEnumerable<CpuModelHeader>>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetAllCpuModelsQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CpuModelHeader>> Handle(GetAllCpuModelsQuery request, CancellationToken cancellationToken)
    {
        return await _context.CpuModels.Select(c => new CpuModelHeader(c)).ToListAsync(cancellationToken);
    }
}