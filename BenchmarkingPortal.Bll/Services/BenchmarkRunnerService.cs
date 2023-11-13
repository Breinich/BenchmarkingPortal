using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BenchmarkingPortal.Bll.Services;

public class BenchmarkRunnerService : BackgroundService
{
    private IBenchmarkQueue Benchmarks { get; }
    private ILogger<BenchmarkRunnerService> Logger { get; }
    private IServiceScopeFactory ScopeFactory { get; }
    
    public BenchmarkRunnerService(IBenchmarkQueue queue, ILogger<BenchmarkRunnerService> logger, IServiceScopeFactory scopeFactory)
    {
        Benchmarks = queue;
        Logger = logger;
        ScopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var task = await Benchmarks.DequeueAsync(stoppingToken);
                
                _ = Task.Run(async () => await task.ExecuteAsync(ScopeFactory, stoppingToken), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            Logger.Log(LogLevel.Information, "The benchmark runner service was stopped.");
        }
    }
}