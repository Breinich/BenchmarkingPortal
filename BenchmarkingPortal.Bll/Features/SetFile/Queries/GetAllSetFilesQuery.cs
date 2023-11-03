using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.Queries;

public class GetAllSetFilesQuery : IRequest<IEnumerable<SetFileHeader>>
{
}