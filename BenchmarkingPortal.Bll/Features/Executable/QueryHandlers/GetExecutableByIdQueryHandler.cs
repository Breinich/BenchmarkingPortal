using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetExecutableByIdQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetExecutableByIdQueryHandler : IRequestHandler<GetExecutableByIdQuery, ExecutableHeader?>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetExecutableByIdQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public Task<ExecutableHeader?> Handle(GetExecutableByIdQuery request, CancellationToken cancellationToken)
    {
        return _context.Executables.Where(x => x.Id == request.Id)
            .Select(x => new ExecutableHeader(x))
            .FirstOrDefaultAsync(cancellationToken);
    }
}