using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class CpuModelEntityConfiguration : IEntityTypeConfiguration<CpuModel>
{
    public void Configure(EntityTypeBuilder<CpuModel> builder)
    {
        builder.ToTable("CpuModels");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Value).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<CpuModel> builder) { }
}
