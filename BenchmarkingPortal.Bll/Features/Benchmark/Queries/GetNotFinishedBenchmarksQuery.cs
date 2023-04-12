using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Queries;

public class GetNotFinishedBenchmarksQuery : IRequest<IEnumerable<BenchmarkHeader>> { }