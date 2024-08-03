using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class SetFileExistsByNameQueryHandler(BenchmarkingDbContext dbContext, PathConfigs pathConfigs)
    : IRequestHandler<SetFileExistsByNameQuery, bool>
{
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