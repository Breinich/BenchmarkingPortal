using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

/// <summary>
/// Represents a <see cref="Benchmark"/>'s header.
/// </summary>
public class BenchmarkHeader
{
    /// <summary>
    /// Default constructor for BenchmarkHeader.
    /// </summary>
    public BenchmarkHeader() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarkHeader"/> class.
    /// </summary>
    /// <param name="b">The <see cref="Benchmark"/> entity.</param>
    public BenchmarkHeader(Benchmark b)
    {
        Id = b.Id;
        Name = b.Name;
        Priority = b.Priority;
        Status = b.Status;
        Ram = b.Ram;
        Cpu = b.Cpu;
        ResultPath = b.ResultPath;
        TimeLimit = b.TimeLimit;
        HardTimeLimit = b.HardTimeLimit;
        CpuModelId = b.CpuModelId;
        ComputerGroupId = b.ComputerGroupId;
        ExecutableId = b.ExecutableId;
        SetFilePath = b.SetFilePath;
        PropertyFilePath = b.PropertyFilePath;
        StartedDate = b.StartedDate;
        ConfigurationId = b.ConfigurationId;
        UserName = b.UserName;
        VcloudId = b.VcloudId;
    }

    /// <summary>
    /// <see cref="Benchmark.Id"/>
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// <see cref="Benchmark.Name"/>
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// <see cref="Benchmark.Priority"/>
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// <see cref="Benchmark.Status"/>
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// <see cref="Benchmark.Ram"/>
    /// </summary>
    public int Ram { get; init; }

    /// <summary>
    /// <see cref="Benchmark.Cpu"/>
    /// </summary>
    public int Cpu { get; init; }

    /// <summary>
    /// <see cref="Benchmark.ResultPath"/>
    /// </summary>
    public string? ResultPath { get; set; }

    /// <summary>
    /// <see cref="Benchmark.TimeLimit"/>
    /// </summary>
    public int TimeLimit { get; init; }

    /// <summary>
    /// <see cref="Benchmark.HardTimeLimit"/>
    /// </summary>
    public int HardTimeLimit { get; init; }

    /// <summary>
    /// <see cref="Benchmark.CpuModelId"/>
    /// </summary>
    public int CpuModelId { get; init; }

    /// <summary>
    /// <see cref="Benchmark.ComputerGroupId"/>
    /// </summary>
    public int ComputerGroupId { get; init; }

    /// <summary>
    /// <see cref="Benchmark.ExecutableId"/>
    /// </summary>
    public int ExecutableId { get; init; }

    /// <summary>
    /// <see cref="Benchmark.SetFilePath"/>
    /// </summary>
    public string? SetFilePath { get; init; }

    /// <summary>
    /// <see cref="Benchmark.PropertyFilePath"/>
    /// </summary>
    public string? PropertyFilePath { get; init; }

    /// <summary>
    /// <see cref="Benchmark.StartedDate"/>
    /// </summary>
    public DateTime StartedDate { get; set; }

    /// <summary>
    /// <see cref="Benchmark.ConfigurationId"/>
    /// </summary>
    public int ConfigurationId { get; init; }

    /// <summary>
    /// <see cref="Benchmark.UserName"/>
    /// </summary>
    public string UserName { get; init; } = null!;

    /// <summary>
    /// <see cref="Benchmark.VcloudId"/>
    /// </summary>
    public string? VcloudId { get; set; }

    /// <summary>
    /// The name of the target CPU model.
    /// </summary>
    public string? CpuModelValue { get; init; }
}