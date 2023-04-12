using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

public class DeleteBenchmarkCommandHandler : IRequestHandler<DeleteBenchmarkCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<User> _userManager;

    public DeleteBenchmarkCommandHandler(BenchmarkingDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Handle(DeleteBenchmarkCommand request, CancellationToken cancellationToken)
    {
        var benchmark = await _context.Benchmarks.Where(b => b.Id == request.BenchmarkId).Select(b => b)
            .FirstAsync(cancellationToken);

        var benchmarkHeader = new BenchmarkHeader(benchmark);

        if (benchmark == null)
        {
            throw new ArgumentException("The benchmark, that wanted to be deleted, doesn't exist.");
        }

        // Only the owner or the administrators have the permission to delete a specific benchmark
        if (benchmarkHeader.UserId != request.UserId)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new ArgumentException("The provided userId is not in the database.");
            }

            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
            {
                throw new ArgumentException(
                    "The user is neither the owner of the benchmark, nor an admin.");
            }
        }

        // Only finished benchmarks are allowed to delete
        if (benchmarkHeader.Status != Status.Finished)
        {
            throw new ArgumentException("The benchmark, that wanted to be deleted hasn't been finished yet.");
        }

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // Somehow remove the results of the benchmark, maybe ask a service
        // If this uses the whole object, BenchmarkHeader should be used

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        _context.Benchmarks.Remove(benchmark);
        await _context.SaveChangesAsync(cancellationToken);
    }
}