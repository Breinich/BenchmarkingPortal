using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class ExecutableExistsByNameQueryHandler : IRequestHandler<ExecutableExistsByNameQuery, bool>
{
    private readonly BenchmarkingDbContext _dbContext;
    
    public ExecutableExistsByNameQueryHandler(BenchmarkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<bool> Handle(ExecutableExistsByNameQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.Executables.AnyAsync(x => x.Version + "+" + x.Name + ".zip" == request.FileName, cancellationToken);
    }
}