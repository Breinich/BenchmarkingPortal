using BenchmarkingPortal.Bll.Features.UploadedFile.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.UploadedFile.CommandHandlers;

/// <summary>
/// Handler for <see cref="DownloadUploadedFileCommand"/>
/// </summary>
public class DownloadUploadedFileCommandHandler : IRequestHandler<DownloadUploadedFileCommand, (Stream, string, string)>
{
    private readonly BenchmarkingDbContext _context;
    private readonly IMediator _mediator;
    private readonly string _workDir;
    
    public DownloadUploadedFileCommandHandler(BenchmarkingDbContext context, IMediator mediator, PathConfigs pathConfigs)
    {
        _context = context;
        _mediator = mediator;
        _workDir = pathConfigs.WorkingDir;
    }
    
    public async Task<(Stream, string, string)> Handle(DownloadUploadedFileCommand request, CancellationToken cancellationToken)
    {
        var storePath = "";
        var extension = Path.GetExtension(request.FileId);
        switch (extension)
        {
            case ".zip":
                var exe = await _context.Executables.Where(e => e.Path == request.FileId)
                    .Select(e => new ExecutableHeader(e)).FirstOrDefaultAsync(cancellationToken);
                storePath = Path.Join(_workDir, (exe ?? throw new ApplicationException($"File with id {request.FileId} was not found."))
                    .UserName, "tools");
                break;
        }
        
        var store = new CustomTusDiskStore(storePath, _mediator);
        var file = await store.GetFileAsync(request.FileId, cancellationToken);

        var fileStream = await file.GetContentAsync(cancellationToken);
        var metadata = await file.GetMetadataAsync(cancellationToken);
        
        return (fileStream, TusUtil.GetContentTypeOrDefault(metadata, extension.TrimStart('.')), 
            TusUtil.GetContentNameOrDefault(metadata, request.FileId));
    }
}