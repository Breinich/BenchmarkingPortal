using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class BenchmarkHeader
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Priority { get; set; }

    public Status Status { get; set; }

    public int Ram { get; set; }

    public int Cpu { get; set; }

    public string? Result { get; set; }

    public int TimeLimit { get; set; }

    public int HardTimeLimit { get; set; }

    public int ComputerGroupId { get; set; }

    public int ExecutableId { get; set; }

    public int SourceSetId { get; set; }

    public string? SetFilePath { get; set; }

    public string? PropertyFilePath { get; set; }

    public DateTime StartedDate { get; set; }

    public int ConfigurationId { get; set; }
    public int UserId { get; set; }

    public BenchmarkHeader() { }

    public BenchmarkHeader(Benchmark b)
    {
        Id = b.Id;
        Name = b.Name;
        Priority = b.Priority;
        Status = b.Status;
        Ram = b.Ram;
        Cpu = b.Cpu;
        Result = b.Result;
        TimeLimit = b.TimeLimit;
        HardTimeLimit = b.HardTimeLimit;
        ComputerGroupId = b.ComputerGroupId;
        ExecutableId = b.ExecutableId;
        SourceSetId = b.SourceSetId;
        SetFilePath = b.SetFilePath;
        PropertyFilePath = b.PropertyFilePath;
        StartedDate = b.StartedDate;
        ConfigurationId = b.ConfigurationId;
        UserId = b.UserId;
    }

}
