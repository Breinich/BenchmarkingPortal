namespace BenchmarkingPortal.Dal.Dtos;

public class ComputerGroupHeader
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public ICollection<BenchmarkHeader>? Benchmarks { get; set; }

    public ICollection<WorkerHeader>? Workers { get; set; }
}
