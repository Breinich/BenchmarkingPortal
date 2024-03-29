﻿using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BenchmarkingPortal.Dal;

public class BenchmarkingDbContext : IdentityDbContext<User, IdentityRole<int>, int>
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
    public virtual DbSet<CpuModel> CpuModels => Set<CpuModel>();
    public virtual DbSet<Executable> Executables => Set<Executable>();
    public virtual DbSet<PropertyFile> PropertyFiles => Set<PropertyFile>();
    public virtual DbSet<SetFile> SetFiles => Set<SetFile>();
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
        builder.ApplyConfiguration(new CpuModelEntityConfiguration());
        builder.ApplyConfiguration(new ExecutableEntityConfiguration());
        builder.ApplyConfiguration(new PropertyFileEntityConfiguration());
        builder.ApplyConfiguration(new SetFileEntityConfiguration());
        builder.ApplyConfiguration(new SourceSetEntityConfiguration());
        builder.ApplyConfiguration(new WorkerEntityConfiguration());
        builder.ApplyConfiguration(new UserEntityConfiguration());

        SeedData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SeedData(ModelBuilder modelBuilder)
    {
        //Add seed data here, if not at the configurations
    }
}