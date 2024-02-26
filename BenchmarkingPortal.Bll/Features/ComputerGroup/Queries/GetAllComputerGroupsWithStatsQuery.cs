using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;

/// <summary>
/// Query to get all computer groups with statistic data
/// </summary>
public class GetAllComputerGroupsWithStatsQuery : IRequest<IEnumerable<ComputerGroupHeader>> { }