using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class CpuModelHeader
{
    public CpuModelHeader() { }

    public CpuModelHeader(CpuModel cm)
    {
        Id = cm.Id;
        Value = cm.Value;
        Name = cm.Name;
    }

    public int Id { get; set; }
    public string? Value { get; set; }
    public string? Name { get; set; }
}
