using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Commands;

/// <summary>
/// Command to create a new configuration
/// </summary>
public class CreateConfigurationCommand : IRequest<ConfigurationHeader>
{
    public List<(Scope, string, string)>? Configurations { get; init; }
    public List<(string, string)>? Constraints { get; init; }
}