using MediatR;

namespace BenchmarkingPortal.Bll.Features.Result.Commands;

/// <summary>
/// Download results from the given path
/// </summary>
public class DownloadResultCommand : IRequest<(FileStream, string, string)>
{
    public string Path { get; init; } = null!;
}