using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.QueryHandlers;

public class GetExecutableToolNameByIdQueryHandler : IRequestHandler<GetExecutableToolNameByIdQuery, string?>
{
    private readonly BenchmarkingDbContext _context;

    public GetExecutableToolNameByIdQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<string?> Handle(GetExecutableToolNameByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Executables.Where(x => x.Id == request.Id).Select(x => x.OwnerTool).FirstOrDefaultAsync(cancellationToken);
    }
}