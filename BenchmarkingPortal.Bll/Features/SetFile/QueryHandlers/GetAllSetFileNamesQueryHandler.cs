using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace BenchmarkingPortal.Bll.Features.SetFile.QueryHandlers;

public class GetAllSetFileNamesQueryHandler : IRequestHandler<GetAllSetFileNamesQuery, IEnumerable<String>>
{
    private readonly IConfiguration _configuration;
    
    public GetAllSetFileNamesQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task<IEnumerable<string>> Handle(GetAllSetFileNamesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Directory.GetFiles((_configuration["Storage:SV-Benchmarks"] ??
                            throw new ApplicationException("SV-Benchmarks path is not configured."))
                           + Path.DirectorySeparatorChar + "c", "*.set", SearchOption.TopDirectoryOnly)
            .Select(s => s));
    }
}