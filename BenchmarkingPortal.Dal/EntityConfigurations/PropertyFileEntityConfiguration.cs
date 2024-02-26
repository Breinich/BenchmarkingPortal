using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class PropertyFileEntityConfiguration : IEntityTypeConfiguration<PropertyFile>
{
    public void Configure(EntityTypeBuilder<PropertyFile> builder)
    {
        builder.ToTable("PropertyFiles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Path).HasMaxLength(50).IsRequired();
        builder.Property(e => e.UploadedDate).IsRequired();
        builder.Property(e => e.Version).HasMaxLength(50).IsRequired();

        builder.HasOne(d => d.User).WithMany(p => p.PropertyFiles)
            .HasForeignKey(d => d.UserName)
            .HasPrincipalKey(d => d.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PropertyFile_User");

        builder.HasOne(s => s.SourceSet).WithMany(s => s.PropertyFiles)
            .HasForeignKey(s => s.SourceSetId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_PropertyFile_SourceSet");

        builder.HasIndex(e => new { e.Name, e.Version }).IsUnique();

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<PropertyFile> builder) { }
}
