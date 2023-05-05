using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ConfigurationItemHeader
{
    public string? Key { get; set; }

    public string? Value { get; set; }

    public Scope Scope { get; set; }

    public ConfigurationHeader? Configuration { get; set; }
}
