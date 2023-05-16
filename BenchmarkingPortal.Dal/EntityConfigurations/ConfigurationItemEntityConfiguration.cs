using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ConfigurationItemEntityConfiguration : IEntityTypeConfiguration<ConfigurationItem>
{
    public void Configure(EntityTypeBuilder<ConfigurationItem> builder)
    {
        builder.ToTable("ConfigurationItems");

        builder.HasKey(e => new { e.Key, e.Value, e.Scope, e.ConfigurationId });

        builder.Property(e => e.Key).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Value).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Scope).HasConversion(s => s.ToString(),
            v => (Scope)Enum.Parse(typeof(Scope), v)).IsRequired();

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<ConfigurationItem> builder)
    { 
        // Load test data here
    }
}