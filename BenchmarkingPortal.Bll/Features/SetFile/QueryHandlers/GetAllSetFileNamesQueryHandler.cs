using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetAllSetFileNamesQueryHandler : IRequestHandler<GetAllSetFileNamesQuery, IEnumerable<String>>
{
    public Task<IEnumerable<string>> Handle(GetAllSetFileNamesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}