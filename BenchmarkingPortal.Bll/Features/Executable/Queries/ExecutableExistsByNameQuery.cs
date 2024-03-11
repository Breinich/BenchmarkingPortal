using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

/// <summary>
/// Query if any executable with the given name exist
/// </summary>
public class ExecutableExistsByNameQuery : IRequest<bool>
{
    public string FileName { get; init; } = null!;
}