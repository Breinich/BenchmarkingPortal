using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Commands;

/// <summary>
/// Command for deleting a configuration
/// </summary>
public class DeleteConfigurationCommand : IRequest
{
    public int Id { get; init; }
}