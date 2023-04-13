namespace BenchmarkingPortal.Dal.Entities;

public partial class Executable
{
    public int Id { get; set; }

    public string OwnerTool { get; set; } = null!;

    public string ToolVersion { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public DateTime UploadedDate { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Benchmark> Benchmarks { get; } = new List<Benchmark>();

    public virtual User User { get; set; } = null!;
}
