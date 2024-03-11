using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

/// <summary>
/// Query an executable by its path
/// </summary>
public class GetExecutableByPathQuery : IRequest<ExecutableHeader?>
{
    public string FileId { get; init; } = null!;
}