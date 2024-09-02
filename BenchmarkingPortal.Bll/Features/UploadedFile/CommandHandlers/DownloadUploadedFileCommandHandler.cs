using BenchmarkingPortal.Bll.Exceptions;
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
public class DownloadUploadedFileCommandHandler(
    BenchmarkingDbContext context,
    PathConfigs pathConfigs)
    : IRequestHandler<DownloadUploadedFileCommand, (Stream, string, string)>
{
    public async Task<(Stream, string, string)> Handle(DownloadUploadedFileCommand request, CancellationToken cancellationToken)
    {
        var storePath = "";
        var extension = Path.GetExtension(request.FileId);
        switch (extension)
        {
            case ".zip":
                var found = false;
                var sourceSet = await context.SourceSets.Where(s => s.Path == request.FileId)
                    .Select(s => new SourceSetHeader(s)).FirstOrDefaultAsync(cancellationToken);
                if (sourceSet != null)
                {
                    found = true;
                    storePath = Path.Join(pathConfigs.WorkingDir, sourceSet.UserName, pathConfigs.SourceSetDir);
                }
                else
                {
                    var exe = await context.Executables.Where(e => e.Path == request.FileId)
                        .Select(e => new ExecutableHeader(e)).FirstOrDefaultAsync(cancellationToken);
                    if (exe != null)
                    {
                        found = true;
                        storePath = Path.Join(pathConfigs.WorkingDir, exe.UserName, pathConfigs.ExecutableDir);
                    }
                }
                
                if (!found)
                    throw new ArgumentException("File not found");
                break;
            case ".set":
                var setFile = await context.SetFiles.Where(s => s.Path == request.FileId)
                    .Select(s => new SetFileHeader(s)).FirstOrDefaultAsync(cancellationToken);
                if (setFile == null)
                    throw new ArgumentException("File not found");
                var sourceRoot = await context.SourceSets.Where(s => s.Id == setFile.SourceSetId)
                    .Select(s => s.Name).FirstOrDefaultAsync(cancellationToken) ?? 
                                 throw new ArgumentException(ExceptionMessage<Dal.Entities.SourceSet>.ObjectNotFound);
                storePath = Path.Join(pathConfigs.WorkingDir, setFile.UserName, pathConfigs.SourceSetDir, sourceRoot);
                break;
            default:
                throw new ArgumentException("File not found");
        }
        
        var store = new CustomTusDiskStore(storePath);
        var file = await store.GetFileAsync(request.FileId, cancellationToken);

        var fileStream = await file.GetContentAsync(cancellationToken);
        var metadata = await file.GetMetadataAsync(cancellationToken);
        
        return (fileStream, TusUtil.GetContentTypeOrDefault(metadata, extension.TrimStart('.')), 
            TusUtil.GetContentNameOrDefault(metadata, request.FileId));
    }
}