using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Commands;

public class DeleteSourceSetCommand : IRequest
{
    public int SourceSetId { get; set; }
    public string InvokerName { get; set; } = null!;
    public string FileId { get; set; } = null!;
}