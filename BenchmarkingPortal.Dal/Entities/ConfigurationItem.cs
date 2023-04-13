namespace BenchmarkingPortal.Dal.Entities;

public partial class ConfigurationItem
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public Scope Scope { get; set; }

    public int ConfigurationId { get; set; }
}
