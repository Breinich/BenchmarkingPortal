using BenchmarkingPortal.Bll.Features.Worker.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Worker.QueryHandlers;

/// <summary>
/// Handler for <see cref="GetAllWorkersQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, IEnumerable<WorkerHeader>>
{
    private readonly BenchmarkingDbContext _context;

    public GetAllWorkersQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkerHeader>>
        Handle(GetAllWorkersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Workers.Select(w => new WorkerHeader(w)).ToListAsync(cancellationToken);
    }
}