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
        var nameCountAsync = await _context.Benchmarks.Where(b => b.Name.Equals(request.Name)).Select(b => b.Id)
            .CountAsync(cancellationToken);

        if (nameCountAsync > 0)
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
        var xmlName = _workDir + Path.DirectorySeparatorChar + newBenchmark.UserName
                       + Path.DirectorySeparatorChar + "benchmarks"
                       + Path.DirectorySeparatorChar + newBenchmark.Name
                       + ".xml";
        
        await CreateXmlSetup(newBenchmark, xmlName, cancellationToken);
        
        var startedDate = DateTime.UtcNow;
        await StartBenchmark(newBenchmark, startedDate, xmlName, cancellationToken);
        
        // If the starting of the benchmark would be compromised, de scheduler must throw an exception and
        // in this case, no benchmark will be started

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
            ComputerGroupId = request.ComputerGroupId,
            ExecutableId = request.ExecutableId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            StartedDate = startedDate,
            ConfigurationId = request.ConfigurationId,
            UserName = newBenchmark.UserName
        };

        //_context.Benchmarks.Add(benchmark);
        //await _context.SaveChangesAsync(cancellationToken);

        // Returning the benchmark with the generated Id from the DB
        return new BenchmarkHeader(benchmark);
    }

    /// <summary>
    /// Creates the XML file for the benchmark
    /// </summary>
    /// <param name="newBenchmark">Benchmark info</param>
    /// <param name="xmlName">The absolute path for the xml</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private async Task CreateXmlSetup(BenchmarkHeader newBenchmark, string xmlName, CancellationToken cancellationToken)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            Async = true
        };

        await using var writer = XmlWriter.Create(xmlName, settings);
        await writer.WriteStartDocumentAsync();
        await writer.WriteDocTypeAsync("benchmark", "+//IDN sosy-lab.org//DTD BenchExec benchmark 1.9//EN", 
            "https://www.sosy-lab.org/benchexec/benchmark-2.3.dtd", null);
            
        await writer.WriteStartElementAsync(null, "benchmark", null);
        await writer.WriteAttributeStringAsync(null, "tool", null, 
            await _mediator.Send(new GetExecutableToolNameQuery
            {
                Id = newBenchmark.ExecutableId
            }, cancellationToken));
        await writer.WriteAttributeStringAsync(null, "timelimit", null, 
            newBenchmark.TimeLimit+"s");
        await writer.WriteAttributeStringAsync(null, "hardtimelimit", null,
            newBenchmark.HardTimeLimit+"s");
        await writer.WriteAttributeStringAsync(null, "memlimit", null,
            newBenchmark.Ram+"GB");
        await writer.WriteAttributeStringAsync(null, "cpuCores", null,
            newBenchmark.Cpu.ToString());
        
            
        var config = await _mediator.Send(new GetConfigurationByIdQuery
        {
            Id = newBenchmark.ConfigurationId
        }, cancellationToken) ?? throw new ApplicationException("Configuration not found.");

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
                        newBenchmark.SetFilePath!.Split(Path.DirectorySeparatorChar).Last().Split(".")[0]);
                    await writer.WriteStartElementAsync(null, "includesfile", null);
                    await writer.WriteStringAsync("sv-benchmarks" + Path.DirectorySeparatorChar + "c" 
                                                  + Path.DirectorySeparatorChar 
                                                  + newBenchmark.SetFilePath!.Split(Path.DirectorySeparatorChar).Last());
                    await writer.WriteEndElementAsync();
                    await writer.WriteStartElementAsync(null, "propertyfile", null);
                    await writer.WriteStringAsync("sv-benchmarks" + Path.DirectorySeparatorChar + "c" 
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
    /// <param name="startedDate">The registered start time</param>
    /// <param name="xmlName">The absolute path of the xml config</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ApplicationException">Shows server-side problem</exception>
    private async Task StartBenchmark(BenchmarkHeader newBenchmark, DateTime startedDate, string xmlName,
        CancellationToken cancellationToken)
    {
        var exe = await _context.Executables.Where(x => x.Id == newBenchmark.ExecutableId)
            .Select(x => new ExecutableHeader(x)).FirstOrDefaultAsync(cancellationToken);
        if (exe == null)
            throw new ApplicationException("The connecting executable not found.");

        var resultDir = _workDir + Path.DirectorySeparatorChar + newBenchmark.UserName
                        + Path.DirectorySeparatorChar + "results"
                        + Path.DirectorySeparatorChar
                        + exe.Name + "_" + startedDate.ToString("%Y-%m-%d_%H:%M:%S");
        // prepare results directory
        Directory.CreateDirectory(resultDir);

        var xmlRelativePath = xmlName.TrimStart((_workDir + Path.DirectorySeparatorChar).ToCharArray());
        var toolDir = newBenchmark.UserName + Path.DirectorySeparatorChar + "tools" + Path.DirectorySeparatorChar
                      + exe.Name;

        var result = await Cli.Wrap(_vcloudBenchmarkPath)
            .WithArguments(new [] {"--tryLessMemory", "--no-container", xmlRelativePath, "--tool-directory", toolDir, 
                "--vcloudAdditionalFiles", toolDir, "-o", resultDir, "--vcloudPriority", newBenchmark.Priority.ToString()})
            .WithWorkingDirectory(_workDir)
            .ExecuteAsync(cancellationToken);
    }
    
    
}