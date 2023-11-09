using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Web;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetAllSetFileNamesQueryHandler : IRequestHandler<GetAllSetFileNamesQuery, IEnumerable<String>>
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