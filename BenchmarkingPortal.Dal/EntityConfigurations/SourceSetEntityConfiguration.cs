using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class SourceSetEntityConfiguration : IEntityTypeConfiguration<SourceSet>
{
    public void Configure(EntityTypeBuilder<SourceSet> builder)
    {
        builder.ToTable("SourceSets");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Root).HasMaxLength(50).IsRequired();

        builder.HasOne(s => s.User).WithMany(u => u.SourceSets)
            .HasForeignKey(s => s.UserName)
            .HasPrincipalKey(u => u.UserName)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_SourceSet_User");

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<SourceSet> builder)
    {

    }
}
