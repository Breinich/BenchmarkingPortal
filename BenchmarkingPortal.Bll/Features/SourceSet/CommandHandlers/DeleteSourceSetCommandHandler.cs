using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.SourceSet.CommandHandlers;

public class DeleteSourceSetCommandHandler : IRequestHandler<DeleteSourceSetCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager; 

    public DeleteSourceSetCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task Handle(DeleteSourceSetCommand request, CancellationToken cancellationToken)
    {
        var sourceSet = await _context.SourceSets.FindAsync(request.SourceSetId, cancellationToken) ??
                        throw new ArgumentException(new ExceptionMessage<Dal.Entities.SourceSet>().ObjectNotFound);

        if (sourceSet.UserId != request.UserId)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString()) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
            {
                throw new ArgumentException(new ExceptionMessage<Dal.Entities.SourceSet>().NoPrivilege);
            }
        }

        _context.Remove(sourceSet);
        await _context.SaveChangesAsync(cancellationToken);
    }
}