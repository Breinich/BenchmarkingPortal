namespace BenchmarkingPortal.Bll.Services;

public interface IBenchmarkQueue
{
    public ValueTask QueueBenchmarkAsync(BenchmarkTask task, CancellationToken cancellationToken);
    
    public ValueTask<BenchmarkTask> DequeueAsync(CancellationToken cancellationToken);
}