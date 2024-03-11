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
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public DeleteUserCommandHandler(UserManager<Dal.Entities.User> userManager, BenchmarkingDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }


    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var invoker = await _userManager.FindByNameAsync(request.InvokerName);

        if (invoker != null && await _userManager.IsInRoleAsync(invoker, Roles.Admin))
        {
            var user = await _context.Users.Include(u => u.Executables).Include(u => u.SetFiles)
                           .Include(u => u.Benchmarks).Where(u => u.UserName == request.UserName)
                           .FirstOrDefaultAsync(cancellationToken) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            foreach (var executable in user.Executables) executable.User = invoker;

            foreach (var setFile in user.SetFiles) setFile.User = invoker;

            foreach (var benchmark in user.Benchmarks) benchmark.User = invoker;

            await _context.SaveChangesAsync(cancellationToken);

            await _userManager.DeleteAsync(user);
        }
        else
        {
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().NoPrivilege);
        }
    }
}