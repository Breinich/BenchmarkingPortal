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
    public async Task Handle(DeleteConfigurationCommand request, CancellationToken cancellationToken)
    {
        var configuration = await context.Configurations
            .Where(c => c.Id == request.Id).Include(c => c.ConfigurationItems)
            .Include(c => c.Constraints).FirstOrDefaultAsync(cancellationToken);

        if (configuration is null)
            throw new ArgumentException(ExceptionMessage<Dal.Entities.Configuration>.ObjectNotFound);
        
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