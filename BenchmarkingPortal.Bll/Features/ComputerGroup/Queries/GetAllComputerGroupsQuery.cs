using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;

/// <summary>
/// Query to get all computer groups
/// </summary>
public class GetAllComputerGroupsQuery : IRequest<IEnumerable<ComputerGroupHeader>> { }