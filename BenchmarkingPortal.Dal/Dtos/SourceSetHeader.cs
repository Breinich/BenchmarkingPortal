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
}
