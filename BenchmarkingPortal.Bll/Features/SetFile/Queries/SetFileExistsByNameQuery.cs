using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Queries;

public class SetFileExistsByNameQuery : IRequest<bool>
{
    public string FileName { get; init; } = null!;
}