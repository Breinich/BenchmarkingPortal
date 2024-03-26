using System.Xml;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.CommandHandlers;

/// <summary>
/// The handler for the <see cref="CreateConfigurationCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand, ConfigurationHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly string _workDir;
    private readonly IMediator _mediator;

    public CreateConfigurationCommandHandler(BenchmarkingDbContext context,  PathConfigs pathConfigs, IMediator mediator)
    {
        _context = context;
        _workDir = pathConfigs.WorkingDir;
        _mediator = mediator;
    }


    public async Task<ConfigurationHeader> Handle(CreateConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        // Value validations:
        
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
        
        if (request.BenchmarkName != request.InvokerName)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.NoPrivilege);
        
        var newBenchmark = new BenchmarkHeader
        {
            Name = request.BenchmarkName,
            Status = Status.Running,
            Ram = request.Ram,
            Cpu = request.Cpu,
            TimeLimit = request.TimeLimit,
            HardTimeLimit = request.HardTimeLimit,
            ExecutableId = request.ExecutableId,
            SetFilePath = request.SetFilePath,
            PropertyFilePath = request.PropertyFilePath,
            UserName = request.InvokerName
        };
        
        Directory.CreateDirectory(Path.Join(_workDir, newBenchmark.UserName, "benchmarks"));
        var xmlFilePath = Path.Join(_workDir, newBenchmark.UserName, "benchmarks", newBenchmark.Name + ".xml");
        
        var exe = await _mediator.Send(new GetExecutableByIdQuery
        {
            Id = newBenchmark.ExecutableId
        }, cancellationToken) ?? throw new ApplicationException("The according executable not found.");

        var configHeader = new ConfigurationHeader
        {
            Constraints = request.Constraints?.Select(c => new ConstraintHeader { Expression = c }).ToList(),
            ConfigurationItems = request.Configurations.Select(c => new ConfigurationItemHeader
            {
                Scope = c.Item1,
                Key = c.Item2,
                Value = c.Item3
            }).ToList()
        };
        
        var removedConfigs = await CreateXmlSetup(configHeader, newBenchmark, xmlFilePath, exe.OwnerTool);

        var config = new Dal.Entities.Configuration
        {
            XmlFilePath = xmlFilePath
        };

        await _context.Configurations.AddAsync(config, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        
        foreach (var configItem in configHeader.ConfigurationItems)
            await _context.ConfigurationItems.AddAsync(new ConfigurationItem
            {
                Key = configItem.Key!,
                Value = configItem.Value!,
                Scope = configItem.Scope,
                ConfigurationId = config.Id
            }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        if (configHeader.Constraints != null)
            foreach (var constraint in configHeader.Constraints)
                await _context.Constraints.AddAsync(new Constraint
                {
                    Expression = constraint.Expression!,
                    ConfigurationId = config.Id
                }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ConfigurationHeader
        {
            Id = config.Id,
            RemovedItems = removedConfigs
        };
    }

    /// <summary>
    /// Creates the XML file for the benchmark
    /// </summary>
    /// <param name="config">Configuration info</param>
    /// <param name="newBenchmark">Benchmark info</param>
    /// <param name="xmlFilePath"> The path to write the XML file to</param>
    /// <param name="exeToolName">The name of the executable's owner tool</param>
    private static async Task<List<List<ConfigurationItemHeader>>> CreateXmlSetup(ConfigurationHeader config, BenchmarkHeader newBenchmark, string xmlFilePath, string? exeToolName)
    {
        var removedConfigs = new List<List<ConfigurationItemHeader>>();
            
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            Async = true
        };

        await using var writer = XmlWriter.Create(xmlFilePath, settings);
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
        
        if (config.ConfigurationItems != null)
        {
            var globalItems = config.ConfigurationItems.Where(ci => ci.Scope == Scope.Global).ToList();
            foreach (var item in globalItems)
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
                        optionList.Add(new List<ConfigurationItemHeader> { item });
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
            
            // Add the global configuration items to the end of the list
            optionList.AddRange(globalItems.Select(item => new List<ConfigurationItemHeader> { item }));

            if (optionList.Count > globalItems.Count)
            {
                // Create the cross product of the local configuration items
                var crossProduct = CrossProduct(optionList).Select(l => l.ToList()).ToList();
                List<List<ConfigurationItemHeader>> filteredCrossProduct = new();
                filteredCrossProduct.AddRange(crossProduct);

                try
                {
                    // filter the cross product based on the constraints
                    if (config.Constraints != null)
                    {
                        var evaluator = new ConstraintEvaluator();
                        filteredCrossProduct = config.Constraints.Aggregate(filteredCrossProduct,
                            (current, constraint) => current.FindAll(evaluator.Evaluate(constraint.Expression!)));

                        removedConfigs = crossProduct.Except(filteredCrossProduct).ToList();
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Error while evaluating the constraints: " + e.Message);
                }

                // remove the global configurations from the lists of the cross products
                foreach (var list in filteredCrossProduct)
                {
                    list.RemoveAll(ci => globalItems.Select(gi => gi.Key).Contains(ci.Key));
                }
                foreach (var list in removedConfigs)
                {
                    list.RemoveAll(ci => globalItems.Select(gi => gi.Key).Contains(ci.Key));
                }

                foreach (var configurationItemHeaders in filteredCrossProduct)
                {
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
                        Path.ChangeExtension(Path.GetFileName(newBenchmark.SetFilePath!)
                            .TrimStart('.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', '-', '+'), null));
                    await writer.WriteStartElementAsync(null, "includesfile", null);
                    await writer.WriteStringAsync(Path.Join("..", "..", "sv-benchmarks", "c", Path.GetFileName(newBenchmark.SetFilePath!)));
                    await writer.WriteEndElementAsync();
                    await writer.WriteStartElementAsync(null, "propertyfile", null);
                    await writer.WriteStringAsync(Path.Join("..", "..", "sv-benchmarks", "c", "properties", Path.GetFileName(newBenchmark.PropertyFilePath!)));
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                }
            }
        }

        await writer.WriteEndElementAsync();
        await writer.FlushAsync();

        return removedConfigs;
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
            (acc, src) => src.SelectMany<T, IEnumerable<T>>(x => acc.Select<IEnumerable<T>, IEnumerable<T>>(a => a.Concat(new[]
            {
                x
            })))
        );
}