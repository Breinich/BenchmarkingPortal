using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class ExecutableExistsByNameQuery : IRequest<bool>
{
    public string FileName { get; init; } = null!;
}