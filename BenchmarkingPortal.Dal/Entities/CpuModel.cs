namespace BenchmarkingPortal.Dal.Entities;

public class CpuModel
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public string Name { get; set; } = null!;
    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
    public virtual ICollection<Benchmark> Benchmarks { get; } = new List<Benchmark>();
}
