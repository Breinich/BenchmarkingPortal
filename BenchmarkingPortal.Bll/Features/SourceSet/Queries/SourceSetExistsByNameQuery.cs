using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Queries;

public class SourceSetExistsByNameQuery : IRequest<bool>
{
    public string FileName { get; init; } = null!;
}