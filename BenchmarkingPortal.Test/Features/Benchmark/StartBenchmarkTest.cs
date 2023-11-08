using BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal.Entities;
using Castle.Core.Configuration;

namespace BenchmarkingPortal.Test.Features.Benchmark;

public class StartBenchmarkTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly Mock<Configuration> _configurationFixture;

    public StartBenchmarkTest(DatabaseFixture fixture, Mock<Configuration> configuration)
    {
        _fixture = fixture;
        _configurationFixture = configuration;
    }

    public void Dispose()
    {
    }

    [Fact]
    public async Task TestSimpleStart()
    {
        throw new NotImplementedException();
    }
}