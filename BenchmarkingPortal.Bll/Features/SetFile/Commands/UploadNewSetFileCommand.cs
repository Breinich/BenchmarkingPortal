using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Commands;

public class UploadNewSetFileCommand : IRequest<SetFileHeader>
{
    public string Name { get; init; } = null!;
    public string Path { get; init; } = null!;
    public string? Version { get; set; }
    public DateTime UploadedDate { get; init; }
    public string InvokerName { get; init; } = null!;
}