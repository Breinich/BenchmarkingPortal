using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Commands;

public class DeleteExecutableCommand : IRequest
{
    public int ExecutableId { get; init; }
    public string InvokerName { get; init; } = null!;
    public string FileId { get; init; } = null!;
}