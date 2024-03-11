using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.Configuration.CommandHandlers;

/// <summary>
/// The handler for the <see cref="DeleteConfigurationCommand"/>.
/// </summary>
// ReSharper disable once UnusedType.Global
public class DeleteConfigurationCommandHandler : IRequestHandler<DeleteConfigurationCommand>
{
    private readonly BenchmarkingDbContext _context;
    
    public DeleteConfigurationCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }
    
    public async Task Handle(DeleteConfigurationCommand request, CancellationToken cancellationToken)
    {
        var configuration = await _context.Configurations
            .Where(c => c.Id == request.Id).Include(c => c.ConfigurationItems)
            .Include(c => c.Constraints).FirstOrDefaultAsync(cancellationToken);

        if (configuration is null)
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.Configuration>().ObjectNotFound);
        
        foreach (var item in configuration.ConfigurationItems)
        {
            _context.Remove(item);
        }

        foreach (var constraint in configuration.Constraints)
        {
            _context.Remove(constraint);
        }
        
        _context.Configurations.Remove(configuration);
        await _context.SaveChangesAsync(cancellationToken);
    }
}