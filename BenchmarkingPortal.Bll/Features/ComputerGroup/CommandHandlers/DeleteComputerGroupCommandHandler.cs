using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.CommandHandlers;

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
        if (user == null || !(await _userManager.IsInRoleAsync(user, Roles.Admin)))
        {
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.ComputerGroup>().NoPrivilege);
        }

        var computerGroup = await _context.ComputerGroups.FindAsync(request.Id);
        _context.ComputerGroups.Remove(computerGroup ?? 
                                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.ComputerGroup>()
                                           .ObjectNotFound));
    }
}