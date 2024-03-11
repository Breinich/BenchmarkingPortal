using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.CpuModel.Queries;

/// <summary>
/// Query all CPU models
/// </summary>
public class GetAllCpuModelsQuery : IRequest<IEnumerable<CpuModelHeader>> { }