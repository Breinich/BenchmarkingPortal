using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.CommandHandlers;

public class CreateComputerGroupCommandHandler : IRequestHandler<CreateComputerGroupCommand, ComputerGroupHeader>
{
    private readonly BenchmarkingDbContext _context;

    public CreateComputerGroupCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }

    public async Task<ComputerGroupHeader> Handle(CreateComputerGroupCommand request, CancellationToken cancellationToken)
    {
        var computerGroup = new Dal.Entities.ComputerGroup();

        if (request.Description != null)
        {
            computerGroup.Description = request.Description;
        }

        _context.ComputerGroups.Add(computerGroup);
        await _context.SaveChangesAsync(cancellationToken);

        return new ComputerGroupHeader()
        {
            Id = computerGroup.Id,
            Description = request.Description,
        };
    }
}