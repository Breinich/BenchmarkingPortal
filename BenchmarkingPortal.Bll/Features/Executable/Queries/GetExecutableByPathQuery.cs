using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class GetExecutableByPathQuery : IRequest<ExecutableHeader?>
{
    public string FileId { get; init; } = null!;
}