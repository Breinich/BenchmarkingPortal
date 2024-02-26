using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.CpuModel.Queries;

public class GetAllCpuModelsQuery : IRequest<IEnumerable<CpuModelHeader>>
{
}