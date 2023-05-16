using BenchmarkingPortal.Dal.Entities;

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

    public string UserName { get; set; } = null!;

    public ExecutableHeader() { }

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
}
