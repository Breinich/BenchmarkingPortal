using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using tusdotnet.Interfaces;
using tusdotnet.Models;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

public class DeleteExecutableCommandHandler : IRequestHandler<DeleteExecutableCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly ITusTerminationStore _terminationStore;

    public DeleteExecutableCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
        DefaultTusConfiguration config)
    {
        _context = context;
        _userManager = userManager;
        _terminationStore = (ITusTerminationStore)config.Store;
    }


    public async Task Handle(DeleteExecutableCommand request, CancellationToken cancellationToken)
    {
        var exe = await _context.Executables.FindAsync(request.ExecutableId, cancellationToken) ??
                  throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().ObjectNotFound);

        if (exe.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByIdAsync(request.InvokerName) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().NoPrivilege);
        }


        _context.Remove(exe);
        await _context.SaveChangesAsync(cancellationToken);
        await _terminationStore.DeleteFileAsync(request.FileId, cancellationToken);
    }
}