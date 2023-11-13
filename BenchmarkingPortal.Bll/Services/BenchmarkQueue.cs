using System.Threading.Channels;

namespace BenchmarkingPortal.Bll.Services;

public class BenchmarkQueue : IBenchmarkQueue
{
    private readonly Channel<BenchmarkTask> _queue = Channel.CreateUnbounded<BenchmarkTask>();

    public async ValueTask QueueBenchmarkAsync(BenchmarkTask task, CancellationToken cancellationToken)
    {
        if (task == null)
            throw new ArgumentException("Command parameter is not defined!\n" +
                                        "Nothing added to queue.");

        await _queue.Writer.WriteAsync(task, cancellationToken);
    }

    public async ValueTask<BenchmarkTask> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}