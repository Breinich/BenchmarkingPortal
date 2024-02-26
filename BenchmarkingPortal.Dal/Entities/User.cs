using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Dal.Entities;

public class User : IdentityUser<int>
{
    public bool Subscription { get; set; } = true;

    public virtual ICollection<Benchmark> Benchmarks { get; } = new List<Benchmark>();
    public virtual ICollection<Executable> Executables { get; } = new List<Executable>();
    public virtual ICollection<SetFile> SetFiles { get; } = new List<SetFile>();
    public virtual ICollection<PropertyFile> PropertyFiles { get; } = new List<PropertyFile>();
    public virtual ICollection<SourceSet> SourceSets { get; } = new List<SourceSet>();
}