using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

public class DeleteComputerGroupCommand : IRequest
{
    public int Id { get; init; }
    public string InvokerName { get; init; } = null!;
}