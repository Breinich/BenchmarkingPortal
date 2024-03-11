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
        var worker = await _context.Workers.FindAsync(new object?[] { request.WorkerId }, 
                         cancellationToken: cancellationToken) ??
                     throw new ArgumentException(new ExceptionMessage<Dal.Entities.Worker>().ObjectNotFound);


        var workerDto = new WorkerHeader(worker)
        {
            ComputerGroupId = request.ComputerGroupId
        };

        // If succeeded, update the DB
        worker.ComputerGroupId = request.ComputerGroupId;

        await _context.SaveChangesAsync(cancellationToken);

        return workerDto;
    }
}