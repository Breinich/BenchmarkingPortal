using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Queries;

public class GetConfigurationByIdQuery : IRequest<ConfigurationHeader?>
{
    public int Id { get; set; }
}