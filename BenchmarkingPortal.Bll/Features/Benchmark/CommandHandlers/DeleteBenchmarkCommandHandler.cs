﻿using BenchmarkingPortal.Bll.Exceptions;
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
public class DeleteBenchmarkCommandHandler : IRequestHandler<DeleteBenchmarkCommand>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public DeleteBenchmarkCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Handle(DeleteBenchmarkCommand request, CancellationToken cancellationToken)
    {
        var benchmark = await _context.Benchmarks.Where(b => b.Id == request.Id).Select(b => b)
                            .FirstAsync(cancellationToken) ??
                        throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.ObjectNotFound);

        var benchmarkHeader = new BenchmarkHeader(benchmark);

        // Only the owner or the administrators have the permission to delete a specific benchmark
        if (benchmarkHeader.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);


            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
                throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.NoPrivilege);
        }

        // Only finished benchmarks are allowed to delete
        if (benchmarkHeader.Status != Status.Finished)
            throw new ArgumentException("The benchmark, that wanted to be deleted hasn't been finished yet.");


        if (benchmarkHeader.ResultPath != null)
        {
            Directory.Delete(benchmarkHeader.ResultPath, true);
            File.Delete(benchmarkHeader.ResultPath + ".zip");
        }

        _context.Benchmarks.Remove(benchmark);
        await _context.SaveChangesAsync(cancellationToken);
    }
}