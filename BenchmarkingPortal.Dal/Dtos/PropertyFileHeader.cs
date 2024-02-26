using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class PropertyFileHeader
{
    public PropertyFileHeader() { }

    public PropertyFileHeader(PropertyFile pf)
    {
        Id = pf.Id;
        Name = pf.Name;
        Path = pf.Path;
        Version = pf.Version;
        SourceSetId = pf.SourceSetId;
        UploadedDate = pf.UploadedDate;
        UserName = pf.UserName;
    }

    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Path { get; init; }
    public string? Version { get; init; }
    public int SourceSetId { get; init; }
    public DateTime UploadedDate { get; init; }
    public string? UserName { get; init; }
}
