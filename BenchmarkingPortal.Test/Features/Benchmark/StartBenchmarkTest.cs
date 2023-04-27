using BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;

namespace BenchmarkingPortal.Test.Features.Benchmark;

public class StartBenchmarkTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;    

    public StartBenchmarkTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    public void Dispose()
    {
    }

    [Fact]
    public async Task TestSimpleStart()
    {
        var handler = new StartBenchmarkCommandHandler(_fixture.Context);
        
        await handler.Handle(new StartBenchmarkCommand()
        {
            Name = "b2",
            Priority = 1,
            Ram = 8,
            Cpu = 4,
            TimeLimit = 900,
            HardTimeLimit = 960,
            ExecutableId = 1,
            SourceSetId = 1,
            SetFilePath = "C:\\Users\\User\\Desktop\\test.txt",
            PropertyFilePath = "C:\\Users\\User\\Desktop\\test.txt",
            ConfigurationId = 1,
            UserId = 1,

        }, CancellationToken.None);

        Assert.Equal(1, _fixture.Context.Benchmarks
            .Where(b => b.Name.Equals("b2")).Select(b => b.Id).Count());
    }
}