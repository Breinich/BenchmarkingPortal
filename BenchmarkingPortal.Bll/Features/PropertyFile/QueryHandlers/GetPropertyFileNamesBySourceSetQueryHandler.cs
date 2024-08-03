using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetPropertyFileNamesBySourceSetQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetPropertyFileNamesBySourceSetQueryHandler(BenchmarkingDbContext context)
    : IRequestHandler<GetPropertyFileNamesBySourceSetQuery, IEnumerable<string?>>
{
    public async Task<IEnumerable<string?>> Handle(GetPropertyFileNamesBySourceSetQuery request, CancellationToken cancellationToken)
    {
        var propertyRoot = await context.SourceSets
            .Where(x => x.Id == request.SourceSetId)
            .Select(x => new SourceSetHeader(x).PropertyFilesPath)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (string.IsNullOrWhiteSpace(propertyRoot))
            throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);

        return [.. Directory.GetFiles(propertyRoot, "*.properties", SearchOption.TopDirectoryOnly)];
    }
}