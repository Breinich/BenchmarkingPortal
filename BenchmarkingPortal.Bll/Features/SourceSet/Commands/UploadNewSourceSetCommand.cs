using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.Commands;

public class UploadNewSourceSetCommand : IRequest<SourceSetHeader>
{
    public string Name { get; init; } = null!;
    public string Path { get; init; } = null!;
    public DateTime UploadedDate { get; init; }
    public string InvokerName { get; init; } = null!;
}