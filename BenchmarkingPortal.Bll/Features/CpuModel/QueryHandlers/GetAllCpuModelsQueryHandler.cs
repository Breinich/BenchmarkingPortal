using BenchmarkingPortal.Bll.Features.CpuModel.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.CpuModel.QueryHandlers;

public class GetAllCpuModelsQueryHandler : IRequestHandler<GetAllCpuModelsQuery, IEnumerable<CpuModelHeader>>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetAllCpuModelsQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public Task<IEnumerable<CpuModelHeader>> Handle(GetAllCpuModelsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Needs a migration to let the CpuModels DBSet be available in the DB context."
            );
    }
}