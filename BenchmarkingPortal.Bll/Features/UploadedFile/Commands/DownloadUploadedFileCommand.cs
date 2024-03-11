using MediatR;

namespace BenchmarkingPortal.Bll.Features.UploadedFile.Commands;

/// <summary>
/// Command to download an uploaded file
/// </summary>
public class DownloadUploadedFileCommand : IRequest<(Stream, string, string)>
{
    public string FileId { get; init; } = null!;
}