using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class SourceSetHeader
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Path { get; set; }

    public string? Version { get; set; }

    public DateTime UploadedDate { get; set; }
    public int UserId { get; set; }

    public SourceSetHeader() { }

    public SourceSetHeader(SourceSet s)
    {
        Id = s.Id;
        Name = s.Name;
        Path = s.Path;
        UploadedDate = s.UploadedDate;
        UserId = s.UserId;
        Version = s.Version;
    }
}
