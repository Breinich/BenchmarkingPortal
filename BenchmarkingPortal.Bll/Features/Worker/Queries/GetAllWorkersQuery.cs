using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Queries;

/// <summary>
/// Defines a request for all workers.
/// </summary>
public class GetAllWorkersQuery : IRequest<IEnumerable<WorkerHeader>>
{
}