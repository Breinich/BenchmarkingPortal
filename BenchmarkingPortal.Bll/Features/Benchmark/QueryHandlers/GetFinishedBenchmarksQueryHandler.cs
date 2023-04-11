using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

public class GetFinishedBenchmarksQueryHandler : IRequestHandler<GetFinishedBenchmarksQuery, IEnumerable<BenchmarkHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetFinishedBenchmarksQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<BenchmarkHeader>> Handle(GetFinishedBenchmarksQuery request, CancellationToken cancellationToken) =>
    
        await _context.Benchmarks.Where(b => b.Status.Equals(Status.Finished)).Select(b => new BenchmarkHeader() 
        {
            Id = b.Id,
            Name = b.Name,
            Priority = b.Priority,
            Status = b.Status,
            Result = b.Result,
            Ram = b.Ram,
            Cpu = b.Cpu,
            TimeLimit = b.TimeLimit,
            HardTimeLimit = b.HardTimeLimit,
            ComputerGroupId = b.ComputerGroupId,
            ExecutableId = b.ExecutableId,
            SourceSetId = b.SourceSetId,
            StartedDate = b.StartedDate,
            ConfigurationId = b.ConfigurationId,
            UserId = b.UserId,
        }).ToListAsync(cancellationToken);
    
}