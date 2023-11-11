namespace BenchmarkingPortal.Bll.Services;

public class StoragePaths
{
    public string WorkingDir { get; init; } = null!;
    public string SetFilesDir { get; init; } = null!;
    public string PropertyFilesDir { get; init; } = null!;
    public string VcloudBenchmarkPath { get; init; } = null!;
    public string WorkerConfig { get; set; } = null!;
    public string SshConfig { get; set; } = null!;
}