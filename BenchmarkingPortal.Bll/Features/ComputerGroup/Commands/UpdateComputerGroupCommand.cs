using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

/// <summary>
/// Command for updating a computer group
/// </summary>
public class UpdateComputerGroupCommand : IRequest<ComputerGroupHeader>
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string? Name { get; init; }
    public string? Hostname { get; init; }
    public string InvokerName { get; init; } = null!;
}