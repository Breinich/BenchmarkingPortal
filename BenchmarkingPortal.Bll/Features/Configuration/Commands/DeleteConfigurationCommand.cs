using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Commands;

public class DeleteConfigurationCommand : IRequest
{
    public int Id { get; init; }
}