using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Dal;

public partial class BenchmarkingDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{

    public BenchmarkingDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual DbSet<Benchmark> Benchmarks => Set<Benchmark>();

    public virtual DbSet<ComputerGroup> ComputerGroups => Set<ComputerGroup>();

    public virtual DbSet<Configuration> Configurations => Set<Configuration>();

    public virtual DbSet<ConfigurationItem> ConfigurationItems => Set<ConfigurationItem>();

    public virtual DbSet<Constraint> Constraints => Set<Constraint>();

    public virtual DbSet<Executable> Executables => Set<Executable>();

    public virtual DbSet<SourceSet> SourceSets => Set<SourceSet>();

    public virtual DbSet<Worker> Workers => Set<Worker>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        
        builder.ApplyConfiguration(new BenchmarkEntityConfiguration());

        builder.ApplyConfiguration(new ComputerGroupEntityConfiguration());

        builder.ApplyConfiguration(new ConfigurationEntityConfiguration());

        builder.ApplyConfiguration(new ConfigurationItemEntityConfiguration());

        builder.ApplyConfiguration(new ConstraintEntityConfiguration());

        builder.ApplyConfiguration(new ExecutableEntityConfiguration());

        builder.ApplyConfiguration(new SourceSetEntityConfiguration());

        builder.ApplyConfiguration(new WorkerEntityConfiguration());

        builder.ApplyConfiguration(new UserEntityConfiguration());


        SeedData(builder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        //Add seed data here, if not at the configurations
    }
}
