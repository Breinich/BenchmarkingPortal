using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

public class UpdateBenchmarkCommand : IRequest<BenchmarkHeader>
{
    public int Id { get; init; }

    public Priority Priority { get; init; }

    public Status Status { get; init; }

    public string InvokerName { get; init; } = null!;
}