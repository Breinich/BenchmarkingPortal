using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetSetFileNamesBySourceSetIdQueryHandler(BenchmarkingDbContext context, PathConfigs pathConfigs) : IRequestHandler<GetSetFileNamesBySourceSetIdQuery, IEnumerable<string>>
{
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