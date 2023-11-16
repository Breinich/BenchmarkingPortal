using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class VcloudBenchmarkHeader
{
    public VcloudBenchmarkHeader()
    { }
    
    public VcloudBenchmarkHeader(VcloudBenchmark vB)
    {
        BenchmarkId = vB.BenchmarkId;
        VcloudId = vB.VcloudId;
    }
    
    public int BenchmarkId { get; set; }
    
    public string VcloudId { get; set; } = null!;
}