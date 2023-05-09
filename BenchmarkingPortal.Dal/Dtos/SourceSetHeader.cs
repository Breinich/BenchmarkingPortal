using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class SourceSetHeader
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Path { get; set; }

    public string? Version { get; set; }

    public DateTime UploadedDate { get; set; }
    public UserHeader User { get; set; } = null!;

    public SourceSetHeader() { }

    public SourceSetHeader(SourceSet s)
    {
        Id = s.Id;
        Name = s.Name;
        Path = s.Path;
        UploadedDate = s.UploadedDate;
        User = new UserHeader(s.User);
        Version = s.Version;
    }
}
