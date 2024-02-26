using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

/// <summary>
/// Command for deleting a computer group
/// </summary>
public class DeleteComputerGroupCommand : IRequest
{
    public int Id { get; init; }
    public string InvokerName { get; init; } = null!;
}