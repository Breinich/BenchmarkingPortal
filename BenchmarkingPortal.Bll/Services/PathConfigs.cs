namespace BenchmarkingPortal.Bll.Services;

public class PathConfigs
{
    public string WorkingDir { get; init; } = null!;
    public string ExecutableDir { get; init; } = null!;
    public string SourceSetDir { get; init; } = null!;
    public string SetFileDir { get; init; } = null!;
    public string PropertyFilesDir { get; init; } = null!;
    public string ResultsDir { get; init; } = null!;
    public string BenchmarkDir { get; init; } = null!;
    public string VcloudBenchmarkPath { get; init; } = null!;
    public string VcloudDir { get; init; } = null!;
    public string WorkerConfig { get; init; } = null!;
    public string SshConfig { get; init; } = null!;
    public string SshPubKey { get; init; } = null!;
    public string VcloudHost { get; init; } = null!;
    public string Tab { get; init; } = null!;
}