using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Benchmark.CommandHandlers;

public class UpdateBenchmarkCommandHandler : IRequestHandler<UpdateBenchmarkCommand, BenchmarkHeader>
{
    private readonly BenchmarkingDbContext _context;

    public UpdateBenchmarkCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<BenchmarkHeader> Handle(UpdateBenchmarkCommand request, CancellationToken cancellationToken)
    {
        var bAsync = await _context.Benchmarks.Where(b => b.Id.Equals(request.Id)).Select(b => new BenchmarkHeader()
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
        }).FirstOrDefaultAsync(cancellationToken);

        if (bAsync != null)
        {
            bAsync.Priority = request.Priority;
            bAsync.Status = request.Status;

            // Ask the scheduler to modify the selected benchmark
            // TODO

            // If succeeded, the modified Benchmark will be written into the DB
            var benchmarkEntity = _context.Benchmarks.FindAsync(request.Id, cancellationToken).Result;
            if (benchmarkEntity != null)
            {
                benchmarkEntity.Priority = request.Priority;
                benchmarkEntity.Status = request.Status;

                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new ApplicationException("Database error, couldn't find the appropriate Benchmark twice.");
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException("request", request.Id,
                "There isn't any benchmark with the given Id.");
        }

        return bAsync;
    }
}