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
public class DeleteConfigurationCommandHandler(BenchmarkingDbContext context)
    : IRequestHandler<DeleteConfigurationCommand>
{
    
    /// <summary>
    /// Handles the <see cref="DeleteConfigurationCommand"/>
    /// </summary>
    /// <param name="request"> The configuration to delete </param>
    /// <param name="cancellationToken"> The token to monitor for cancellation requests </param>
    /// <exception cref="ArgumentException"> Thrown when the configuration is not found or in use </exception>
    public async Task Handle(DeleteConfigurationCommand request, CancellationToken cancellationToken)
    {
        var configuration = await context.Configurations
            .Where(c => c.Id == request.Id).Include(c => c.ConfigurationItems)
            .Include(c => c.Constraints).FirstOrDefaultAsync(cancellationToken);

        if (configuration is null)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Configuration>.ObjectNotFound);
        
        var benchmark = await context.Benchmarks
            .Where(b => b.ConfigurationId == configuration.Id).FirstOrDefaultAsync(cancellationToken);
        if (benchmark != null)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Configuration>.InUse);
        
        foreach (var item in configuration.ConfigurationItems)
        {
            context.Remove(item);
        }

        foreach (var constraint in configuration.Constraints)
        {
            context.Remove(constraint);
        }
        
        File.Delete(configuration.XmlFilePath);
        
        context.Configurations.Remove(configuration);
        await context.SaveChangesAsync(cancellationToken);
    }
}