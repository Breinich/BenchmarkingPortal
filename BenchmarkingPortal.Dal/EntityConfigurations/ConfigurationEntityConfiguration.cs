using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ConfigurationEntityConfiguration : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        builder.ToTable("Configurations");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.XmlFilePath).HasMaxLength(250).IsRequired();

        builder.HasMany(p => p.ConfigurationItems).WithOne()
            .HasForeignKey(d => d.ConfigurationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ConfigurationItem_Configuration");

        builder.HasMany(p => p.Constraints).WithOne()
            .HasForeignKey(d => d.ConfigurationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Constraint_Configuration");

        SampleData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SampleData(EntityTypeBuilder<Configuration> builder)
    {
        // Load test data here
    }
}