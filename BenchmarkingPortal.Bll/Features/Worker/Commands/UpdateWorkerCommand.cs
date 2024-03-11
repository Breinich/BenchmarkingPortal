using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Commands;

/// <summary>
/// Command for changing the computer group of a worker
/// </summary>
public class UpdateWorkerCommand : IRequest<WorkerHeader>
{
    public int WorkerId { get; init; }
    public int ComputerGroupId { get; init; }
}