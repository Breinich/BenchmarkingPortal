namespace BenchmarkingPortal.Dal.Dtos;

public class ConfigurationItemHeader
{
    public string? Key { get; set; }

    public string? Value { get; set; }

    public int Scope { get; set; }

    public int ConfigurationId { get; set; }
}
