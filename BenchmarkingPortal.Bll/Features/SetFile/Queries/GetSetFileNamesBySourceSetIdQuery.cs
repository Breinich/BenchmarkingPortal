using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Queries;

public class GetSetFileNamesBySourceSetIdQuery : IRequest<IEnumerable<string>>
{
    public int SourceSetId { get; init; }
}