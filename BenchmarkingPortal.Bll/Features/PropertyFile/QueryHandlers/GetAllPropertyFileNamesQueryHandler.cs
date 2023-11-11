using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using BenchmarkingPortal.Bll.Services;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.QueryHandlers;

public class GetAllPropertyFileNamesQueryHandler : IRequestHandler<GetAllPropertyFileNamesQuery, IEnumerable<string>>
{
    private readonly string _propertyFilesDir;
    
    public GetAllPropertyFileNamesQueryHandler(StoragePaths storagePaths)
    {
        _propertyFilesDir = storagePaths.PropertyFilesDir;
    }
    
    public Task<IEnumerable<string>> Handle(GetAllPropertyFileNamesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Directory.GetFiles( _propertyFilesDir, "*.prp", 
            SearchOption.TopDirectoryOnly).Select(s => s));
    }
}