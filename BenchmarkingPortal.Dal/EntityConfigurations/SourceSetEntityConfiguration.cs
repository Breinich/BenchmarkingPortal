using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class SourceSetEntityConfiguration : IEntityTypeConfiguration<SourceSet>
{
    public void Configure(EntityTypeBuilder<SourceSet> builder)
    {
        builder.ToTable("SourceSets");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Path).HasMaxLength(50).IsRequired();
        builder.Property(e => e.UploadedDate).HasColumnType("datetime").IsRequired();
        builder.Property(e => e.Version).HasMaxLength(50).IsRequired();

        builder.HasOne(d => d.User).WithMany(p => p.SourceSets)
            .HasForeignKey(d => d.UserName)
            .HasPrincipalKey(d => d.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_SourceSet_User");

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<SourceSet> builder)
    {
        // Load test data here
    }
}