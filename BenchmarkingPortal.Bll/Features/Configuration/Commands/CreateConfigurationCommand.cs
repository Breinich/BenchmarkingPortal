using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Commands;

/// <summary>
/// Command to create a new configuration
/// </summary>
public class CreateConfigurationCommand : IRequest<ConfigurationHeader>
{
    public required List<(Scope, string, string)> Configurations { get; init; }
    public List<string>? Constraints { get; init; }
    public required string BenchmarkName { get; init; }
    public int Ram { get; init; }
    public int Cpu { get; init; }
    public int TimeLimit { get; set; }
    public int HardTimeLimit { get; set; }
    public int ExecutableId { get; init; }
    public required string SetFilePath { get; init; }
    public required string PropertyFilePath { get; init; }
    public required string InvokerName { get; init; }
}