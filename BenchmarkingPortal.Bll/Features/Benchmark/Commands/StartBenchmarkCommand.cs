using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

/// <summary>
/// The command for starting a benchmark.
/// </summary>
public class StartBenchmarkCommand : IRequest<BenchmarkHeader>
{
    public string Name { get; init; } = null!;
    public Priority Priority { get; init; }
    public int Ram { get; init; }
    public int Cpu { get; init; }
    public int TimeLimit { get; set; }
    public int HardTimeLimit { get; set; }
    public int CpuModelId { get; init; }
    public string? CpuModelValue { get; init; }
    public int ExecutableId { get; init; }
    public string SetFilePath { get; init; } = null!;
    public string PropertyFilePath { get; init; } = null!;
    public int ConfigurationId { get; init; }
    public int ComputerGroupId { get; init; }
    public string InvokerName { get; init; } = null!;
}