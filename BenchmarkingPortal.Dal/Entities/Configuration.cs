namespace BenchmarkingPortal.Dal.Entities;

public class Configuration
{
    public int Id { get; set; }
    public string XmlFilePath { get; set; }
    public virtual ICollection<ConfigurationItem> ConfigurationItems { get; } = new List<ConfigurationItem>();
    public virtual ICollection<Constraint> Constraints { get; } = new List<Constraint>();
}