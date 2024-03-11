using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetAllSetFileNamesQueryHandler : IRequestHandler<GetAllSetFileNamesQuery, IEnumerable<string>>
{
    
    public GetAllSetFileNamesQueryHandler()
    {
    }
    
    public Task<IEnumerable<string>> Handle(GetAllSetFileNamesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        //return Task.FromResult(Directory.GetFiles(_setFilesDir, "*.set", SearchOption.TopDirectoryOnly).Select(s => s));
    }
}