using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class GetExecutableToolNameQueryHandler : IRequestHandler<GetExecutableToolNameQuery, string?>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetExecutableToolNameQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public Task<string?> Handle(GetExecutableToolNameQuery request, CancellationToken cancellationToken)
    {
        return _context.Executables.Where(e => e.Id == request.Id).Select(e => e.OwnerTool)
            .FirstOrDefaultAsync(cancellationToken);
    }
}