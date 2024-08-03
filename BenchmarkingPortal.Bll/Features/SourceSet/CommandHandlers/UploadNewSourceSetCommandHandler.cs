using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.SourceSet.CommandHandlers;

public class UploadNewSourceSetCommandHandler(BenchmarkingDbContext context)
    : IRequestHandler<UploadNewSourceSetCommand, SourceSetHeader>
{
    public async Task<SourceSetHeader> Handle(UploadNewSourceSetCommand request, CancellationToken cancellationToken)
    {
        var sourceSet = new Dal.Entities.SourceSet
        {
            Name = request.Name,
            UserName = request.InvokerName,
            UploadedDate = request.UploadedDate,
            Path = request.Path
        };
        
        await context.SourceSets.AddAsync(sourceSet, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new SourceSetHeader(sourceSet);
    }
}