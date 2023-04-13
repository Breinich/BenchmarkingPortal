using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Commands;

public class DeleteSourceSetCommand : IRequest
{
    public int SourceSetId { get; set; }
    public int UserId { get; set; }
}