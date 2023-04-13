using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.CommandHandlers;

public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerHeader>
{
    private readonly BenchmarkingDbContext _context;

    public UpdateWorkerCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<WorkerHeader> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
    {
        var worker = await _context.Workers.FindAsync(request.WorkerId, cancellationToken) ??
                     throw new ArgumentException(new ExceptionMessage<Dal.Entities.Worker>().ObjectNotFound);
        

        var workerDto = new WorkerHeader(worker);
        workerDto.ComputerGroupId = request.ComputerGroupId;

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // Assign the worker to an other computer group

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // If succeeded, update the DB
        worker.ComputerGroupId = request.ComputerGroupId;

        await _context.SaveChangesAsync(cancellationToken);

        return workerDto;
    }
}