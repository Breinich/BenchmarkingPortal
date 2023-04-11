using System.Net;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

public class StartBenchmarkCommandHandler : IRequestHandler<StartBenchmarkCommand, BenchmarkHeader>
{
    private readonly BenchmarkingDbContext _context;

    public StartBenchmarkCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<BenchmarkHeader> Handle(StartBenchmarkCommand request, CancellationToken cancellationToken)
    {
        // ---------------------------------------------------------
        // Value validations first:
        
        // Checking whether the name of the new benchmark is unique
        int nameCountAsync = await _context.Benchmarks.Where(b => b.Name.Equals(request.Name)).Select(b => b.Id)
            .CountAsync(cancellationToken);

        if (nameCountAsync > 0)
        {
            throw new ArgumentOutOfRangeException("Name",request.Name,"A benchmark with the same name already exists.");
        }

        // Maximum available RAM amount is 16 GB
        if (request.Ram is <= 0 or > 16)
        {
            throw new ArgumentOutOfRangeException("Ram",request.Ram,"The benchmark must use minimum 1 GB and maximum 16 GB amount of RAM.");
        }

        // Maximum available CPU core count is 8
        if (request.Cpu is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException("Cpu", request.Cpu,
                "The benchmark must run on at least 1 CPU core and at most on 8 CPU cores.");
        }

        // Default value for TimeLimit is 900 seconds
        if (request.TimeLimit is 0)
        {
            request.TimeLimit = 900;
        }

        // Default value for HardTimeLimit is 960 seconds
        if (request.HardTimeLimit is 0)
        {
            request.HardTimeLimit = 960;
        }

        var newBenchmark = new BenchmarkHeader()
        {
            Name = request.Name,
            Priority = request.Priority,
            Status = Status.Running,
            Ram = request.Ram,
            Cpu = request.Cpu,
            TimeLimit = request.TimeLimit,
            HardTimeLimit = request.HardTimeLimit,
            ExecutableId = request.ExecutableId,
            SourceSetId = request.SourceSetId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            ConfigurationId = request.ConfigurationId,
            UserId = request.UserId,
        };

        // The other values will be checked by the scheduler
        // ---------------------------------------------------------

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // Delegating the benchmark to the scheduler and starting it on one or more VMs

        // The scheduler sends back the assigned ComputerGroup's Id and the start time

        // If the starting of the benchmark would be compromised, de scheduler must throw an exception and
        // in this case, no benchmark will be started

        // TODO

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // Creating new Benchmark entity and writing it to the DB
        // At this point, the benchmark has been successfully configured and started
        var benchmark = new Dal.Entities.Benchmark()
        {
            Name = request.Name,
            Priority = request.Priority,
            Status = Status.Running,
            Ram = request.Ram,
            Cpu = request.Cpu,
            TimeLimit = request.TimeLimit,
            HardTimeLimit = request.HardTimeLimit,
            // ComputerGroupId = something,
            ExecutableId = request.ExecutableId,
            SourceSetId = request.SourceSetId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            // StartedDate = something
            ConfigurationId = request.ConfigurationId,
            UserId = request.UserId,
        };

        _context.Benchmarks.Add(benchmark);
        await _context.SaveChangesAsync(cancellationToken);

        // Returning the benchmark with the generated Id from the DB
        return await _context.Benchmarks.Where(b => b.Name.Equals(request.Name)).Select(b => new BenchmarkHeader()
        {
            Id = b.Id,
            Name = b.Name,
            Priority = b.Priority,
            Status = b.Status,
            Result = b.Result,
            Ram = b.Ram,
            Cpu = b.Cpu,
            TimeLimit = b.TimeLimit,
            HardTimeLimit = b.HardTimeLimit,
            ComputerGroupId = b.ComputerGroupId,
            ExecutableId = b.ExecutableId,
            SourceSetId = b.SourceSetId,
            StartedDate = b.StartedDate,
            ConfigurationId = b.ConfigurationId,
            UserId = b.UserId,
        }).FirstAsync(cancellationToken);
    }
}