using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Queries;

public class GetAllSourceSetsQuery : IRequest<IEnumerable<SourceSetHeader>>
{
}