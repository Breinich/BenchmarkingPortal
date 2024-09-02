using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

/// <summary>
/// The handler for the <see cref="DeleteBenchmarkCommand"/>.
/// </summary>
public class DeleteBenchmarkCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    : IRequestHandler<DeleteBenchmarkCommand>
{
    
    /// <summary>
    /// The handler for the <see cref="DeleteBenchmarkCommand"/>.
    /// </summary>
    /// <param name="request"> The benchmark to delete. </param>
    /// <param name="cancellationToken"> The token to monitor for cancellation requests. </param>
    /// <exception cref="ArgumentException"> Thrown when the benchmark is not found, the user has no privilege to delete the benchmark, or the benchmark is not finished. </exception>
    public async Task Handle(DeleteBenchmarkCommand request, CancellationToken cancellationToken)
    {
        var benchmark = await context.Benchmarks.Where(b => b.Id == request.Id).Select(b => b)
                            .FirstAsync(cancellationToken) ??
                        throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.ObjectNotFound);

        var benchmarkHeader = new BenchmarkHeader(benchmark);

        // Only the owner or the administrators have the permission to delete a specific benchmark
        if (benchmarkHeader.UserName != request.InvokerName)
        {
            var user = await userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);


            var admin = await userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
                throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.NoPrivilege);
        }

        // Only finished benchmarks are allowed to delete
        if (benchmarkHeader.Status != Status.Finished)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.InUse);


        if (benchmarkHeader.ResultPath != null)
        {
            Directory.Delete(benchmarkHeader.ResultPath, true);
            File.Delete(benchmarkHeader.ResultPath + ".zip");
        }

        context.Benchmarks.Remove(benchmark);
        await context.SaveChangesAsync(cancellationToken);
    }
}