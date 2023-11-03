using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Dal.Entities;

public class User : IdentityUser<int>
{
    public bool Subscription { get; set; } = true;

    public virtual ICollection<Benchmark> Benchmarks { get; set; } = new List<Benchmark>();
    public virtual ICollection<Executable> Executables { get; set; } = new List<Executable>();
    public virtual ICollection<SetFile> SetFiles { get; set; } = new List<SetFile>();
}