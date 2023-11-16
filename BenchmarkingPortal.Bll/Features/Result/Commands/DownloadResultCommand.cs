using MediatR;

namespace BenchmarkingPortal.Bll.Features.Result.Commands;

public class DownloadResultCommand : IRequest<(FileStream, string, string)>
{
    public string Path { get; init; } = null!;
}