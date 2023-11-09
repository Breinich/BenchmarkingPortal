using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Commands;

public class RemoveWorkerCommand : IRequest
{
    public int WorkerId { get; init; }
}