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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.ApplyConfiguration(new BenchmarkEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ComputerGroupEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ConfigurationEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ConfigurationItemEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ConstraintEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ExecutableEntityConfiguration());

        modelBuilder.ApplyConfiguration(new SourceSetEntityConfiguration());

        modelBuilder.ApplyConfiguration(new WorkerEntityConfiguration());

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());


        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        //Add seed data here, if not at the configurations
    }
}
