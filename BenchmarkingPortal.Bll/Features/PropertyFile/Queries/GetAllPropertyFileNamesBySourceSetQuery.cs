using MediatR;

namespace BenchmarkingPortal.Bll.Features.PropertyFile.Queries;

/// <summary>
/// Get the names of the property files in the given source set
/// </summary>
public class GetAllPropertyFileNamesBySourceSetQuery : IRequest<IEnumerable<string?>>
{
    public int SourceSetId { get; init; }
}