using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.CommandHandlers;

public class UpdateComputerGroupCommandHandler : IRequestHandler<UpdateComputerGroupCommand, ComputerGroupHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public UpdateComputerGroupCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<ComputerGroupHeader> Handle(UpdateComputerGroupCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.InvokerName);
        if (user == null || await _userManager.IsInRoleAsync(user, Roles.Admin))
        {
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.ComputerGroup>().NoPrivilege);
        }
        
        var computerGroup = await _context.ComputerGroups.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
        if (computerGroup == null)
        {
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.ComputerGroup>().ObjectNotFound);
        }

        if (request.Description == null)
            return new ComputerGroupHeader(computerGroup);
        
        computerGroup.Description = request.Description;
            
        await _context.SaveChangesAsync(cancellationToken);
            
        return new ComputerGroupHeader(computerGroup);

    }
}