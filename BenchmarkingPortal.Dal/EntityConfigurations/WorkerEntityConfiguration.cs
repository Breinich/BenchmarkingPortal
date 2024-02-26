using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class WorkerEntityConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("Workers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.AddedDate).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Password).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Login).HasMaxLength(50).IsRequired();

        builder.HasOne(w => w.User).WithMany()
            .HasForeignKey(w => w.UserName)
            .HasPrincipalKey(u => u.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Worker_User");

        builder.HasOne(w => w.ComputerGroup).WithMany(w => w.Workers)
            .HasForeignKey(w => w.ComputerGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Worker_ComputerGroup");

        builder.HasOne(w => w.CpuModel).WithMany(w => w.Workers)
            .HasForeignKey(w => w.CpuModelId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Worker_CpuModel");

        SampleData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SampleData(EntityTypeBuilder<Worker> builder)
    {
        // Load test data here
    }
}