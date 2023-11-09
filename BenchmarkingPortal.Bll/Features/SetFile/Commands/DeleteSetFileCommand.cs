using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Commands;

public class DeleteSetFileCommand : IRequest
{
    public int SetFileId { get; init; }
    public string InvokerName { get; init; } = null!;
    public string FileId { get; init; } = null!;
}