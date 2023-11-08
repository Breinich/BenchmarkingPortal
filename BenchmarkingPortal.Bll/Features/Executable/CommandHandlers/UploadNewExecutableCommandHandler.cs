using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

public class UploadNewExecutableCommandHandler : IRequestHandler<UploadNewExecutableCommand, ExecutableHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly IConfiguration _configuration;

    public UploadNewExecutableCommandHandler(BenchmarkingDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    public async Task<ExecutableHeader> Handle(UploadNewExecutableCommand request, CancellationToken cancellationToken)
    {
        request.Version ??= "1.0";
        
        if (!Directory.Exists(_configuration["Storage:Root"] + Path.DirectorySeparatorChar + request.InvokerName 
                              + Path.DirectorySeparatorChar + request.Name))
        {
            foreach(var file in Directory.EnumerateFiles(_configuration["Storage:Root"] 
                                                         + Path.DirectorySeparatorChar + request.InvokerName))
            {
                // deleting the possibly junk files from the directory
                if(!file.EndsWith(".zip") && !file.EndsWith(".metadata"))
                    File.Delete(file);
            }
            
            // deleting the already uploaded zip and metadata
            File.Delete(_configuration["Storage:Root"] + Path.DirectorySeparatorChar + request.InvokerName 
                        + Path.DirectorySeparatorChar + request.Path);
            File.Delete(_configuration["Storage:Root"] + Path.DirectorySeparatorChar + request.InvokerName 
                        + Path.DirectorySeparatorChar + request.Path + ".metadata");
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