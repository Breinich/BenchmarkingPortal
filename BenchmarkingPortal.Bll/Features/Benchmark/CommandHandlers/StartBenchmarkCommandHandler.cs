using System.Xml;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
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
    /// Creates a configuration XML file for the benchmark and starts it
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

        // Maximum available RAM amount is not specified
        if (request.Ram <= 0)
            throw new ArgumentOutOfRangeException(nameof(request), request.Ram,
                "The benchmark must use minimum 1 GB amount of RAM.");

        // Maximum available CPU core count is not specified
        if (request.Cpu <= 0)
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
            ComputerGroupId = request.ComputerGroupId,
            UserName = request.InvokerName
        };
        // The other values will be checked by the scheduler
        
        
        Directory.CreateDirectory(_workDir + Path.DirectorySeparatorChar + newBenchmark.UserName
                                  + Path.DirectorySeparatorChar + "benchmarks");
        newBenchmark.XmlFilePath = _workDir + Path.DirectorySeparatorChar + newBenchmark.UserName
                       + Path.DirectorySeparatorChar + "benchmarks"
                       + Path.DirectorySeparatorChar + newBenchmark.Name
                       + ".xml";
        
        var exe = await _mediator.Send(new GetExecutableByIdQuery
        {
            Id = newBenchmark.ExecutableId
        }, cancellationToken) ?? throw new ApplicationException("The according executable not found.");
        
        await CreateXmlSetup(newBenchmark, exe.OwnerTool, cancellationToken);
        var startedDate = DateTime.UtcNow;
        
        
        
        var resultDir = _workDir + Path.DirectorySeparatorChar + newBenchmark.UserName
                        + Path.DirectorySeparatorChar + "results"
                        + Path.DirectorySeparatorChar
                        + exe.Name + "_" + startedDate.ToString("yyyy-MM-dd_HH-mm-ss");
        // prepare results directory
        Directory.CreateDirectory(resultDir);
        
        newBenchmark.ResultPath = resultDir;
        newBenchmark.StartedDate = startedDate;
        
        await QueueBenchmark(newBenchmark, exe, cancellationToken);
        
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
            CpuModel = newBenchmark.CpuModel,
            ComputerGroupId = newBenchmark.ComputerGroupId,
            ExecutableId = newBenchmark.ExecutableId,
            SetFilePath = newBenchmark.SetFilePath,
            PropertyFilePath = newBenchmark.PropertyFilePath,
            StartedDate = newBenchmark.StartedDate,
            ConfigurationId = request.ConfigurationId,
            UserName = newBenchmark.UserName,
            ResultPath = newBenchmark.ResultPath,
            XmlFilePath = newBenchmark.XmlFilePath
        };

        _context.Benchmarks.Add(benchmark);
        await _context.SaveChangesAsync(cancellationToken);

        // Returning the benchmark with the generated Id from the DB
        return new BenchmarkHeader(benchmark);
    }

    /// <summary>
    /// Creates the XML file for the benchmark
    /// </summary>
    /// <param name="newBenchmark">Benchmark info</param>
    /// <param name="exeToolName">The name of the executable's owner tool</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private async Task CreateXmlSetup(BenchmarkHeader newBenchmark, string? exeToolName, CancellationToken cancellationToken)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            Async = true
        };

        await using var writer = XmlWriter.Create(newBenchmark.XmlFilePath!, settings);
        await writer.WriteStartDocumentAsync();
        await writer.WriteDocTypeAsync("benchmark", "+//IDN sosy-lab.org//DTD BenchExec benchmark 1.9//EN", 
            "https://www.sosy-lab.org/benchexec/benchmark-2.3.dtd", null);
            
        await writer.WriteStartElementAsync(null, "benchmark", null);
        await writer.WriteAttributeStringAsync(null, "tool", null, exeToolName ?? throw new ApplicationException("According executable not found."));
        await writer.WriteAttributeStringAsync(null, "timelimit", null, 
            newBenchmark.TimeLimit+"s");
        await writer.WriteAttributeStringAsync(null, "hardtimelimit", null,
            newBenchmark.HardTimeLimit+"s");
        await writer.WriteAttributeStringAsync(null, "memlimit", null,
            newBenchmark.Ram+"GB");
        await writer.WriteAttributeStringAsync(null, "cpuCores", null,
            newBenchmark.Cpu.ToString());
        
            
        var config =  await _mediator.Send(new GetConfigurationByIdQuery
        {
            Id = newBenchmark.ConfigurationId
        }, cancellationToken) ?? throw new ApplicationException("According configuration not found.");

        if (config.ConfigurationItems != null)
        {
            foreach (var item in config.ConfigurationItems.Where(ci => ci.Scope == Scope.Global).ToList())
            {
                await writer.WriteStartElementAsync(null, "option", null);
                await writer.WriteAttributeStringAsync(null, "name", null, "--" + item.Key);
                await writer.WriteStringAsync(item.Value ?? "");
                await writer.WriteEndElementAsync();
            }

            var optionList = new List<List<ConfigurationItemHeader>>();
            foreach (var item in config.ConfigurationItems.Where(ci => ci.Scope == Scope.Local).ToList())
            {
                if (optionList.Count == 0)
                    optionList.Add(new List<ConfigurationItemHeader>
                    {
                        item
                    });
                else
                {
                    var found = false;
                    foreach (var list in optionList.Where(list => list[0].Key!.Equals(item.Key)))
                    {
                        list.Add(item);
                        found = true;
                        break;
                    }

                    if (!found)
                        optionList.Add(new List<ConfigurationItemHeader>{ item });
                }
            }
                
            // Add an empty ConfigItem to the end of the list, if there is no value for the only one option,
            // to let the combinations contain this option and let them contain also the empty option
            foreach (var list in optionList.Where(list => list.Count == 1).Where(list => string.IsNullOrEmpty(list[0].Value)))
            {
                list.Add(new ConfigurationItemHeader
                {
                    Key = null,
                    Value = null
                });
            }

            if (optionList.Count > 0)
            {
                var crossProduct = CrossProduct(optionList);
                foreach (var list in crossProduct)
                {
                        
                    var configurationItemHeaders = list.ToList();
                    await writer.WriteStartElementAsync(null, "rundefinition", null);
                    await writer.WriteAttributeStringAsync(null, "name", null,
                        configurationItemHeaders.Aggregate("", (current, item) =>
                        {
                            if (item.Key != null)
                                return current + item.Key.Split("-")[0] + "-" + item.Value + "_";
                            return current;
                        }));
                        
                    foreach (var item in configurationItemHeaders.Where(item => item.Key != null))
                    {
                        await writer.WriteStartElementAsync(null, "option", null);
                        await writer.WriteAttributeStringAsync(null, "name", null, "--" + item.Key);
                        await writer.WriteStringAsync(item.Value ?? "");
                        await writer.WriteEndElementAsync();
                    }
                        
                    await writer.WriteStartElementAsync(null, "tasks", null);
                    await writer.WriteAttributeStringAsync(null, "name", null, 
                        newBenchmark.SetFilePath!.Split(Path.DirectorySeparatorChar).Last()
                            .TrimStart('.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', '-', '+').Split(".")[0]);
                    await writer.WriteStartElementAsync(null, "includesfile", null);
                    await writer.WriteStringAsync(".." + Path.DirectorySeparatorChar + ".." 
                                                  + Path.DirectorySeparatorChar + "sv-benchmarks" 
                                                  + Path.DirectorySeparatorChar + "c" 
                                                  + Path.DirectorySeparatorChar 
                                                  + newBenchmark.SetFilePath!.Split(Path.DirectorySeparatorChar).Last());
                    await writer.WriteEndElementAsync();
                    await writer.WriteStartElementAsync(null, "propertyfile", null);
                    await writer.WriteStringAsync(".." + Path.DirectorySeparatorChar + ".." 
                                                  + Path.DirectorySeparatorChar + "sv-benchmarks" 
                                                  + Path.DirectorySeparatorChar + "c" 
                                                  + Path.DirectorySeparatorChar + "properties" 
                                                  + Path.DirectorySeparatorChar 
                                                  + newBenchmark.PropertyFilePath!.Split(Path.DirectorySeparatorChar).Last());
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                }
            }
        }

        await writer.WriteEndElementAsync();
        await writer.FlushAsync();

        var xmlPath = _workDir + Path.DirectorySeparatorChar
                               + newBenchmark.Name + ".xml";
        File.Copy(newBenchmark.XmlFilePath!, xmlPath, true);
        return xmlPath;
    }
    
    /// <summary>
    /// Creates the cross product of the given lists
    /// </summary>
    /// <param name="source">Source list of lists</param>
    /// <typeparam name="T">Type of one element in the inner lists</typeparam>
    /// <returns>The cross product in the form of list of lists</returns>
    private static IEnumerable<IEnumerable<T>> CrossProduct<T>(IEnumerable<IEnumerable<T>> source) => 
        source.Aggregate(
            (IEnumerable<IEnumerable<T>>) new[] { Enumerable.Empty<T>() },
            (acc, src) => src.SelectMany(x => acc.Select(a => a.Concat(new[] {x}))));

    /// <summary>
    /// Starts the benchmark
    /// </summary>
    /// <param name="newBenchmark">The benchmark's info</param>
    /// <param name="exe">The executable, that will be used</param>
    /// <param name="xmlPath">The absolute path for the xml file, must be in the working directory</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private async Task QueueBenchmark(BenchmarkHeader newBenchmark, ExecutableHeader exe,
        CancellationToken cancellationToken)
    {
        var xmlRelativePath = newBenchmark.XmlFilePath![_workDir.Length..].TrimStart(Path.DirectorySeparatorChar);
        var toolDir = exe.UserName + Path.DirectorySeparatorChar + "tools" + Path.DirectorySeparatorChar
                      + exe.Name;

        var outputLogPath = newBenchmark.ResultPath! + Path.DirectorySeparatorChar + "output.log";
        var errorLogPath = newBenchmark.ResultPath! + Path.DirectorySeparatorChar + "error.log";
        
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
                if(!string.IsNullOrEmpty(newBenchmark.CpuModel) && !newBenchmark.CpuModel.Equals("-"))
                    args.Add("--vcloudCPUModel").Add(newBenchmark.CpuModel);
            })
            .WithWorkingDirectory(_workDir)
            .WithStandardOutputPipe(PipeTarget.ToFile(outputLogPath))
            .WithStandardErrorPipe(PipeTarget.ToFile(errorLogPath))
            .WithValidation(CommandResultValidation.None);

        await _queue.QueueBenchmarkAsync(new BenchmarkTask(newBenchmark, cmd), cancellationToken);
    }
}