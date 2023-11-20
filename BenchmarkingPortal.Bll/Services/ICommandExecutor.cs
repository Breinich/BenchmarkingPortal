namespace BenchmarkingPortal.Bll.Services;

public interface ICommandExecutor : IDisposable
{
    Task ExecuteAsync(string command, IEnumerable<string> args);
    Task InitializeAsync();
}