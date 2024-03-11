using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class SourceSetHeader
{
    public SourceSetHeader() { }

    public SourceSetHeader(SourceSet sc)
    {
        Id = sc.Id;
        Name = sc.Name;
        Root = sc.Root;
        UserName = sc.UserName;
    }

    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Root { get; init; }
    public string? UserName { get; init; }
    
    public string PropertyFilesPath => Path.Combine(Root ?? throw new ArgumentException(nameof(SourceSet) + "." + nameof(Root) + " is not set!"), "c", "properties");
    public string SetFilesPath => Path.Combine(Root ?? throw new ArgumentException(nameof(SourceSet) + "." + nameof(Root) + " is not set!"), "c");
}
