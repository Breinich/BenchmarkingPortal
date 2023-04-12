using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;

public class CreateComputerGroupCommand : IRequest<ComputerGroupHeader>
{
    public string? Description { get; set; }
}