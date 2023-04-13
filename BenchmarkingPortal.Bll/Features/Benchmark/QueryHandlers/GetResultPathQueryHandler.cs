using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BenchmarkingPortal.Bll.Features.Benchmark.QueryHandlers;

public class GetResultPathQueryHandler : IRequestHandler<GetResultPathQuery, string>
{
    private readonly BenchmarkingDbContext _context;

    public GetResultPathQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetResultPathQuery request, CancellationToken cancellationToken)
    {
        var benchmark = await _context.Benchmarks.Where(b => b.Id == request.BenchmarkId)
                            .Select(b => b).FirstOrDefaultAsync(cancellationToken) ??
                        throw new ArgumentException(new ExceptionMessage<Dal.Entities.Benchmark>().ObjectNotFound);


        if(benchmark.Status != Status.Finished)
            throw new ArgumentException(
                "The benchmark hasn't been finished yet.");
        

        return benchmark.Result ?? throw new ApplicationException("Somehow this finished benchmark doesn't store the results' path.");
    }
}