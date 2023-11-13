using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class GetExecutableNameByIdQuery : IRequest<string?>
{
    public int Id { get; init; }
}