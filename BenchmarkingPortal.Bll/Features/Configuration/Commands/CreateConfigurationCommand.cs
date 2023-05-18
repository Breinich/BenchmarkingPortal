using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Commands;

public class CreateConfigurationCommand : IRequest<ConfigurationHeader>
{
    public List<(Scope, string, string)>? Configurations { get; set; }
    public List<(string, string)>? Constraints { get; set; }
}