using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.CommandHandlers;

/// <summary>
/// Command handler for the <see cref="CreateComputerGroupCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class CreateComputerGroupCommandHandler(BenchmarkingDbContext context)
    : IRequestHandler<CreateComputerGroupCommand, ComputerGroupHeader>
{
    public async Task<ComputerGroupHeader> Handle(CreateComputerGroupCommand request,
        CancellationToken cancellationToken)
    {
        var computerGroup = new Dal.Entities.ComputerGroup();

        if (request.Description != null) computerGroup.Description = request.Description;
        if (request.Name != null) computerGroup.Name = request.Name;
        if (request.Hostname != null) computerGroup.Hostname = request.Hostname;

        await context.ComputerGroups.AddAsync(computerGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new ComputerGroupHeader
        {
            Id = computerGroup.Id,
            Description = request.Description
        };
    }
}