using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Worker.Queries;

public class GetAllWorkersQuery : IRequest<IEnumerable<WorkerHeader>> { }