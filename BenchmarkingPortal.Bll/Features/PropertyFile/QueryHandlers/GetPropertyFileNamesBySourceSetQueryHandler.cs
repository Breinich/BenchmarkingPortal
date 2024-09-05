using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetPropertyFileNamesBySourceSetQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetPropertyFileNamesBySourceSetQueryHandler(BenchmarkingDbContext context, PathConfigs pathConfigs)
    : IRequestHandler<GetPropertyFileNamesBySourceSetQuery, IEnumerable<string?>>
{
    public async Task<IEnumerable<string?>> Handle(GetPropertyFileNamesBySourceSetQuery request, CancellationToken cancellationToken)
    {
        var sourceSet = await context.SourceSets
            .Where(x => x.Id == request.SourceSetId)
            .Select(x => new SourceSetHeader(x))
            .FirstOrDefaultAsync(cancellationToken) 
                        ?? throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);

        return [.. Directory.GetFiles(Path.Join(
                pathConfigs.WorkingDir, 
                sourceSet.UserName, 
                pathConfigs.SourceSetDir, 
                sourceSet.PropertyFilesPath), "*.prp", SearchOption.TopDirectoryOnly)];
    }
}