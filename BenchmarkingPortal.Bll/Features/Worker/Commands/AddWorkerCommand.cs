using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Commands;

public class AddWorkerCommand : IRequest<WorkerHeader>
{
    public string Name { get; init; } = null!;
    public int Ram { get; init; }
    public int Cpu { get; init; }
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public int Storage { get; init; }
    public string Address { get; init; } = null!;
    public int Port { get; init; }
    public int ComputerGroupId { get; init; }
    public DateTime AddedDate { get; init; }
    public string InvokerName { get; init; } = null!;
}