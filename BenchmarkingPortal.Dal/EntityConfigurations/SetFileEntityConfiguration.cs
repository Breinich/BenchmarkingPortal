﻿using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class SetFileEntityConfiguration : IEntityTypeConfiguration<SetFile>
{
    public void Configure(EntityTypeBuilder<SetFile> builder)
    {
        builder.ToTable("SetFiles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Path).HasMaxLength(50).IsRequired();
        builder.Property(e => e.UploadedDate).IsRequired();
        builder.Property(e => e.Version).HasMaxLength(50).IsRequired();

        builder.HasOne(d => d.User).WithMany(p => p.SetFiles)
            .HasForeignKey(d => d.UserName)
            .HasPrincipalKey(d => d.UserName)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_SetFile_User");

        builder.HasOne(s => s.SourceSet).WithMany(s => s.SetFiles)
            .HasForeignKey(s => s.SourceSetId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_SetFile_SourceSet");
        
        builder.HasIndex(e => new {e.Name, e.Version}).IsUnique();

        SampleData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SampleData(EntityTypeBuilder<SetFile> builder)
    {
        // Load test data here
    }
}