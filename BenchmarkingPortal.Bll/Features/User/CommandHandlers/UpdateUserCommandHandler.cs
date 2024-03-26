using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.User.CommandHandlers;

/// <summary>
/// Handler for <see cref="UpdateUserCommand"/>
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserHeader>
{
    private readonly UserManager<Dal.Entities.User> _usermanager;

    public UpdateUserCommandHandler(UserManager<Dal.Entities.User> userManager)
    {
        _usermanager = userManager;
    }


    public async Task<UserHeader> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var invoker = await _usermanager.FindByNameAsync(request.InvokerName) ??
                      throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);

        if (await _usermanager.IsInRoleAsync(invoker, Roles.Admin))
        {
            var userEntity = await _usermanager.FindByNameAsync(request.UserName) ??
                             throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);

            userEntity.Subscription = request.Subscription;
            await _usermanager.UpdateAsync(userEntity);

            var userHeader = new UserHeader(userEntity);

            var currentRoles = await _usermanager.GetRolesAsync(userEntity);
            if (request.Role != null && !currentRoles.Contains(request.Role))
            {
                await _usermanager.RemoveFromRolesAsync(userEntity, currentRoles);
                await _usermanager.AddToRoleAsync(userEntity, request.Role);
                userHeader.Roles.Add(request.Role);
            }

            return userHeader;
        }

        throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.NoPrivilege);
    }
}