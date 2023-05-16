using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

public class DeleteExecutableCommandHandler : IRequestHandler<DeleteExecutableCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public DeleteExecutableCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task Handle(DeleteExecutableCommand request, CancellationToken cancellationToken)
    {
        var exe = await _context.Executables.FindAsync(request.ExecutableId, cancellationToken) ??
                  throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().ObjectNotFound);

        if (exe.User.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByIdAsync(request.InvokerName) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
            {
                throw new ArgumentException(new ExceptionMessage<Dal.Entities.Executable>().NoPrivilege);
            }
        }
        

        _context.Remove(exe);
        await _context.SaveChangesAsync(cancellationToken);
    }
}