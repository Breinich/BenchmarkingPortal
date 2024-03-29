﻿using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

/// <summary>
/// Handler for the <see cref="UpdateBenchmarkCommand"/>.
/// </summary>
// ReSharper disable once UnusedType.Global
public class UpdateBenchmarkCommandHandler : IRequestHandler<UpdateBenchmarkCommand, BenchmarkHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public UpdateBenchmarkCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public async Task<BenchmarkHeader> Handle(UpdateBenchmarkCommand request, CancellationToken cancellationToken)
    {
        var benchmarkEntity = await _context.Benchmarks.FindAsync(new object?[] { request.Id}, 
                                  cancellationToken: cancellationToken) ??
                              throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.ObjectNotFound);
        
        var benchmarkHeader = new BenchmarkHeader(benchmarkEntity);

        // Only the owner or the administrators have the permission to modify a specific benchmark
        if (benchmarkHeader.UserName != request.InvokerName)
        {
            var user = await _userManager.FindByNameAsync(request.InvokerName) ??
                       throw new ArgumentException(ExceptionMessage<Dal.Entities.User>.ObjectNotFound);
            
            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
                throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.NoPrivilege);
        }

        if (benchmarkHeader.Status == Status.Finished)
            throw new ArgumentException("The benchmark, that wanted to be modified, has already been finished.");
        
        if (benchmarkHeader.Priority != request.Priority)
        {
            benchmarkHeader.Priority = request.Priority;

            // tell vcloud to change the priority of the benchmark
            throw new NotImplementedException("This feature is not fully implemented yet.");
        }
        
        if (request.Status == Status.Finished)
        {
            benchmarkHeader.Status = Status.Finished;
            
            // tell vcloud to finish the benchmark
            throw new NotImplementedException("This feature is not fully implemented yet.");
            
            // Remove the benchmark from the computer group
            benchmarkEntity.ComputerGroup = null;
            // Remove the benchmark from the CPU model
            // TODO: should we keep this information?
            benchmarkEntity.CpuModel = null;
        }

        // If succeeded, the modified Benchmark will be written into the DB
        benchmarkEntity.Priority = benchmarkHeader.Priority;
        benchmarkEntity.Status = benchmarkHeader.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return benchmarkHeader;
    }
}