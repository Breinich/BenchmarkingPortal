using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ComputerGroupHeader
{
    public ComputerGroupHeader()
    {
    }

    public ComputerGroupHeader(ComputerGroup cg)
    {
        Id = cg.Id;
        Description = cg.Description;
    }

    public int Id { get; set; }

    public string? Description { get; set; }
}