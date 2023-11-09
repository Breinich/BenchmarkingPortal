using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Commands;

public class UploadNewExecutableCommand : IRequest<ExecutableHeader>
{
    public string OwnerTool { get; init; } = null!;
    public string ToolVersion { get; init; } = null!;
    public string Path { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Version { get; set; }
    public DateTime UploadedDate { get; init; }
    public string InvokerName { get; init; } = null!;
}