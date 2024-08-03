using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SourceSet.QueryHandlers;

public class GetAllSourceSetsQueryHandler(BenchmarkingDbContext context) 
    : IRequestHandler<GetAllSourceSetsQuery, IEnumerable<SourceSetHeader>>
{
    public async Task<IEnumerable<SourceSetHeader>> Handle(GetAllSourceSetsQuery request, CancellationToken cancellationToken)
    {
        return await context.SourceSets.Select(s => new SourceSetHeader(s)).ToListAsync(cancellationToken);
    }
}