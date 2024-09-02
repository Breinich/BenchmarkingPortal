using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

/// <summary>
/// Query handler for <see cref="SetFileExistsByNameQuery"/>.
/// </summary>
/// <param name="dbContext"> <see cref="BenchmarkingDbContext"/> instance. </param>
/// <param name="pathConfigs"> <see cref="PathConfigs"/> instance. </param>
public class SetFileExistsByNameQueryHandler(BenchmarkingDbContext dbContext, PathConfigs pathConfigs)
    : IRequestHandler<SetFileExistsByNameQuery, bool>
{
    
    /// <summary>
    /// Handle the <see cref="SetFileExistsByNameQuery"/>.
    /// </summary>
    /// <param name="request"> <see cref="SetFileExistsByNameQuery"/> instance. </param>
    /// <param name="cancellationToken"> <see cref="CancellationToken"/> instance. </param>
    /// <returns> <see cref="Task"/> representing the result of the operation. </returns>
    public async Task<bool> Handle(SetFileExistsByNameQuery request, CancellationToken cancellationToken)
    {
        var sourceSets = await dbContext.SourceSets.Select(s => new SourceSetHeader(s)).ToListAsync(cancellationToken);
        
        if (sourceSets.Count == 0)
            return false;

        return sourceSets.Any(set => Directory.Exists(Path.Join(
            pathConfigs.WorkingDir, set.UserName, pathConfigs.SourceSetDir, set.SetFilesDir, request.FileName
            )));
    }
}