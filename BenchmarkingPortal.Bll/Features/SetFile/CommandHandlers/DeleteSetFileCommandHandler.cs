using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using tusdotnet.Interfaces;

namespace BenchmarkingPortal.Bll.Features.SetFile.CommandHandlers;

public class DeleteSetFileCommandHandler : IRequestHandler<DeleteSetFileCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public DeleteSetFileCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
        IConfiguration configuration, IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _mediator = mediator;
    }


    public async Task Handle(DeleteSetFileCommand request, CancellationToken cancellationToken)
    {
        var setFile = await _context.SetFiles.FindAsync(request.SetFileId, cancellationToken) ??
                        throw new ArgumentException(new ExceptionMessage<Dal.Entities.SetFile>().ObjectNotFound);

        if (setFile.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(new ExceptionMessage<Dal.Entities.User>().ObjectNotFound);

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(new ExceptionMessage<Dal.Entities.SetFile>().NoPrivilege);
        }

        if (setFile.Path != request.FileId)
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.SetFile>().ObjectNotFound);
        
        ITusTerminationStore terminationStore = new CustomTusDiskStore(
            (_configuration["Storage:SV-Benchmarks"] ?? 
             throw new ApplicationException("SV-Benchmarks path not set in configuration.")) 
            + Path.DirectorySeparatorChar + "c", _mediator);

        _context.Remove(setFile);
        await _context.SaveChangesAsync(cancellationToken);
        await terminationStore.DeleteFileAsync(request.FileId, cancellationToken);
    }
}