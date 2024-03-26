using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetAllPropertyFileNamesBySourceSetQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetAllPropertyFileNamesBySourceSetQueryHandler : IRequestHandler<GetAllPropertyFileNamesBySourceSetQuery, IEnumerable<string?>>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetAllPropertyFileNamesBySourceSetQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public Task<IEnumerable<string?>> Handle(GetAllPropertyFileNamesBySourceSetQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This feature is not fully implemented yet.");
        
        var propertyRoot = _context.SourceSets
            .Where(x => x.Id == request.SourceSetId)
            .Select(x => new SourceSetHeader(x).PropertyFilesPath)
            .FirstOrDefault() ?? throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);
        
        return Task.FromResult(Directory.GetFiles(propertyRoot, "*.properties")
            .Select(Path.GetFileName));
    }
}