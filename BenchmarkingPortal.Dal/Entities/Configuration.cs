namespace BenchmarkingPortal.Dal.Entities;

public partial class Configuration
{
    public int Id { get; set; }

    public virtual ICollection<ConfigurationItem> ConfigurationItems { get; } = new List<ConfigurationItem>();

    public virtual ICollection<Constraint> Constraints { get; } = new List<Constraint>();
}
