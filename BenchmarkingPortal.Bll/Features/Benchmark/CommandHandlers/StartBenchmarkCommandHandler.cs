using System.Xml;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Configuration.Queries;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Web;
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
    private readonly IMediator _mediator;

    public StartBenchmarkCommandHandler(BenchmarkingDbContext context, StoragePaths storagePaths, IMediator mediator)
    {
        _context = context;
        _vcloudBenchmarkPath = storagePaths.VcloudBenchmarkPath;
        _workDir = storagePaths.WorkingDir;
        _mediator = mediator;
    }

    public async Task<BenchmarkHeader> Handle(StartBenchmarkCommand request, CancellationToken cancellationToken)
    {
        // ---------------------------------------------------------
        // Value validations first:

        // Checking whether the name of the new benchmark is unique
        var nameCount =  _context.Benchmarks.Where(b => b.Name.Equals(request.Name)).Select(b => b.Id)
            .Count();
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
        
        CreateXmlSetup(newBenchmark, cancellationToken);
        var startedDate = DateTime.UtcNow;
        
        var toolName =  _mediator.Send(new GetExecutableToolNameByIdQuery
        {
            Id = newBenchmark.ExecutableId
        }).Result;
        if (toolName == null)
            throw new ApplicationException("The according executable not found.");
        
        var resultDir = _workDir + Path.DirectorySeparatorChar + newBenchmark.UserName
                        + Path.DirectorySeparatorChar + "results"
                        + Path.DirectorySeparatorChar
                        + toolName + "_" + startedDate.ToString("%Y-%m-%d_%H:%M:%S");
        // prepare results directory
        Directory.CreateDirectory(resultDir);
        
        newBenchmark.ResultPath = resultDir;
        newBenchmark.StartedDate = startedDate;
        
        await StartBenchmark(newBenchmark, toolName, cancellationToken);
        
        // Creating new Benchmark entity and writing it to the DB
        // At this point, the benchmark has been successfully configured and started
        var benchmark = new Dal.Entities.Benchmark
        {
            Name = newBenchmark.Name,
            Priority = newBenchmark.Priority,
            Status = Status.Running,
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
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private void CreateXmlSetup(BenchmarkHeader newBenchmark, CancellationToken cancellationToken)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            Async = true
        };

        using var writer = XmlWriter.Create(newBenchmark.XmlFilePath!, settings);
         writer.WriteStartDocumentAsync();
         writer.WriteDocTypeAsync("benchmark", "+//IDN sosy-lab.org//DTD BenchExec benchmark 1.9//EN", 
            "https://www.sosy-lab.org/benchexec/benchmark-2.3.dtd", null);
            
         writer.WriteStartElementAsync(null, "benchmark", null);
         writer.WriteAttributeStringAsync(null, "tool", null, 
             _mediator.Send(new GetExecutableToolNameByIdQuery
            {
                Id = newBenchmark.ExecutableId
            }, cancellationToken).Result ?? throw new ApplicationException("According executable not found."));
         writer.WriteAttributeStringAsync(null, "timelimit", null, 
            newBenchmark.TimeLimit+"s");
         writer.WriteAttributeStringAsync(null, "hardtimelimit", null,
            newBenchmark.HardTimeLimit+"s");
         writer.WriteAttributeStringAsync(null, "memlimit", null,
            newBenchmark.Ram+"GB");
         writer.WriteAttributeStringAsync(null, "cpuCores", null,
            newBenchmark.Cpu.ToString());
        
            
        var config =  _mediator.Send(new GetConfigurationByIdQuery
        {
            Id = newBenchmark.ConfigurationId
        }, cancellationToken).Result ?? throw new ApplicationException("According configuration not found.");

        if (config.ConfigurationItems != null)
        {
            foreach (var item in config.ConfigurationItems.Where(ci => ci.Scope == Scope.Global).ToList())
            {
                 writer.WriteStartElementAsync(null, "option", null);
                 writer.WriteAttributeStringAsync(null, "name", null, "--" + item.Key);
                 writer.WriteStringAsync(item.Value ?? "");
                 writer.WriteEndElementAsync();
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
            foreach (var list in optionList.Where(list => list.Count == 1).Where(list => list[0].Value == ""))
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
                     writer.WriteStartElementAsync(null, "rundefinition", null);
                     writer.WriteAttributeStringAsync(null, "name", null,
                        configurationItemHeaders.Aggregate("", (current, item) =>
                        {
                            if (item.Key != null)
                                return current + item.Key.Split("-")[0] + "-" + item.Value + "_";
                            return current;
                        }));
                        
                    foreach (var item in configurationItemHeaders.Where(item => item.Key != null))
                    {
                         writer.WriteStartElementAsync(null, "option", null);
                         writer.WriteAttributeStringAsync(null, "name", null, "--" + item.Key);
                         writer.WriteStringAsync(item.Value ?? "");
                         writer.WriteEndElementAsync();
                    }
                        
                    writer.WriteStartElementAsync(null, "tasks", null);
                    writer.WriteAttributeStringAsync(null, "name", null, 
                        newBenchmark.SetFilePath!.Split(Path.DirectorySeparatorChar).Last().Split(".")[0]);
                    writer.WriteStartElementAsync(null, "includesfile", null);
                    writer.WriteStringAsync("sv-benchmarks" + Path.DirectorySeparatorChar + "c" 
                                            + Path.DirectorySeparatorChar 
                                            + newBenchmark.SetFilePath!.Split(Path.DirectorySeparatorChar).Last());
                    writer.WriteEndElementAsync();
                    writer.WriteStartElementAsync(null, "propertyfile", null);
                    writer.WriteStringAsync("sv-benchmarks" + Path.DirectorySeparatorChar + "c" 
                                            + Path.DirectorySeparatorChar + "properties" 
                                            + Path.DirectorySeparatorChar 
                                            + newBenchmark.PropertyFilePath!.Split(Path.DirectorySeparatorChar).Last());
                    writer.WriteEndElementAsync();
                    writer.WriteEndElementAsync();
                    writer.WriteEndElementAsync();
                }
            }
        }

        writer.WriteEndElementAsync();
        writer.FlushAsync();
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
    /// <param name="toolName"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private async Task StartBenchmark(BenchmarkHeader newBenchmark, string toolName, CancellationToken cancellationToken)
    {
        var xmlRelativePath = newBenchmark.XmlFilePath!.TrimStart((_workDir + Path.DirectorySeparatorChar).ToCharArray());
        var toolDir = newBenchmark.UserName + Path.DirectorySeparatorChar + "tools" + Path.DirectorySeparatorChar
                      + toolName;

        var outputLogPath = newBenchmark.ResultPath! + Path.DirectorySeparatorChar + "output.log";
        var errorLogPath = newBenchmark.ResultPath! + Path.DirectorySeparatorChar + "error.log";
        
        using var forcefulCts = new CancellationTokenSource();
        
        var result = await Cli.Wrap(_vcloudBenchmarkPath)
            .WithArguments(args =>
            {
                args
                    .Add("--tryLessMemory")
                    .Add("--no-container")
                    .Add(xmlRelativePath)
                    .Add("--tool-directory").Add(toolDir)
                    .Add("--vcloudAdditionalFiles").Add(toolDir)
                    .Add("-o").Add(newBenchmark.ResultPath!.TrimStart((_workDir + Path.DirectorySeparatorChar).ToCharArray()))
                    .Add("--vcloudPriority").Add(newBenchmark.Priority.ToString());
                if(newBenchmark.CpuModel != null && !newBenchmark.CpuModel.Equals("") && !newBenchmark.CpuModel.Equals("-"))
                    args.Add("--vcloudCPUModel").Add(newBenchmark.CpuModel);
            })
            .WithWorkingDirectory(_workDir)
            .WithStandardOutputPipe(PipeTarget.ToFile(outputLogPath))
            .WithStandardErrorPipe(PipeTarget.ToFile(errorLogPath))
            .ExecuteAsync(forcefulCts.Token, cancellationToken);
        
        if (result.ExitCode != 0)
            throw new ApplicationException("The benchmark did not finish successfully.\n" +
                                           "You can check the errors in the error.log file among the result files.");
        if (result.ExitCode == 0)
        {
            var benchmark = await _context.Benchmarks
                .Where(b => b.Name.Equals(newBenchmark.Name))
                .FirstOrDefaultAsync(cancellationToken);
            if (benchmark != null)
                benchmark.Status = Status.Finished;
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}