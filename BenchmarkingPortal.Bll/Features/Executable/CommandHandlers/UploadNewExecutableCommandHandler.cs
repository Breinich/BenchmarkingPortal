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
public class UploadNewExecutableCommandHandler : IRequestHandler<UploadNewExecutableCommand, ExecutableHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly string _workDir;

    public UploadNewExecutableCommandHandler(BenchmarkingDbContext context, PathConfigs pathConfigs)
    {
        _context = context;
        _workDir = pathConfigs.WorkingDir;
    }


    public async Task<ExecutableHeader> Handle(UploadNewExecutableCommand request, CancellationToken cancellationToken)
    {
        request.Version ??= "1.0";
        
        if (!Directory.Exists(Path.Join(_workDir, request.InvokerName, "tools", request.Name)))
        {
            foreach(var file in Directory.EnumerateFiles(Path.Join(_workDir, "tools", request.InvokerName)))
            {
                // deleting the possibly junk files from the directory
                if(!file.EndsWith(".zip") && !file.EndsWith(".metadata"))
                    File.Delete(file);
            }
            
            // deleting the already uploaded zip and metadata
            File.Delete(Path.Join(_workDir, request.InvokerName, request.Path));
            File.Delete(Path.Join(_workDir, request.InvokerName, request.Path + ".metadata"));
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

        await _context.Executables.AddAsync(exe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new ExecutableHeader(exe);
    }
}