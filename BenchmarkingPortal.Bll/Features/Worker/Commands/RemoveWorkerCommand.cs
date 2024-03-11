using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Commands;

/// <summary>
/// Command for removing a worker
/// </summary>
public class RemoveWorkerCommand : IRequest
{
    public int WorkerId { get; init; }
}