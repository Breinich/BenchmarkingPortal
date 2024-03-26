using System.Xml;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Configuration.CommandHandlers;
using BenchmarkingPortal.Bll.Features.Configuration.Queries;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CliWrap;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

/// <summary>
/// The handler for the <see cref="StartBenchmarkCommand"/>.
/// </summary>
// ReSharper disable once UnusedType.Global
public class StartBenchmarkCommandHandler : IRequestHandler<StartBenchmarkCommand, BenchmarkHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly string _vcloudBenchmarkPath;
    private readonly string _workDir;
    private readonly string _vcloudHost;
    private readonly IMediator _mediator;
    private readonly IBenchmarkQueue _queue;

    public StartBenchmarkCommandHandler(BenchmarkingDbContext context, PathConfigs pathConfigs, IMediator mediator, IBenchmarkQueue queue)
    {
        _context = context;
        _vcloudBenchmarkPath = pathConfigs.VcloudBenchmarkPath;
        _workDir = pathConfigs.WorkingDir;
        _vcloudHost = pathConfigs.VcloudHost;
        _mediator = mediator;
        _queue = queue;
    }

    /// <summary>
    /// Creates a configuration XML file for the benchmark and starts the benchmark
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>A copy of the information of the benchmark</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ApplicationException"></exception>
    public async Task<BenchmarkHeader> Handle(StartBenchmarkCommand request, CancellationToken cancellationToken)
    {
        // Value validations first:

        // Checking whether the name of the new benchmark is unique
        var nameCount =  await _context.Benchmarks.Where(b => b.Name.Equals(request.Name)).Select(b => b.Id)
            .CountAsync(cancellationToken);
        if (nameCount > 0)
            throw new ArgumentOutOfRangeException(nameof(request), request.Name,
                "A benchmark with the same name already exists.");
        
        if (request.Name != request.InvokerName)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.NoPrivilege);

        var newBenchmark = new BenchmarkHeader
        {
            Name = request.Name,
            Priority = request.Priority,
            Status = Status.Running,
            Ram = request.Ram,
            Cpu = request.Cpu,
            TimeLimit = request.TimeLimit,
            HardTimeLimit = request.HardTimeLimit,
            CpuModelId = request.CpuModelId,
            CpuModelValue = request.CpuModelValue,
            ExecutableId = request.ExecutableId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            ConfigurationId = request.ConfigurationId,
            ComputerGroupId = request.ComputerGroupId,
            UserName = request.InvokerName
        };
        // The other values will be checked by the scheduler
        
        
        var exe = await _mediator.Send(new GetExecutableByIdQuery
        {
            Id = newBenchmark.ExecutableId
        }, cancellationToken) ?? throw new ApplicationException("The according executable not found.");
        
        var config = await _mediator.Send(new GetConfigurationByIdQuery
        {
            Id = newBenchmark.ConfigurationId,
            IncludeItems = false
        }, cancellationToken) ?? throw new ApplicationException("The according configuration not found.");
        
        
        var startedDate = DateTime.UtcNow;
        
        var resultDir = Path.Join(_workDir, newBenchmark.UserName, "results", 
            exe.Name + "_" + startedDate.ToString("yyyy-MM-dd_HH-mm-ss"));
        // prepare results directory
        Directory.CreateDirectory(resultDir);
        
        newBenchmark.ResultPath = resultDir;
        newBenchmark.StartedDate = startedDate;
        
        await QueueBenchmark(newBenchmark, exe, config, cancellationToken);
        
        // Creating new Benchmark entity and writing it to the DB
        // At this point, the benchmark has been successfully configured and queued for running
        var benchmark = new Dal.Entities.Benchmark
        {
            Name = newBenchmark.Name,
            Priority = newBenchmark.Priority,
            Status = Status.Queued,
            Ram = newBenchmark.Ram,
            Cpu = newBenchmark.Cpu,
            TimeLimit = newBenchmark.TimeLimit,
            HardTimeLimit = newBenchmark.HardTimeLimit,
            CpuModelId = newBenchmark.CpuModelId,
            ComputerGroupId = newBenchmark.ComputerGroupId,
            ExecutableId = newBenchmark.ExecutableId,
            SetFilePath = newBenchmark.SetFilePath,
            PropertyFilePath = newBenchmark.PropertyFilePath,
            StartedDate = newBenchmark.StartedDate,
            ConfigurationId = request.ConfigurationId,
            UserName = newBenchmark.UserName,
            ResultPath = newBenchmark.ResultPath,
        };

        _context.Benchmarks.Add(benchmark);
        await _context.SaveChangesAsync(cancellationToken);

        // Returning the benchmark with the generated Id from the DB
        return new BenchmarkHeader(benchmark);
    }

    /// <summary>
    /// Starts the benchmark
    /// </summary>
    /// <param name="newBenchmark">The benchmark's info</param>
    /// <param name="exe">The executable, that will be used</param>
    /// <param name="config">The configuration, that will be used</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private async Task QueueBenchmark(BenchmarkHeader newBenchmark, ExecutableHeader exe, ConfigurationHeader config,
        CancellationToken cancellationToken)
    {
        var xmlRelativePath = config.XmlFilePath![_workDir.Length..].TrimStart(Path.DirectorySeparatorChar);
        var toolDir = Path.Join(exe.UserName, "tools", exe.Name);

        var outputLogPath = Path.Join(newBenchmark.ResultPath!, "output.log");
        var errorLogPath = Path.Join(newBenchmark.ResultPath!, "error.log");
        
        using var forcefulCts = new CancellationTokenSource();
        
        var cmd = Cli.Wrap(_vcloudBenchmarkPath)
            .WithArguments(args =>
            {
                args
                    .Add("--tryLessMemory")
                    .Add("--no-container")
                    .Add(xmlRelativePath)
                    .Add("--tool-directory").Add(toolDir)
                    .Add("--vcloudAdditionalFiles").Add(toolDir)
                    .Add("-o").Add(newBenchmark.ResultPath![_workDir.Length..].TrimStart(Path.DirectorySeparatorChar))
                    .Add("--vcloudPriority").Add(newBenchmark.Priority.ToString());
                if(!string.IsNullOrEmpty(_vcloudHost))
                    args.Add("--vcloudMaster").Add(_vcloudHost);
                if(!string.IsNullOrEmpty(newBenchmark.CpuModelValue) && !newBenchmark.CpuModelValue.Equals("-"))
                    args.Add("--vcloudCPUModel").Add(newBenchmark.CpuModelValue);
            })
            .WithWorkingDirectory(_workDir)
            .WithStandardOutputPipe(PipeTarget.ToFile(outputLogPath))
            .WithStandardErrorPipe(PipeTarget.ToFile(errorLogPath))
            .WithValidation(CommandResultValidation.None);

        await _queue.QueueBenchmarkAsync(new BenchmarkTask(newBenchmark, cmd), cancellationToken);
    }
}