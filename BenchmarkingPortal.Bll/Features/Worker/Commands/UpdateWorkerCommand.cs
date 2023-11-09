using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Commands;

public class UpdateWorkerCommand : IRequest<WorkerHeader>
{
    public int WorkerId { get; init; }
    public int ComputerGroupId { get; init; }
}