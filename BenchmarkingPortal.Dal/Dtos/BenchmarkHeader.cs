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
        CpuModelId = b.CpuModelId;
        ComputerGroupId = b.ComputerGroupId;
        ExecutableId = b.ExecutableId;
        SetFilePath = b.SetFilePath;
        PropertyFilePath = b.PropertyFilePath;
        XmlFilePath = b.XmlFilePath;
        StartedDate = b.StartedDate;
        ConfigurationId = b.ConfigurationId;
        UserName = b.UserName;
        VcloudId = b.VcloudId;
    }

    public int Id { get; init; }
    public string? Name { get; init; }
    public Priority Priority { get; set; }
    public Status Status { get; set; }
    public int Ram { get; init; }
    public int Cpu { get; init; }
    public string? ResultPath { get; set; }
    public int TimeLimit { get; init; }
    public int HardTimeLimit { get; init; }
    public int CpuModelId { get; init; }
    public int ComputerGroupId { get; init; }
    public int ExecutableId { get; init; }
    public string? SetFilePath { get; init; }
    public string? PropertyFilePath { get; init; }
    public string? XmlFilePath { get; set; }
    public DateTime StartedDate { get; set; }
    public int ConfigurationId { get; init; }
    public string UserName { get; init; } = null!;
    
    public string? VcloudId { get; set; }
    public string? CpuModelValue { get; init; }
}