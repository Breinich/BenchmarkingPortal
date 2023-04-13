namespace BenchmarkingPortal.Dal.Entities;

public partial class ComputerGroup
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Benchmark> Benchmarks { get; } = new List<Benchmark>();

    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}
