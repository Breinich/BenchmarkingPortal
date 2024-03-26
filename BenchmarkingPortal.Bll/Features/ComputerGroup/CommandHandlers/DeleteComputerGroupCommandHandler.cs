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
public class DeleteComputerGroupCommandHandler : IRequestHandler<DeleteComputerGroupCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public DeleteComputerGroupCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task Handle(DeleteComputerGroupCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.InvokerName);
        if (user == null || !await _userManager.IsInRoleAsync(user, Roles.Admin))
            throw new ArgumentException(ExceptionMessage<Dal.Entities.ComputerGroup>.NoPrivilege);

        var computerGroup = await _context.ComputerGroups.FindAsync(new object?[] { request.Id}, 
            cancellationToken: cancellationToken);

        var workersCount = await _context.Workers
            .CountAsync(w => w.ComputerGroupId == request.Id, cancellationToken);
        var benchmarksCount = await _context.Benchmarks
            .CountAsync(b => b.ComputerGroupId == request.Id, cancellationToken);

        if (benchmarksCount > 0)
            throw new ArgumentException(
                "Cannot delete computer group with running benchmarks, first please wait for them to finish!");

        if (workersCount > 0)
            throw new ArgumentException(
                "Cannot delete computer group with attached workers, first please move them to another group!");

        _context.ComputerGroups.Remove(computerGroup ??
                                       throw new ArgumentException( ExceptionMessage<Dal.Entities.ComputerGroup>.ObjectNotFound));

        await _context.SaveChangesAsync(cancellationToken);
    }
}