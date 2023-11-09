using System.Text;
using MediatR;
using tusdotnet.Models;
using tusdotnet.Parsers;
using tusdotnet.Stores.FileIdProviders;

namespace BenchmarkingPortal.Bll.Tus;

public class CustomGuidProvider : GuidFileIdProvider
{
    private readonly IMediator _mediator;
    
    public CustomGuidProvider(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<string> CreateId(string metadata)
    {
        var parsedMetadata = MetadataParser.ParseAndValidate(MetadataParsingStrategy.AllowEmptyValues, metadata).Metadata;
        return Task.FromResult(parsedMetadata["name"].GetString(Encoding.UTF8));
    }

    public override Task<bool> ValidateId(string fileId)
    {
        return fileId.EndsWith(".zip") || fileId.EndsWith(".set") ? Task.FromResult(true) : Task.FromResult(false);
    }
}