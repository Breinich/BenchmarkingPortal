using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

/// <summary>
/// Get set file names by source set id
/// </summary>
/// <param name="context"></param>
/// <param name="pathConfigs"></param>
public class GetSetFileNamesBySourceSetIdQueryHandler(BenchmarkingDbContext context, PathConfigs pathConfigs) : IRequestHandler<GetSetFileNamesBySourceSetIdQuery, IEnumerable<string>>
{
    
    /// <summary>
    /// Handle get set file names by source set id
    /// </summary>
    /// <param name="request"> Get set file names by source set id query </param>
    /// <param name="cancellationToken"> Cancellation token </param>
    /// <returns> Set file names </returns>
    /// <exception cref="ArgumentException"> Throws when source set not found </exception>
    public async Task<IEnumerable<string>> Handle(GetSetFileNamesBySourceSetIdQuery request, CancellationToken cancellationToken)
    {
        var sourceSet = await context.SourceSets.Where(s => s.Id == request.SourceSetId)
                            .Select(s => new SourceSetHeader(s)).FirstOrDefaultAsync(cancellationToken) 
                        ?? throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);

        if (string.IsNullOrWhiteSpace(sourceSet.SetFilesDir))
            throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);
        
        return [.. Directory.GetFiles(
            Path.Join(
                pathConfigs.WorkingDir, 
                sourceSet.UserName, 
                pathConfigs.SourceSetDir, 
                sourceSet.SetFilesDir
                ), "*.set", SearchOption.TopDirectoryOnly)
        ];
    }
}