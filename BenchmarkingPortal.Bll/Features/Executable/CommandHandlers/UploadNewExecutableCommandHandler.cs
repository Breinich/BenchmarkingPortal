using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

/// <summary>
/// Handler for <see cref="UploadNewExecutableCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class UploadNewExecutableCommandHandler(BenchmarkingDbContext context, PathConfigs pathConfigs)
    : IRequestHandler<UploadNewExecutableCommand, ExecutableHeader>
{
    public async Task<ExecutableHeader> Handle(UploadNewExecutableCommand request, CancellationToken cancellationToken)
    {
        request.Version ??= "1.0";
        
        if (!Directory.Exists(Path.Join(pathConfigs.WorkingDir, request.InvokerName, pathConfigs.ExecutableDir, request.Name)))
        {
            // deleting the already uploaded zip and metadata
            File.Delete(Path.Join(pathConfigs.WorkingDir, request.InvokerName, pathConfigs.ExecutableDir, request.Path));
            File.Delete(Path.Join(pathConfigs.WorkingDir, request.InvokerName, pathConfigs.ExecutableDir, request.Path + ".metadata"));
            throw new ArgumentException("The root folder inside the zip of the tool directory doesn't have " +
                                        "the name of the zip file, please make sure to use the same zip name as the " +
                                        "tool directory name!\n" +
                                        "Aborting.");
        }

        var exe = new Dal.Entities.Executable
        {
            Name = request.Name,
            OwnerTool = request.OwnerTool,
            ToolVersion = request.ToolVersion,
            Path = request.Path,
            Version = request.Version,
            UploadedDate = request.UploadedDate,
            UserName = request.InvokerName
        };

        await context.Executables.AddAsync(exe, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new ExecutableHeader(exe);
    }
}