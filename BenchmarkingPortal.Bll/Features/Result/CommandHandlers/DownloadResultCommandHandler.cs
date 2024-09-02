using System.IO.Compression;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Result.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ArgumentException = System.ArgumentException;

namespace BenchmarkingPortal.Bll.Features.Result.CommandHandlers;

/// <summary>
/// Handler for <see cref="DownloadResultCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class DownloadResultCommandHandler(BenchmarkingDbContext dbContext)
    : IRequestHandler<DownloadResultCommand, (FileStream, string, string)>
{
    public async Task<(FileStream, string, string)> Handle(DownloadResultCommand request, CancellationToken cancellationToken)
    {
        _ = await dbContext.Benchmarks.Where(x => x.ResultPath == request.Path)
            .Select(b => new BenchmarkHeader(b)).FirstOrDefaultAsync(cancellationToken) 
                        ?? throw new ArgumentException(ExceptionMessage<Dal.Entities.Benchmark>.ObjectNotFound);

        var filePath = request.Path + ".zip";
            
        if (!File.Exists(filePath))
        {
            await Task.Run(() => ZipFile.CreateFromDirectory(request.Path, filePath, 
                    CompressionLevel.Optimal, false), cancellationToken);
        }
            
        return (File.OpenRead(filePath), "application/zip", Path.GetFileName(filePath));
    }
}