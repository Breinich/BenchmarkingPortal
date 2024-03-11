using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

/// <summary>
/// Get all executables
/// </summary>
public class GetAllExecutablesQuery : IRequest<IEnumerable<ExecutableHeader>> { }