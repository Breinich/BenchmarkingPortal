using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Commands;

public class DeleteSetFileCommand : IRequest
{
    public int SetFileId { get; set; }
    public string InvokerName { get; set; } = null!;
    public string FileId { get; set; } = null!;
}