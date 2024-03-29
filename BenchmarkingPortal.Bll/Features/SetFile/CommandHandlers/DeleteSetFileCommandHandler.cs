﻿using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using tusdotnet.Interfaces;

namespace BenchmarkingPortal.Bll.Features.SetFile.CommandHandlers;

public class DeleteSetFileCommandHandler : IRequestHandler<DeleteSetFileCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly IMediator _mediator;

    public DeleteSetFileCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
        PathConfigs pathConfigs, IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _mediator = mediator;
    }


    public async Task Handle(DeleteSetFileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This feature is not fully implemented yet.");
        
        var setFile = await _context.SetFiles.FindAsync(new object?[] { request.SetFileId },
                          cancellationToken: cancellationToken) ??
                        throw new ArgumentException(ExceptionMessage<Dal.Entities.SetFile>.ObjectNotFound);

        if (setFile.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(ExceptionMessage<Dal.Entities.SetFile>.NoPrivilege);
        }

        if (setFile.Path != request.FileId)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.SetFile>.ObjectNotFound);
        
        // ITusTerminationStore terminationStore = new CustomTusDiskStore(, _mediator);

        _context.Remove(setFile);
        await _context.SaveChangesAsync(cancellationToken);
        // await terminationStore.DeleteFileAsync(request.FileId, cancellationToken);
    }
}