namespace BenchmarkingPortal.Dal.Entities;

public class ComputerGroup
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public string Hostname { get; set; } = null!;
    public virtual ICollection<Benchmark> Benchmarks { get; } = new List<Benchmark>();
    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}