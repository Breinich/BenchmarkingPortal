using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ConstraintHeader
{
    public ConstraintHeader(Constraint constraint)
    {
        Expression = constraint.Expression;
        ConfigurationId = constraint.ConfigurationId;
    }
    
    public string? Expression { get; init; }
    public int ConfigurationId { get; init; }
}