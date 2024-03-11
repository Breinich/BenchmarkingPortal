using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

/// <summary>
/// Query an executable by its ID
/// </summary>
public class GetExecutableByIdQuery : IRequest<ExecutableHeader?>
{
    public int Id { get; init; }
}