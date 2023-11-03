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
        var nameCountAsync = await _context.Benchmarks.Where(b => b.Name.Equals(request.Name)).Select(b => b.Id)
            .CountAsync(cancellationToken);

        if (nameCountAsync > 0)
            throw new ArgumentOutOfRangeException(nameof(request), request.Name,
                "A benchmark with the same name already exists.");

        // Maximum available RAM amount is not specified
        if (request.Ram is <= 0)
            throw new ArgumentOutOfRangeException(nameof(request), request.Ram,
                "The benchmark must use minimum 1 GB amount of RAM.");

        // Maximum available CPU core count is not specified
        if (request.Cpu is <= 0)
            throw new ArgumentOutOfRangeException(nameof(request), request.Cpu,
                "The benchmark must run on at least 1 CPU core.");

        // Default value for TimeLimit is 900 seconds
        if (request.TimeLimit is 0) request.TimeLimit = 900;

        // Default value for HardTimeLimit is 960 seconds
        if (request.HardTimeLimit is 0) request.HardTimeLimit = 960;

        var newBenchmark = new BenchmarkHeader
        {
            Name = request.Name,
            Priority = request.Priority,
            Status = Status.Running,
            Ram = request.Ram,
            Cpu = request.Cpu,
            TimeLimit = request.TimeLimit,
            HardTimeLimit = request.HardTimeLimit,
            CpuModel = request.CpuModel,
            ExecutableId = request.ExecutableId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            ConfigurationId = request.ConfigurationId,
            UserName = request.InvokerName
        };

        // The other values will be checked by the scheduler
        // ---------------------------------------------------------

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // Delegating the benchmark to the scheduler and starting it on one or more VMs

        // The scheduler sends back the assigned ComputerGroup's Id and the start time, these are ONLY TEMPORARY
        var startedDate = DateTime.UtcNow;
        var computerGroupId = 1;
        // If the starting of the benchmark would be compromised, de scheduler must throw an exception and
        // in this case, no benchmark will be started

        // TODO

        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        // Creating new Benchmark entity and writing it to the DB
        // At this point, the benchmark has been successfully configured and started
        var benchmark = new Dal.Entities.Benchmark
        {
            Name = request.Name,
            Priority = request.Priority,
            Status = Status.Running,
            Ram = request.Ram,
            Cpu = request.Cpu,
            TimeLimit = request.TimeLimit,
            HardTimeLimit = request.HardTimeLimit,
            CpuModel = request.CpuModel,
            // TODO need to be fixed
            ComputerGroupId = computerGroupId,
            ExecutableId = request.ExecutableId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            // TODO need to be fixed
            StartedDate = startedDate,
            ConfigurationId = request.ConfigurationId,
            UserName = newBenchmark.UserName
        };

        _context.Benchmarks.Add(benchmark);
        await _context.SaveChangesAsync(cancellationToken);

        // Returning the benchmark with the generated Id from the DB
        return new BenchmarkHeader(benchmark);
    }
}