namespace BenchmarkingPortal.Dal.Dtos;

public class ConfigurationHeader
{
    public int Id { get; set; }

    public ICollection<ConfigurationItemHeader>? ConfigurationItems { get; set; }

    public ICollection<ConstraintHeader>? Constraints { get; set; }
}
