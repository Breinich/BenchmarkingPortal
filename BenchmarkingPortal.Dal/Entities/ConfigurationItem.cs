namespace BenchmarkingPortal.Dal.Entities;

public class ConfigurationItem
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public Scope Scope { get; set; } = Scope.Local;

    public int ConfigurationId { get; set; }
}