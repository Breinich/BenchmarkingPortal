using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class GetAllExecutableNamesQueryHandler : IRequestHandler<GetAllExecutableNamesQuery, IEnumerable<string>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllExecutableNamesQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<string>> Handle(GetAllExecutableNamesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Executables.Select(e => e.Name+":"+e.Version).ToListAsync(cancellationToken);
    }
}