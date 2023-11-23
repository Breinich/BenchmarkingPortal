using BenchmarkingPortal.Dal.Entities;

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
    public Task TestSimpleStart()
    {
        throw new NotImplementedException("Testing is not implemented yet.");
    }
}