using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using CliWrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BenchmarkingPortal.Bll.Services;

public class BenchmarkTask
{
    private BenchmarkHeader BenchmarkInfo { get; }
    private Command RunConfig { get; }
    private readonly BenchmarkingDbContext _context;
    private readonly ILogger<BenchmarkTask> _logger;
    
    public BenchmarkTask(BenchmarkHeader benchmarkInfo, Command runConfig, BenchmarkingDbContext context, ILogger<BenchmarkTask> logger)
    {
        BenchmarkInfo = benchmarkInfo;
        RunConfig = runConfig;
        _context = context;
        _logger = logger;
    }
    
    /// <summary>
    /// Run the benchmark and update the status in the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await RunConfig.ExecuteAsync(cancellationToken);
        
        var benchmark = await _context.Benchmarks.Where(b => b.Name == BenchmarkInfo.Name).FirstAsync(cancellationToken);
        benchmark.Status = Status.Finished;
        
        if (result.ExitCode != 0)
        {
            _logger.Log(LogLevel.Warning, $"The benchmark {BenchmarkInfo.Name} did not finish successfully.\n" +
                                          "You can check the errors in the error.log file among the result files.");
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}