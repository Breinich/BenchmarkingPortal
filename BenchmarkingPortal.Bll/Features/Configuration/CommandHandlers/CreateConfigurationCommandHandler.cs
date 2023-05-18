using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.CommandHandlers;

public class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand, ConfigurationHeader>
{
    private readonly BenchmarkingDbContext _context;

    public CreateConfigurationCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<ConfigurationHeader> Handle(CreateConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // config check needed based on the constraints
        // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        
        var config = new Dal.Entities.Configuration();

        await _context.Configurations.AddAsync(config, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (request.Configurations != null)
            foreach (var configItem in request.Configurations)
                await _context.ConfigurationItems.AddAsync(new ConfigurationItem
                {
                    Key = configItem.Item2,
                    Value = configItem.Item3,
                    Scope = configItem.Item1,
                    ConfigurationId = config.Id
                }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        if (request.Constraints != null)
            foreach (var constraint in request.Constraints)
                await _context.Constraints.AddAsync(new Constraint
                {
                    Premise = constraint.Item1,
                    Consequence = constraint.Item2,
                    ConfigurationId = config.Id
                }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ConfigurationHeader { Id = config.Id };
    }
}