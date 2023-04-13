using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SourceSet.QueryHandlers;

public class GetAllSourceSetsQueryHandler : IRequestHandler<GetAllSourceSetsQuery, IEnumerable<SourceSetHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllSourceSetsQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<SourceSetHeader>> Handle(GetAllSourceSetsQuery request,
        CancellationToken cancellationToken) =>
        await _context.SourceSets.Select(s => new SourceSetHeader(s)).ToListAsync(cancellationToken);
}