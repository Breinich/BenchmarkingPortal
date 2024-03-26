using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Queries;

/// <summary>
/// Query a configuration by its ID
/// </summary>
public class GetConfigurationByIdQuery : IRequest<ConfigurationHeader?>
{
    public int Id { get; init; }
    public bool IncludeItems { get; init; } = true;
}