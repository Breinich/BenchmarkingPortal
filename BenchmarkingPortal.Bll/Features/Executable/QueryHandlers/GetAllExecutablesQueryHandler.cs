using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class GetAllExecutablesQueryHandler : IRequestHandler<GetAllExecutablesQuery, IEnumerable<ExecutableHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllExecutablesQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<ExecutableHeader>> Handle(GetAllExecutablesQuery request,
        CancellationToken cancellationToken) =>
        await _context.Executables.Select(e => new ExecutableHeader(e))
            .ToListAsync(cancellationToken);
}