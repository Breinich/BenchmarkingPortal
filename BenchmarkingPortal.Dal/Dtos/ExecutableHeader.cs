using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ExecutableHeader
{
    public ExecutableHeader()
    {
    }

    public ExecutableHeader(Executable e)
    {
        Id = e.Id;
        OwnerTool = e.OwnerTool;
        ToolVersion = e.ToolVersion;
        Path = e.Path;
        Name = e.Name;
        Version = e.Version;
        UploadedDate = e.UploadedDate;
        UserName = e.UserName;
    }

    public int Id { get; init; }
    public string? OwnerTool { get; init; }
    public string? ToolVersion { get; init; }
    public string? Path { get; init; }
    public string Name { get; init; } = null!;
    public string? Version { get; init; }
    public DateTime UploadedDate { get; init; }
    public string UserName { get; init; } = null!;
}