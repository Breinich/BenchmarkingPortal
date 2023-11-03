using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SetFile.CommandHandlers;

public class UploadNewSetFileCommandHandler : IRequestHandler<UploadNewSetFileCommand, SetFileHeader>
{
    private readonly BenchmarkingDbContext _context;

    public UploadNewSetFileCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<SetFileHeader> Handle(UploadNewSetFileCommand request, CancellationToken cancellationToken)
    {
        request.Version ??= "1.0";

        var setFile = new Dal.Entities.SetFile
        {
            Name = request.Name,
            Path = request.Path,
            UserName = request.InvokerName,
            UploadedDate = request.UploadedDate,
            Version = request.Version
        };

        await _context.SetFiles.AddAsync(setFile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new SetFileHeader(setFile);
    }
}