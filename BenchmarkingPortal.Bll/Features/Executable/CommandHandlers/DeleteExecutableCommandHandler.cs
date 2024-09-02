using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    
    /// <summary>
    /// Handles the <see cref="DeleteExecutableCommand"/>
    /// </summary>
    /// <param name="request"> The executable to delete </param>
    /// <param name="cancellationToken"> The token to monitor for cancellation requests </param>
    /// <exception cref="ArgumentException"> Thrown when the invoker is not an admin or the executable is in use </exception>
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
        
        var benchmarksCount = await context.Benchmarks
            .CountAsync(b => b.ExecutableId == request.ExecutableId, cancellationToken);
        if (benchmarksCount > 0)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Executable>.InUse);

        context.Remove(exe);
        await context.SaveChangesAsync(cancellationToken);
        
        var store = new CustomTusDiskStore(Path.Join(pathConfigs.WorkingDir, exe.UserName, pathConfigs.ExecutableDir));
        await store.DeleteFileAsync(request.FileId, cancellationToken);

        new Task(() => Directory.Delete(Path.Join(pathConfigs.WorkingDir, exe.UserName, pathConfigs.ExecutableDir, exe.Name), true)).Start();
    }
}