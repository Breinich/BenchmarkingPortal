using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class BenchmarkHeader
{
    public BenchmarkHeader()
    {
    }

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
        CpuModel = b.CpuModel;
        ComputerGroupId = b.ComputerGroupId;
        ExecutableId = b.ExecutableId;
        SetFilePath = b.SetFilePath;
        PropertyFilePath = b.PropertyFilePath;
        StartedDate = b.StartedDate;
        ConfigurationId = b.ConfigurationId;
        UserName = b.UserName;
    }

    public int Id { get; init; }

    public string? Name { get; init; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }

    public int Ram { get; init; }

    public int Cpu { get; init; }

    public string? ResultPath { get; init; }

    public int TimeLimit { get; init; }

    public int HardTimeLimit { get; init; }
    
    public string? CpuModel { get; init; }

    public int ComputerGroupId { get; init; }

    public int ExecutableId { get; init; }

    public string? SetFilePath { get; init; }

    public string? PropertyFilePath { get; init; }

    public DateTime StartedDate { get; init; }

    public int ConfigurationId { get; init; }
    public string UserName { get; init; } = null!;
}