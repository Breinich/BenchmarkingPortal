using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Test.Features;

public class DatabaseFixture : IAsyncLifetime
{
    public BenchmarkingDbContext Context { get; private set; }
    public Mock<UserManager<User>> UserManager { get; init; }

    public DatabaseFixture()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BenchmarkingDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=BenchmarkingPortalTestDb;Trusted_Connection=True;MultipleActiveResultSets=true",
            x => x.MigrationsAssembly("BenchmarkingPortal.Migrations.Test"));
        
        Context =  new BenchmarkingDbContext(optionsBuilder.Options);
        
        UserManager = new Mock<UserManager<User>>();
    }
    
    public async Task InitializeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.Database.EnsureCreatedAsync();
        
        await SeedDatabase();
    }
    
    /// <summary>
    /// Seeds the database with test data
    /// </summary>
    private async Task SeedDatabase()
    {
        var user = new User
        {
            UserName = "TestUser",
            Email = "test@me.hu",
        };
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        var executable = new Executable
        {
            OwnerTool = "TestTool",
            ToolVersion = "1.0",
            Path = "C:\\TestTool",
            Name = "TestToolAlpha",
            Version = "1.0",
            UploadedDate = DateTime.Now,
            UserId = user.Id,
        };
        await Context.Executables.AddAsync(executable);
        await Context.SaveChangesAsync();
        
        var sourceSet = new SourceSet
        {
            Name = "TestSourceSet",
            Path = "C:\\TestSourceSet",
            Version = "1.0",
            UploadedDate = DateTime.Now,
            UserId = user.Id,
        };
        await Context.SourceSets.AddAsync(sourceSet);
        await Context.SaveChangesAsync();

        var configuration = new Configuration();
        await Context.Configurations.AddAsync(configuration);
        await Context.SaveChangesAsync();

        var computerGroup = new ComputerGroup();
        await Context.ComputerGroups.AddAsync(computerGroup);
        await Context.SaveChangesAsync();

        var worker = new Worker
        {
            Name = "TestWorker",
            Ram = 4,
            Cpu = 2,
            Username = "TestUser",
            Password = "TestPassword",
            Storage = 40,
            Address = "192.168.0.0",
            Port = 8080,
            ComputerGroupId = computerGroup.Id,
            AddedDate = DateTime.Now,
        };
        await Context.Workers.AddAsync(worker);
        await Context.SaveChangesAsync();
        
        await Context.Benchmarks.AddAsync(new Dal.Entities.Benchmark
        {
            Name = "TestBenchmark",
            Priority = 0,
            Status = Status.Running,
            StartedDate = DateTime.Now,
            Ram = 1,
            Cpu = 1,
            TimeLimit = 900,
            HardTimeLimit = 960,
            ComputerGroupId = computerGroup.Id,
            ExecutableId = executable.Id,
            UserId = user.Id,
            SourceSetId = sourceSet.Id,
            SetFilePath = "C:\\TestSourceSet\\test.txt",
            PropertyFilePath = "C:\\TestSourceSet\\test.properties",
            ConfigurationId = configuration.Id,
        });
        await Context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
    }
}