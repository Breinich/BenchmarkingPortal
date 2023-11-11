using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetAllSetFileNamesQueryHandler : IRequestHandler<GetAllSetFileNamesQuery, IEnumerable<string>>
{
    private readonly string _setFilesDir;
    
    public GetAllSetFileNamesQueryHandler(StoragePaths storagePaths)
    {
        _setFilesDir = storagePaths.SetFilesDir;
    }
    
    public Task<IEnumerable<string>> Handle(GetAllSetFileNamesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Directory.GetFiles(_setFilesDir, "*.set", SearchOption.TopDirectoryOnly)
            .Select(s => s));
    }
}