using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ConfigurationHeader
{
    public ConfigurationHeader()
    { }
    
    public ConfigurationHeader(Configuration configuration)
    {
        Id = configuration.Id;
        ConfigurationItems = configuration.ConfigurationItems.Select(ci => new ConfigurationItemHeader(ci)).ToList();
        Constraints = configuration.Constraints.Select(co => new ConstraintHeader(co)).ToList();
    }
    
    public int Id { get; init; }
    
    public ICollection<ConfigurationItemHeader>? ConfigurationItems { get; init; }
    
    public ICollection<ConstraintHeader>? Constraints { get; init; }
}