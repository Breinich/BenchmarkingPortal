using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class SetFileExistsByNameQueryHandler : IRequestHandler<SetFileExistsByNameQuery, bool>
{
    private readonly BenchmarkingDbContext _dbContext;
    
    public SetFileExistsByNameQueryHandler(BenchmarkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<bool> Handle(SetFileExistsByNameQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.SetFiles.AnyAsync(x => x.Version + "+" + x.Name + ".set" == request.FileName, cancellationToken);
    }
}