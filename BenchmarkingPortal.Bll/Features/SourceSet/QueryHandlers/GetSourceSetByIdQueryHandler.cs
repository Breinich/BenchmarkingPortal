using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SourceSet.QueryHandlers;

public class GetSourceSetByIdQueryHandler(BenchmarkingDbContext context) : IRequestHandler<GetSourceSetByIdQuery, SourceSetHeader?>
{
    public async Task<SourceSetHeader?> Handle(GetSourceSetByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.SourceSets.Where(s => s.Id == request.Id).Select(s => new SourceSetHeader(s)).FirstOrDefaultAsync(cancellationToken);
    }
}