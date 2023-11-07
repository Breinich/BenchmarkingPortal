using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.QueryHandlers;

public class GetPropertyFileNamesQueryHandler : IRequestHandler<GetPropertyFileNamesQuery, IEnumerable<string>>
{
    private readonly IConfiguration _configuration;
    
    public GetPropertyFileNamesQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task<IEnumerable<string>> Handle(GetPropertyFileNamesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Directory.GetFiles((_configuration["Storage:SV-Benchmarks"] ??
                            throw new ApplicationException("SV-Benchmarks path is not configured."))
                           + Path.DirectorySeparatorChar + "c" + Path.DirectorySeparatorChar + "properties", "*.prp", SearchOption.TopDirectoryOnly)
            .Select(s => s));
    }
}