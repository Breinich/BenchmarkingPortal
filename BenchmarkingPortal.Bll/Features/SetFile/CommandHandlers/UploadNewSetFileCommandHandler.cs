using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.CommandHandlers;

public class UploadNewSetFileCommandHandler(BenchmarkingDbContext context)
    : IRequestHandler<UploadNewSetFileCommand, SetFileHeader>
{
    public async Task<SetFileHeader> Handle(UploadNewSetFileCommand request, CancellationToken cancellationToken)
    {

        var setFile = new Dal.Entities.SetFile
        {
            Name = request.Name,
            Path = request.Path,
            UserName = request.InvokerName,
            UploadedDate = request.UploadedDate,
            SourceSetId = request.SourceSetId
        };

        await context.SetFiles.AddAsync(setFile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new SetFileHeader(setFile);
    }
}