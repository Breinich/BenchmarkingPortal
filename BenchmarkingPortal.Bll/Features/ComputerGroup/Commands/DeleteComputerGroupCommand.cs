using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

public class DeleteComputerGroupCommand : IRequest
{
    public int Id { get; set; }
    public string InvokerName { get; set; } = null!;
}