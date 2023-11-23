namespace BenchmarkingPortal.Bll.Services;

public interface ICommandExecutor : IDisposable
{
    Task<string> ExecuteAsync(string command, IEnumerable<string> args);
    Task InitializeAsync();
}