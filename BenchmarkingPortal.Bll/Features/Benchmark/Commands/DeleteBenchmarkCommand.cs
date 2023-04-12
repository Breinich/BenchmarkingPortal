using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

public class DeleteBenchmarkCommand : IRequest
{
    public int BenchmarkId { get; set; }

    public int UserId { get; set; }
}