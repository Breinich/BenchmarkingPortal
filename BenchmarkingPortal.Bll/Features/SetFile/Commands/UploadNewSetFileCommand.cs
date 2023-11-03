using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Commands;

public class UploadNewSetFileCommand : IRequest<SetFileHeader>
{
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? Version { get; set; }
    public DateTime UploadedDate { get; set; }
    public string InvokerName { get; set; } = null!;
}