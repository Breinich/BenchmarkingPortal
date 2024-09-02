using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

/// <summary>
/// Query handler for getting all set files.
/// </summary>
/// <param name="context"> BenchmarkingDbContext instance. </param>
public class GetAllSetFilesQueryHandler(BenchmarkingDbContext context)
    : IRequestHandler<GetAllSetFilesQuery, IEnumerable<SetFileHeader>>
{
    
    /// <summary>
    /// Handle method for getting all set files.
    /// </summary>
    /// <param name="request"> GetAllSetFilesQuery instance. </param>
    /// <param name="cancellationToken"> CancellationToken instance. </param>
    /// <returns></returns>
    public async Task<IEnumerable<SetFileHeader>> Handle(GetAllSetFilesQuery request,
        CancellationToken cancellationToken)
    {   
        return await context.SetFiles.Select(s => new SetFileHeader(s)).ToListAsync(cancellationToken);
    }
}