using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

public class UpdateComputerGroupCommand : IRequest<ComputerGroupHeader>
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string InvokerName { get; set; } = null!;
}