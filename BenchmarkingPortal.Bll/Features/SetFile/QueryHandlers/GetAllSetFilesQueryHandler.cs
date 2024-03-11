using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetAllSetFilesQueryHandler : IRequestHandler<GetAllSetFilesQuery, IEnumerable<SetFileHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllSetFilesQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<SetFileHeader>> Handle(GetAllSetFilesQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This feature is not fully implemented yet.");
        
        return await _context.SetFiles.Select(s => new SetFileHeader(s)).ToListAsync(cancellationToken);
    }
}