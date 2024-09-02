using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.CommandHandlers;

/// <summary>
/// Command handler for the <see cref="DeleteComputerGroupCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class DeleteComputerGroupCommandHandler(
    BenchmarkingDbContext context,
    UserManager<Dal.Entities.User> userManager)
    : IRequestHandler<DeleteComputerGroupCommand>
{
    
    /// <summary>
    /// Handles the <see cref="DeleteComputerGroupCommand"/>
    /// </summary>
    /// <param name="request"> The computer group to delete </param>
    /// <param name="cancellationToken"> The token to monitor for cancellation requests </param>
    /// <exception cref="ArgumentException"> Thrown when the invoker is not an admin </exception>
    public async Task Handle(DeleteComputerGroupCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.InvokerName);
        if (user == null || !await userManager.IsInRoleAsync(user, Roles.Admin))
            throw new ArgumentException(ExceptionMessage<Dal.Entities.ComputerGroup>.NoPrivilege);

        var computerGroup = await context.ComputerGroups.FindAsync(new object?[] { request.Id}, 
            cancellationToken: cancellationToken);

        var workersCount = await context.Workers
            .CountAsync(w => w.ComputerGroupId == request.Id, cancellationToken);
        var benchmarksCount = await context.Benchmarks
            .CountAsync(b => b.ComputerGroupId == request.Id, cancellationToken);

        if (benchmarksCount > 0 || workersCount > 0)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.ComputerGroup>.InUse);

        context.ComputerGroups.Remove(computerGroup ??
                                       throw new ArgumentException( ExceptionMessage<Dal.Entities.ComputerGroup>.ObjectNotFound));

        await context.SaveChangesAsync(cancellationToken);
    }
}