using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class GetExecutableByIdQuery : IRequest<ExecutableHeader?>
{
    public int Id { get; init; }
}