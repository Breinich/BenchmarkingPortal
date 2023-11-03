using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class SetFileHeader
{
    public SetFileHeader()
    {
    }

    public SetFileHeader(SetFile s)
    {
        Id = s.Id;
        Name = s.Name;
        Path = s.Path;
        UploadedDate = s.UploadedDate;
        UserName = s.UserName;
        Version = s.Version;
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Path { get; set; }

    public string? Version { get; set; }

    public DateTime UploadedDate { get; set; }
    public string UserName { get; set; } = null!;
}