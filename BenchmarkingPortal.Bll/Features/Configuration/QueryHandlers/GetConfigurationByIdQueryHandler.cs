using System.Diagnostics;
using BenchmarkingPortal.Bll.Features.Configuration.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Configuration.QueryHandlers;

/// <summary>
/// The handler for <see cref="GetConfigurationByIdQuery"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class GetConfigurationByIdQueryHandler : IRequestHandler<GetConfigurationByIdQuery, ConfigurationHeader?>
{
    private readonly BenchmarkingDbContext _context;
    
    public GetConfigurationByIdQueryHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public async Task<ConfigurationHeader?> Handle(GetConfigurationByIdQuery request, CancellationToken cancellationToken)
    {
        return request.IncludeItems switch
        {
            true => await _context.Configurations.Where(c => c.Id == request.Id)
                .Include(c => c.ConfigurationItems)
                .Include(c => c.Constraints)
                .Select(c => new ConfigurationHeader(c))
                .FirstOrDefaultAsync(cancellationToken),
            false => await _context.Configurations.Where(c => c.Id == request.Id)
                .Select(c => new ConfigurationHeader(c))
                .FirstOrDefaultAsync(cancellationToken)
        };
    }
}