using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Commands;

public class UploadNewExecutableCommand : IRequest<ExecutableHeader>
{
    public string OwnerTool { get; set; } = null!;
    public string ToolVersion { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public DateTime UploadedDate { get; set; }
    public string InvokerName { get; set; } = null!;
}