using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class GetExecutableNameByIdQueryHandler : IRequestHandler<GetExecutableByIdQuery, string?>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetExecutableNameByIdQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public Task<string?> Handle(GetExecutableByIdQuery request, CancellationToken cancellationToken)
    {
        return _context.Executables.Where(e => e.Id == request.Id).Select(e => e.Name)
            .FirstOrDefaultAsync(cancellationToken);
    }
}