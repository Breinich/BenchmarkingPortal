using MediatR;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.Queries;

public class GetAllPropertyFileNamesQuery : IRequest<IEnumerable<string>>
{ }