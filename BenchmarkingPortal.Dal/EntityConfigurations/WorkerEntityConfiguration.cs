﻿using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class WorkerEntityConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("Worker");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.AddedDate).HasColumnType("datetime").IsRequired();
        builder.Property(e => e.Address).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Password).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Username).HasMaxLength(50).IsRequired();

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<Worker> builder)
    {
        // Load test data here
    }
}