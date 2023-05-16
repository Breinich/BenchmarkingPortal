using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ConstraintEntityConfiguration : IEntityTypeConfiguration<Constraint>
{
    public void Configure(EntityTypeBuilder<Constraint> builder)
    {
        builder.ToTable("Constraints");

        builder.HasKey(e => new { e.Premise, e.Consequence, e.ConfigurationId });

        builder.Property(e => e.Premise).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Consequence).HasMaxLength(50).IsRequired();

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<Constraint> builder)
    {
        // Load test data here
    }
}