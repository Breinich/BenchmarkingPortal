using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenchmarkingPortal.Dal.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(e => e.Subscription).IsRequired();
        builder.Property(e => e.GitHubUserName).HasMaxLength(50);

        SampleData(builder);
    }

    private void SampleData(EntityTypeBuilder<User> builder)
    {
        // Load test data here
    }
}