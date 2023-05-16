namespace BenchmarkingPortal.Dal.Entities;

public class SourceSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Version { get; set; } = null!;

    public DateTime UploadedDate { get; set; }
    public string UserName { get; set; } = null!;


    public virtual User User { get; set; } = null!;
}