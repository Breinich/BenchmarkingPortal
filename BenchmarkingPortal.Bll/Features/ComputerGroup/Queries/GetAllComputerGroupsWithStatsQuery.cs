using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;

public class GetAllComputerGroupsWithStatsQuery : IRequest<IEnumerable<ComputerGroupHeader>> { }