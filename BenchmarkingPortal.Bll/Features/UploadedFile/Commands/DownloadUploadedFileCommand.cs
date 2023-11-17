using MediatR;

namespace BenchmarkingPortal.Bll.Features.UploadedFile.Commands;

public class DownloadUploadedFileCommand : IRequest<(Stream, string, string)>
{
    public string FileId { get; init; } = null!;
}