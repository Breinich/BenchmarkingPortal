namespace BenchmarkingPortal.Dal.Entities;

public class Constraint
{
    public int Id { get; set; }
    public string Expression { get; set; } = null!;
    public int ConfigurationId { get; set; }
}