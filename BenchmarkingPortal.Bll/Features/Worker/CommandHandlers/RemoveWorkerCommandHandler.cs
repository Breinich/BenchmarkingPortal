using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.CommandHandlers;

/// <summary>
/// Handler for <see cref="RemoveWorkerCommand"/>
/// </summary>
public class RemoveWorkerCommandHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly BenchmarkingDbContext _context;

    public RemoveWorkerCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public async Task Handle(RemoveWorkerCommand request, CancellationToken cancellationToken)
    {
        var worker = await _context.Workers.FindAsync(new object?[] { request.WorkerId }, 
                         cancellationToken: cancellationToken) ??
                     throw new ArgumentException(new ExceptionMessage<Dal.Entities.Worker>().ObjectNotFound);

        var workerDto = new WorkerHeader(worker);

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        
        throw new NotImplementedException("This feature is not fully implemented yet.");

        // Checks, whether the worker has any tasks running, and if not, remove it
        
        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        _context.Remove(worker);
        await _context.SaveChangesAsync(cancellationToken);
    }
}