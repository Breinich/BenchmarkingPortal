using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

public class UploadNewExecutableCommandHandler : IRequestHandler<UploadNewExecutableCommand, ExecutableHeader>
{
    private readonly BenchmarkingDbContext _context;

    public UploadNewExecutableCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<ExecutableHeader> Handle(UploadNewExecutableCommand request, CancellationToken cancellationToken)
    {
        request.Version ??= "1.0";

        var exe = new Dal.Entities.Executable()
        {
            Name = request.Name,
            OwnerTool = request.OwnerTool,
            ToolVersion = request.ToolVersion,
            Path = request.Path,
            Version = request.Version,
            UploadedDate = request.UploadedDate,
            UserName = request.InvokerName
        };

        await _context.Executables.AddAsync(exe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new ExecutableHeader(exe);
    }
}