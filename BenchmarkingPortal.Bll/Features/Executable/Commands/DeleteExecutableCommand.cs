using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Commands;

public class DeleteExecutableCommand : IRequest
{
    public int ExecutableId { get; set; }
    public int UserId { get; set; }
}