﻿using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

public class UpdateBenchmarkCommandHandler : IRequestHandler<UpdateBenchmarkCommand, BenchmarkHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<User> _userManager;

    public UpdateBenchmarkCommandHandler(BenchmarkingDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<BenchmarkHeader> Handle(UpdateBenchmarkCommand request, CancellationToken cancellationToken)
    {
        var benchmarkEntity = await _context.Benchmarks.FindAsync(request.Id, cancellationToken) ??
                              throw new ArgumentException(new ExceptionMessage<Dal.Entities.Benchmark>()
                                  .ObjectNotFound);


        var benchmarkHeader = new BenchmarkHeader(benchmarkEntity);

        // Only the owner or the administrators have the permission to modify a specific benchmark
        if (benchmarkHeader.UserId != request.UserId)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString()) ??
                       throw new ArgumentException(new ExceptionMessage<User>().ObjectNotFound);


            var admin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!admin)
            {
                throw new ArgumentException(
                    new ExceptionMessage<Dal.Entities.Benchmark>().NoPrivilege);
            }
        }

        
        if (benchmarkHeader.Status != Status.Finished)
        {
            benchmarkHeader.Priority = request.Priority;
            benchmarkHeader.Status = request.Status;

            // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            // Ask the scheduler to modify the selected benchmark
            // TODO

            // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx


            // If succeeded, the modified Benchmark will be written into the DB
           
           
            benchmarkEntity.Priority = request.Priority;
            benchmarkEntity.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);
            
        }
        else
        {
            throw new ArgumentException("The benchmark, that wanted to be modified, has already been finished.");
        }


        return benchmarkHeader;
    }
}