﻿using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class ComputerGroupEntityConfiguration : IEntityTypeConfiguration<ComputerGroup>
{
    public void Configure(EntityTypeBuilder<ComputerGroup> builder)
    {
        builder.ToTable("ComputerGroups");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Description).HasMaxLength(50);
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(cg => cg.Hostname).HasMaxLength(50).IsRequired();

        builder.HasMany(p => p.Workers).WithOne()
            .HasForeignKey(d => d.ComputerGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Worker_ComputerGroup");

        SampleData(builder);
    }

    // ReSharper disable once UnusedParameter.Local
    private void SampleData(EntityTypeBuilder<ComputerGroup> builder)
    {
        // Load test data here
    }
}