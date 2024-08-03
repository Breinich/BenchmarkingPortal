using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SourceSet.QueryHandlers;

public class SourceSetExistsByNameQueryHandler(BenchmarkingDbContext context) : IRequestHandler<SourceSetExistsByNameQuery, bool>
{
    public async Task<bool> Handle(SourceSetExistsByNameQuery request, CancellationToken cancellationToken)
    {
        return await context.SourceSets.AnyAsync(x => x.Path == request.FileName, cancellationToken);
    }
}