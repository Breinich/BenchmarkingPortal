using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Commands;

public class AddWorkerCommand : IRequest<WorkerHeader>
{
    public string Name { get; set; } = null!;
    public int Ram { get; set; }
    public int Cpu { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Storage { get; set; }
    public string Address { get; set; } = null!;
    public int Port { get; set; }
    public int ComputerGroupId { get; set; }
    public DateTime AddedDate { get; set; }
}