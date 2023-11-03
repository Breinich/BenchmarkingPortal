using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using tusdotnet.Interfaces;
using tusdotnet.Models;

namespace BenchmarkingPortal.Bll.Features.SetFile.CommandHandlers;

public class DeleteSetFileCommandHandler : IRequestHandler<DeleteSetFileCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly ITusTerminationStore _terminationStore;

    public DeleteSetFileCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
        DefaultTusConfiguration config)
    {
        _context = context;
        _userManager = userManager;
        _terminationStore = (ITusTerminationStore)config.Store;
    }


    public async Task Handle(DeleteSetFileCommand request, CancellationToken cancellationToken)
    {
        var setFile = await _context.SetFiles.FindAsync(request.SetFileId, cancellationToken) ??
                        throw new ArgumentException(new ExceptionMessage<Dal.Entities.SetFile>().ObjectNotFound);

        if (setFile.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByIdAsync(request.InvokerName) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(new ExceptionMessage<Dal.Entities.SetFile>().NoPrivilege);
        }

        _context.Remove(setFile);
        await _context.SaveChangesAsync(cancellationToken);
        await _terminationStore.DeleteFileAsync(request.FileId, cancellationToken);
    }
}