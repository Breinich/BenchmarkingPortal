using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

public class DeleteExecutableCommandHandler : IRequestHandler<DeleteExecutableCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly string _workDir;
    private readonly IMediator _mediator;

    public DeleteExecutableCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
        StoragePaths storagePaths, IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _workDir = storagePaths.WorkingDir;
        _mediator = mediator;
    }


    public async Task Handle(DeleteExecutableCommand request, CancellationToken cancellationToken)
    {
        var exe = await _context.Executables.FindAsync(request.ExecutableId, cancellationToken) ??
                  throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().ObjectNotFound);

        if (exe.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().NoPrivilege);
        }
        
        if (exe.Path != request.FileId)
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().ObjectNotFound);

        var store = new CustomTusDiskStore(_workDir + Path.DirectorySeparatorChar + exe.UserName 
                                           + Path.DirectorySeparatorChar + "tools" , _mediator);
        await store.DeleteFileAsync(request.FileId, cancellationToken);
        
        _context.Remove(exe);
        await _context.SaveChangesAsync(cancellationToken);
    }
}