using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using CliWrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BenchmarkingPortal.Bll.Services;

public class BenchmarkTask
{
    private BenchmarkHeader BenchmarkInfo { get; }
    private Command RunConfig { get; }
    public BenchmarkTask(BenchmarkHeader benchmarkInfo, Command runConfig)
    {
        BenchmarkInfo = benchmarkInfo;
        RunConfig = runConfig;
    }

    /// <summary>
    /// Run the benchmark and update the status in the database.
    /// </summary>
    /// <param name="scopeFactory">Scope factory for resolving the DB context</param>
    /// <param name="cancellationToken"></param>
    public async ValueTask ExecuteAsync(IServiceScopeFactory scopeFactory, CancellationToken cancellationToken)
    {
        if (scopeFactory == null) throw new ArgumentNullException("scopeFactory");
        using var scope = scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BenchmarkTask>>();
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<BenchmarkingDbContext>();
            
            Benchmark? benchmark = null;
            while(benchmark == null)
            {
                benchmark = await context.Benchmarks.Where(b => b.Name == BenchmarkInfo.Name)
                    .FirstOrDefaultAsync(cancellationToken);
                await Task.Delay(500, cancellationToken);
            }
            
            benchmark.Status = Status.Running;
            await context.SaveChangesAsync(cancellationToken);

            var result = await RunConfig.ExecuteAsync(cancellationToken);

            if (result.ExitCode != 0)
            {
                logger.Log(LogLevel.Warning, $"The benchmark {BenchmarkInfo.Name} did not finish successfully.\n" +
                                             "You can check the errors in the error.log file among the result files.");
            }
            else
            {
                logger.Log(LogLevel.Information, $"The benchmark {BenchmarkInfo.Name} finished successfully.");
            }

            benchmark.Status = Status.Finished;
            await context.SaveChangesAsync(cancellationToken);
           
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, ex, "An error occurred while executing the task.");
        }
    }
}