using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.QueryHandlers;

public class GetAllPropertyFilesQueryHandler(BenchmarkingDbContext context) : IRequestHandler<GetAllPropertyFilesQuery, IEnumerable<PropertyFileHeader>>
{
    public async Task<IEnumerable<PropertyFileHeader>> Handle(GetAllPropertyFilesQuery request, CancellationToken cancellationToken)
    {
        return await context.PropertyFiles.Select(p => new  PropertyFileHeader(p)).ToListAsync(cancellationToken);
    }
}
