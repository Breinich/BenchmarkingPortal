using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class GetAllExecutableNamesQuery : IRequest<IEnumerable<string>>
{ }