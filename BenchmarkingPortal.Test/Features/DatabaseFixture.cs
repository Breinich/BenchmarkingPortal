using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Test.Features;

public class DatabaseFixture : IAsyncLifetime
{
    public DatabaseFixture()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BenchmarkingDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=BenchmarkingPortalTestDb;Trusted_Connection=True;MultipleActiveResultSets=true",
            x => x.MigrationsAssembly("BenchmarkingPortal.Migrations.Test"));

        Context = new BenchmarkingDbContext(optionsBuilder.Options);

        UserManager = new Mock<UserManager<User>>();
    }

    public BenchmarkingDbContext Context { get; }
    public Mock<UserManager<User>> UserManager { get; init; }

    public async Task InitializeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.Database.EnsureCreatedAsync();

        SeedDatabase();
    }

    public async Task DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
    }

    /// <summary>
    ///     Seeds the database with test data
    /// </summary>
    private void SeedDatabase()
    {
        
    }
}