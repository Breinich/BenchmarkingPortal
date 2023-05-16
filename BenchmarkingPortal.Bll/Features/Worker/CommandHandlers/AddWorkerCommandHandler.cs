using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.CommandHandlers;

public class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand, WorkerHeader>
{
    private readonly BenchmarkingDbContext _context;

    public AddWorkerCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<WorkerHeader> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var workerDto = new WorkerHeader()
        {
            AddedDate = request.AddedDate,
            Address = request.Address,
            Port = request.Port,
            ComputerGroupId = request.ComputerGroupId,
            Cpu = request.Cpu,
            Ram = request.Ram,
            Storage = request.Storage,
            Name = request.Name,
            Login = request.Username,
            Password = request.Password,
        };

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // The scheduler connects to the VM and checks the provided data (ram, cpu, storage, username, password)

        // If it succeeds, the worker will be written into the DB

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        var worker = new Dal.Entities.Worker()
        {
            AddedDate = request.AddedDate,
            Address = request.Address,
            Port = request.Port,
            ComputerGroupId = request.ComputerGroupId,
            Cpu = request.Cpu,
            Ram = request.Ram,
            Storage = request.Storage,
            Name = request.Name,
            Login = request.Username,
            Password = request.Password,
        };

        await _context.Workers.AddAsync(worker, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new WorkerHeader(worker);
    }
}