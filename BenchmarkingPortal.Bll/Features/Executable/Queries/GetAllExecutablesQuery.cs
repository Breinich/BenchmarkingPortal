using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class GetAllExecutablesQuery : IRequest<IEnumerable<ExecutableHeader>>
{
}