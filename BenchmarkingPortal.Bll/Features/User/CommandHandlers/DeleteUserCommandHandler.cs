﻿using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.User.CommandHandlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly UserManager<Dal.Entities.User> _userManager;

    public DeleteUserCommandHandler(UserManager<Dal.Entities.User> userManager)
    {
        _userManager = userManager;
    }


    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var invoker = await _userManager.FindByNameAsync(request.InvokerName);
        
        if (invoker != null && await _userManager.IsInRoleAsync(invoker, Roles.Admin))
        {
            var user = await _userManager.FindByNameAsync(request.UserName) ?? 
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);
            
            await _userManager.DeleteAsync(user);
        }
        else
        {
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().NoPrivilege);
        }
    }
}