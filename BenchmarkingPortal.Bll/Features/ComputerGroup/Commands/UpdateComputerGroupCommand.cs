using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

public class UpdateComputerGroupCommand : IRequest<ComputerGroupHeader>
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string InvokerName { get; init; } = null!;
}