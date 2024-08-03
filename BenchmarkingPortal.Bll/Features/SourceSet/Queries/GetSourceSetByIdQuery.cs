using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Queries;

public class GetSourceSetByIdQuery : IRequest<SourceSetHeader?>
{
    public int Id { get; init; }
}