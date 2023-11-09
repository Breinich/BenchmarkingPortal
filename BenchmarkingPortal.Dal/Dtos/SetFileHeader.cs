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

    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string? Path { get; init; }

    public string? Version { get; init; }

    public DateTime UploadedDate { get; init; }
    public string UserName { get; init; } = null!;
}