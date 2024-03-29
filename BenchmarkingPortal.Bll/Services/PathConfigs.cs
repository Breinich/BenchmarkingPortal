﻿namespace BenchmarkingPortal.Bll.Services;

public class PathConfigs
{
    public string WorkingDir { get; init; } = null!;
    public string VcloudBenchmarkPath { get; init; } = null!;
    public string VcloudDirectory { get; init; } = null!;
    public string WorkerConfig { get; init; } = null!;
    public string SshConfig { get; init; } = null!;
    public string SshPubKey { get; init; } = null!;
    public string VcloudHost { get; init; } = null!;
    public string Tab { get; init; } = null!;
}