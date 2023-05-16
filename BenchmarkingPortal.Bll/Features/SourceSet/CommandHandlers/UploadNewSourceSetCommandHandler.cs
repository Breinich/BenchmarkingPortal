using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.SourceSet.CommandHandlers;

public class UploadNewSourceSetCommandHandler : IRequestHandler<UploadNewSourceSetCommand, SourceSetHeader>
{
    private readonly BenchmarkingDbContext _context;

    public UploadNewSourceSetCommandHandler(BenchmarkingDbContext context)
    {
        _context = context;
    }


    public async Task<SourceSetHeader> Handle(UploadNewSourceSetCommand request, CancellationToken cancellationToken)
    {
        request.Version ??= "1.0";

        var sourceSet = new Dal.Entities.SourceSet()
        {
            Name = request.Name,
            Path = request.Path,
            UserName = request.InvokerName,
            UploadedDate = request.UploadedDate,
            Version = request.Version,
        };

        await _context.SourceSets.AddAsync(sourceSet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new SourceSetHeader(sourceSet);
    }
}