using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Commands;

public class DeleteSourceSetCommand : IRequest
{
    public int Id { get; init; }
    public string FileId { get; init; } = null!;
    public string InvokerName { get; init; } = null!;
}