using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Queries;

public class GetAllSetFileNamesQuery : IRequest<IEnumerable<string>>
{
}