using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using tusdotnet.Interfaces;
using tusdotnet.Models;

namespace BenchmarkingPortal.Bll.Features.SourceSet.CommandHandlers;

public class DeleteSourceSetCommandHandler : IRequestHandler<DeleteSourceSetCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly ITusTerminationStore _terminationStore;

    public DeleteSourceSetCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
        DefaultTusConfiguration config)
    {
        _context = context;
        _userManager = userManager;
        _terminationStore = (ITusTerminationStore)config.Store;
    }


    public async Task Handle(DeleteSourceSetCommand request, CancellationToken cancellationToken)
    {
        var sourceSet = await _context.SourceSets.FindAsync(request.SourceSetId, cancellationToken) ??
                        throw new ArgumentException(new ExceptionMessage<Dal.Entities.SourceSet>().ObjectNotFound);

        if (sourceSet.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByIdAsync(request.InvokerName) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(new ExceptionMessage<Dal.Entities.SourceSet>().NoPrivilege);
        }

        _context.Remove(sourceSet);
        await _context.SaveChangesAsync(cancellationToken);
        await _terminationStore.DeleteFileAsync(request.FileId, cancellationToken);
    }
}