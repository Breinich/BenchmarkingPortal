using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class BenchmarkEntityConfiguration : IEntityTypeConfiguration<Benchmark>
{
    public void Configure(EntityTypeBuilder<Benchmark> builder)
    {
        builder.ToTable("Benchmarks");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.PropertyFilePath).HasMaxLength(250).IsRequired();
        builder.Property(e => e.ResultPath).HasMaxLength(250);
        builder.Property(e => e.SetFilePath).HasMaxLength(250).IsRequired();
        builder.Property(e => e.StartedDate).IsRequired();
        builder.Property(e => e.Cpu).IsRequired();
        builder.Property(e => e.Ram).IsRequired();
        builder.Property(e => e.Status).HasConversion(s => s.ToString(),
            v => (Status)Enum.Parse(typeof(Status), v)).IsRequired();
        builder.Property(e => e.TimeLimit).IsRequired();
        builder.Property(e => e.HardTimeLimit).IsRequired();
        builder.Property(e => e.CpuModel).HasMaxLength(50).IsRequired();

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasOne(d => d.ComputerGroup).WithMany(p => p.Benchmarks)
            .HasForeignKey(d => d.ComputerGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Benchmark_ComputerGroup");

        builder.HasOne(d => d.Configuration).WithMany()
            .HasForeignKey(d => d.ConfigurationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Benchmark_Configuration");

        builder.HasOne(d => d.Executable).WithMany(p => p.Benchmarks)
            .HasForeignKey(d => d.ExecutableId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Benchmark_Executable");

        builder.HasOne(d => d.SourceSet).WithMany()
            .HasForeignKey(d => d.SourceSetId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Benchmark_SourceSet");

        builder.HasOne(d => d.User).WithMany(p => p.Benchmarks)
            .HasForeignKey(d => d.UserName)
            .HasPrincipalKey(d => d.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Benchmark_User");

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<Benchmark> builder)
    {
        // Load test data here
    }
}