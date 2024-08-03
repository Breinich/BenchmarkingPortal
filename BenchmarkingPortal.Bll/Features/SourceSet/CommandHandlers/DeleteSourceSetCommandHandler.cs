using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SourceSet.CommandHandlers;

public class DeleteSourceSetCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager, 
    PathConfigs pathConfigs) : IRequestHandler<DeleteSourceSetCommand>
{
    public async Task Handle(DeleteSourceSetCommand request, CancellationToken cancellationToken)
    {
        var sourceSet = await context.SourceSets.FindAsync([request.Id], cancellationToken: cancellationToken) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);

        if (sourceSet.UserName != request.InvokerName)
        {
            var user = await userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);

            var admin = await userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.NoPrivilege);
        }

        var setFilesStore = new CustomTusDiskStore(Path.Join(
            pathConfigs.WorkingDir, sourceSet.UserName, pathConfigs.SourceSetDir, 
            sourceSet.Name, pathConfigs.SetFileDir));
        var setFiles = await context.SetFiles.Where(f => f.SourceSetId == request.Id).ToListAsync(cancellationToken);
        foreach (var setFile in setFiles)
        {
            context.Remove(setFile);
            await context.SaveChangesAsync(cancellationToken);
            
            await setFilesStore.DeleteFileAsync(setFile.Path, cancellationToken);
        }
        
        context.Remove(sourceSet);
        await context.SaveChangesAsync(cancellationToken);
        
        var sourceSetStore = new CustomTusDiskStore(Path.Join(pathConfigs.WorkingDir, sourceSet.UserName, pathConfigs.SourceSetDir));
        await sourceSetStore.DeleteFileAsync(sourceSet.Path, cancellationToken);
        
        new Task(() => Directory.Delete(Path.Join(pathConfigs.WorkingDir, sourceSet.UserName, pathConfigs.SourceSetDir, sourceSet.Name), true)).Start();
    }
}