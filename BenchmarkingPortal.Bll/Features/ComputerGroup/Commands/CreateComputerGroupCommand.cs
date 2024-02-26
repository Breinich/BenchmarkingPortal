using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

/// <summary>
/// Command for creating a new computer group.
/// </summary>
public class CreateComputerGroupCommand : IRequest<ComputerGroupHeader>
{
    public string? Description { get; init; }
    public string? Name { get; init; }
    public string? Hostname { get; init; }
}