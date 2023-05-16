namespace BenchmarkingPortal.Dal.Entities;

public class Constraint
{
    public string Premise { get; set; } = null!;

    public string Consequence { get; set; } = null!;

    public int ConfigurationId { get; set; }
}