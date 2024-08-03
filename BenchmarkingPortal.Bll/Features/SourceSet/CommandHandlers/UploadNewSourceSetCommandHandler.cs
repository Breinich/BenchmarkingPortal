using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.CommandHandlers;

public class UploadNewSourceSetCommandHandler(BenchmarkingDbContext context, PathConfigs pathConfigs)
    : IRequestHandler<UploadNewSourceSetCommand, SourceSetHeader>
{
    public async Task<SourceSetHeader> Handle(UploadNewSourceSetCommand request, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(Path.Join(pathConfigs.WorkingDir, request.InvokerName, pathConfigs.SourceSetDir, request.Name)))
        {
            // deleting the already uploaded zip and metadata
            File.Delete(Path.Join(pathConfigs.WorkingDir, request.InvokerName, pathConfigs.SourceSetDir, request.Path));
            File.Delete(Path.Join(pathConfigs.WorkingDir, request.InvokerName, pathConfigs.SourceSetDir, request.Path + ".metadata"));
            throw new ArgumentException("The root folder inside the zip of the tool directory doesn't have " +
                                        "the name of the zip file, please make sure to use the same zip name as the " +
                                        "tool directory name!\n" +
                                        "Aborting.");
        }
        
        var sourceSet = new Dal.Entities.SourceSet
        {
            Name = request.Name,
            UserName = request.InvokerName,
            UploadedDate = request.UploadedDate,
            Path = request.Path
        };
        
        await context.SourceSets.AddAsync(sourceSet, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new SourceSetHeader(sourceSet);
    }
}