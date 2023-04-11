using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

public class UpdateBenchmarkCommand : IRequest<BenchmarkHeader>
{
    public int Id { get; set; }

    public int Priority { get; set; }

    public Status Status { get; set; }
}