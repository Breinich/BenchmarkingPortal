using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.User.CommandHandlers;

/// <summary>
/// Handler for <see cref="DeleteUserCommand"/>
/// </summary>
public class DeleteUserCommandHandler(UserManager<Dal.Entities.User> userManager, BenchmarkingDbContext context)
    : IRequestHandler<DeleteUserCommand>
{
    
    /// <summary>
    /// Deletes a user from the database
    /// </summary>
    /// <param name="request"> <see cref="DeleteUserCommand"/> </param>
    /// <param name="cancellationToken"> <see cref="CancellationToken"/> </param>
    /// <exception cref="ArgumentException"> If the invoker does not have the privilege to delete a user </exception>
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var invoker = await userManager.FindByNameAsync(request.InvokerName);

        if (invoker != null && await userManager.IsInRoleAsync(invoker, Roles.Admin))
        {
            var user = await context.Users.Include(u => u.Executables).Include(u => u.SetFiles)
                           .Include(u => u.Benchmarks).Where(u => u.UserName == request.UserName)
                           .FirstOrDefaultAsync(cancellationToken) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);

            foreach (var executable in user.Executables) executable.User = invoker;

            foreach (var setFile in user.SetFiles) setFile.User = invoker;

            foreach (var benchmark in user.Benchmarks) benchmark.User = invoker;

            await context.SaveChangesAsync(cancellationToken);

            await userManager.DeleteAsync(user);
        }
        else
        {
            throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.NoPrivilege);
        }
    }
}