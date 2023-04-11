namespace BenchmarkingPortal.Dal.Dtos;

public class ExecutableHeader
{
    public int Id { get; set; }

    public string? OwnerTool { get; set; }

    public string? ToolVersion { get; set; }

    public string? Path { get; set; }

    public string? Name { get; set; }

    public string? Version { get; set; }

    public DateTime UploadedDate { get; set; }

    public int UserId { get; set; }

    public ICollection<BenchmarkHeader>? Benchmarks { get; set; }

    public UserHeader? User { get; set; }
}
