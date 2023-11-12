using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries
{
    public class GetExecutableToolNameByIdQuery : IRequest<string?>
    {
        public int Id { get; init;}
    }
}
