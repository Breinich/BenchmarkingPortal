using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ConstraintHeader
{
    public ConstraintHeader(Constraint constraint)
    {
        Premise = constraint.Premise;
        Consequence = constraint.Consequence;
        ConfigurationId = constraint.ConfigurationId;
    }
    
    public string? Premise { get; set; }

    public string? Consequence { get; set; }

    public int ConfigurationId { get; set; }
}