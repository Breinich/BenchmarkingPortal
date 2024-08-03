using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.Executable.CommandHandlers;

/// <summary>
/// Handler for <see cref="DeleteExecutableCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class DeleteExecutableCommandHandler(
    BenchmarkingDbContext context,
    UserManager<Dal.Entities.User> userManager,
    PathConfigs pathConfigs)
    : IRequestHandler<DeleteExecutableCommand>
{
    public async Task Handle(DeleteExecutableCommand request, CancellationToken cancellationToken)
    {
        var exe = await context.Executables.FindAsync(new object?[] { request.ExecutableId }, 
                      cancellationToken: cancellationToken) ??
                  throw new ArgumentException(ExceptionMessage<Dal.Entities.Executable>.ObjectNotFound);

        if (exe.UserName != request.InvokerName)
        {
            var user = await userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);

            var admin = await userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin) throw new ArgumentException(ExceptionMessage<Dal.Entities.Executable>.NoPrivilege);
        }
        
        if (exe.Path != request.FileId)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Executable>.ObjectNotFound);

        context.Remove(exe);
        await context.SaveChangesAsync(cancellationToken);
        
        var store = new CustomTusDiskStore(Path.Join(pathConfigs.WorkingDir, exe.UserName, pathConfigs.ExecutableDir));
        await store.DeleteFileAsync(request.FileId, cancellationToken);

        new Task(() => Directory.Delete(Path.Join(pathConfigs.WorkingDir, exe.UserName, pathConfigs.ExecutableDir, exe.Name), true)).Start();
    }
}