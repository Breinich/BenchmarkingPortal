using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ExecutableEntityConfiguration : IEntityTypeConfiguration<Executable>
{
    public void Configure(EntityTypeBuilder<Executable> builder)
    {
        builder.ToTable("Executables");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.OwnerTool).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Path).HasMaxLength(50).IsRequired();
        builder.Property(e => e.ToolVersion).HasMaxLength(50).IsRequired();
        builder.Property(e => e.UploadedDate).IsRequired();
        builder.Property(e => e.Version).HasMaxLength(50).IsRequired();

        builder.HasOne(d => d.User).WithMany(p => p.Executables)
            .HasForeignKey(d => d.UserName)
            .HasPrincipalKey(d => d.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Executable_User");

        SampleData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SampleData(EntityTypeBuilder<Executable> builder)
    {
        // Load test data here
    }
}