using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Queries;

public class GetSetFileByPathQuery : IRequest<SetFileHeader?>
{
    public string FileId { get; init; } = null!;
}