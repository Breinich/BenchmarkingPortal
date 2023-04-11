﻿using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Dal.Entities;

public partial class User : IdentityUser<int>
{
    public bool Subscription { get; set; } = true;
    
    public string? GitHubUserName { get; set; }

    public virtual  ICollection<Benchmark> Benchmarks { get; set; } = new List<Benchmark>();
    public virtual ICollection<Executable> Executables { get; set; } = new List<Executable>();
    public virtual ICollection<SourceSet> SourceSets { get; set; } = new List<SourceSet>();
}