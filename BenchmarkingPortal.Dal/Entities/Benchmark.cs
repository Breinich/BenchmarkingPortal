namespace BenchmarkingPortal.Dal.Entities;

/// <summary>
/// Represents a benchmark entity.
/// </summary>
public class Benchmark
{
    /// <summary>
    /// The id of the benchmark.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the benchmark.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// The priority of the benchmark.
    /// </summary>
    public Priority Priority { get; set; }
    
    /// <summary>
    /// The status of the benchmark.
    /// </summary>
    public Status Status { get; set; }
    
    /// <summary>
    /// The amount of RAM the benchmark can use.
    /// </summary>
    public int Ram { get; set; }
    
    /// <summary>
    /// The amount of CPU cores the benchmark can use.
    /// </summary>
    public int Cpu { get; set; }
    
    /// <summary>
    /// The path where the results are stored, relative to the working directory.
    /// </summary>
    public string? ResultPath { get; set; }
    
    /// <summary>
    /// The time limit of the benchmark.
    /// </summary>
    public int TimeLimit { get; set; }
    
    /// <summary>
    /// The hard time limit of the benchmark.
    /// </summary>
    public int HardTimeLimit { get; set; }
    
    /// <summary>
    /// The id of the attached computer group.
    /// </summary>
    public int ComputerGroupId { get; set; }
    
    /// <summary>
    /// The id of the used executable.
    /// </summary>
    public int ExecutableId { get; set; }

    /// <summary>
    /// The id of the used source set.
    /// </summary>
    public int SourceSetId { get; set; }
    
    /// <summary>
    /// The id of the used CPU model.
    /// </summary>
    public int CpuModelId { get; set; }
    
    /// <summary>
    /// The path to the set file, relative to the working directory.
    /// </summary>
    public string SetFilePath { get; set; } = null!;
    
    /// <summary>
    /// The path to the property file, relative to the working directory.
    /// </summary>
    public string PropertyFilePath { get; set; } = null!;
    
    /// <summary>
    /// The date the benchmark was started.
    /// </summary>
    public DateTime StartedDate { get; set; }
    
    /// <summary>
    /// The id of the used configuration.
    /// </summary>
    public int ConfigurationId { get; set; }
    
    /// <summary>
    /// The username of the user who started the benchmark.
    /// </summary>
    public string UserName { get; set; } = null!;
    
    /// <summary>
    /// The id of the benchmark in the Verifier Cloud.
    /// </summary>
    public string? VcloudId { get; set; }
    
    /// <summary>
    /// The used computer group.
    /// </summary>
    public virtual ComputerGroup? ComputerGroup { get; set; }
    
    /// <summary>
    /// The used configuration.
    /// </summary>
    public virtual Configuration Configuration { get; set; } = null!;
    
    /// <summary>
    /// The used executable.
    /// </summary>
    public virtual Executable Executable { get; set; } = null!;
    
    /// <summary>
    /// The used source set.
    /// </summary>
    public virtual SourceSet SourceSet { get; set; } = null!;
    
    /// <summary>
    /// The owner of the benchmark.
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// The used CPU model.
    /// </summary>
    public virtual CpuModel? CpuModel { get; set; }
}