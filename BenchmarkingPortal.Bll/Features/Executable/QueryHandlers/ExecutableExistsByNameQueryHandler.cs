using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

/// <summary>
/// Handler for <see cref="ExecutableExistsByNameQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class ExecutableExistsByNameQueryHandler(BenchmarkingDbContext dbContext)
    : IRequestHandler<ExecutableExistsByNameQuery, bool>
{
    public async Task<bool> Handle(ExecutableExistsByNameQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Executables.AnyAsync(x => x.Path == request.FileName, cancellationToken);
    }
}