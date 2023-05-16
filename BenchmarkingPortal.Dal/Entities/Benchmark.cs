namespace BenchmarkingPortal.Dal.Entities;

public partial class Benchmark
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Priority { get; set; }

    public Status Status { get; set; }

    public int Ram { get; set; }

    public int Cpu { get; set; }

    public string? Result { get; set; }

    public int TimeLimit { get; set; }

    public int HardTimeLimit { get; set; }

    public int ComputerGroupId { get; set; }

    public int ExecutableId { get; set; }

    public int SourceSetId { get; set; }

    public string SetFilePath { get; set; } = null!;

    public string PropertyFilePath { get; set; } = null!;

    public DateTime StartedDate { get; set; }

    public int ConfigurationId { get; set; }
    public string UserName { get; set; } = null!;
    
    public virtual ComputerGroup ComputerGroup { get; set; } = null!;

    public virtual Configuration Configuration { get; set; } = null!;

    public virtual Executable Executable { get; set; } = null!;

    public virtual SourceSet SourceSet { get; set; } = null!;

    public virtual User User { get; set; } = null!;

}
