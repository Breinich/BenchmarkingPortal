using BenchmarkingPortal.Dal;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

namespace BenchmarkingPortal.Web.Hosting;

public sealed class ExpiredFilesCleanupService : IHostedService, IDisposable
{
    private readonly ExpirationBase _expiration;
    private readonly ITusExpirationStore _expirationStore;
    private readonly ILogger<ExpiredFilesCleanupService> _logger;
    private Timer? _timer;

    public ExpiredFilesCleanupService(ILogger<ExpiredFilesCleanupService> logger, DefaultTusConfiguration config)
    {
        _logger = logger;
        _expirationStore = (ITusExpirationStore)config.Store;
        _expiration = config.Expiration;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_expiration == null)
        {
            _logger.LogInformation("Not running cleanup job as no expiration has been set.");
            return;
        }

        await RunCleanup(cancellationToken);
        // ReSharper disable once AsyncVoidLambda
        _timer = new Timer(async e => await RunCleanup((CancellationToken)(e ?? throw new ArgumentNullException(nameof(e)))), cancellationToken, TimeSpan.Zero,
            _expiration.Timeout);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async Task RunCleanup(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Running cleanup job...");
            var numberOfRemovedFiles = await _expirationStore.RemoveExpiredFilesAsync(cancellationToken);
            _logger.LogInformation(
                $"Removed {numberOfRemovedFiles} expired files. Scheduled to run again in {_expiration.Timeout.TotalMilliseconds} ms");
        }
        catch (Exception exc)
        {
            _logger.LogWarning("Failed to run cleanup job: " + exc.Message);
        }
    }
}