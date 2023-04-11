using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ComputerGroupEntityConfiguration : IEntityTypeConfiguration<ComputerGroup>
{
    public void Configure(EntityTypeBuilder<ComputerGroup> builder)
    {
        builder.ToTable("ComputerGroup");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Description).HasMaxLength(50);

        builder.HasMany(p => p.Workers).WithOne()
            .HasForeignKey(d => d.ComputerGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Worker_ComputerGroup");

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<ComputerGroup> builder)
    {
        // Load test data here

    }
}