using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.Queries
{
    public class GetAllPropertyFilesQuery : IRequest<IEnumerable<PropertyFileHeader>> {}
}
