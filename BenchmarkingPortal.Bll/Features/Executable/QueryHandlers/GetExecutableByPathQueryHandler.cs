using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class GetExecutableByPathQueryHandler : IRequestHandler<GetExecutableByPathQuery, ExecutableHeader?>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetExecutableByPathQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public Task<ExecutableHeader?> Handle(GetExecutableByPathQuery request, CancellationToken cancellationToken)
    {
        return _context.Executables
            .Where(e => e.Path == request.FileId).Select(e => new ExecutableHeader(e))
            .FirstOrDefaultAsync(cancellationToken);
    }
}