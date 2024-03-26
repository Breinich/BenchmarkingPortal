using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ConstraintEntityConfiguration : IEntityTypeConfiguration<Constraint>
{
    public void Configure(EntityTypeBuilder<Constraint> builder)
    {
        builder.ToTable("Constraints");

        builder.HasKey(e =>  e.Id);

        builder.Property(e => e.Expression).HasMaxLength(255).IsRequired();

        SampleData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SampleData(EntityTypeBuilder<Constraint> builder)
    {
        // Load test data here
    }
}