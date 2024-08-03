namespace BenchmarkingPortal.Dal.Entities;

/// <summary>
/// Represents a source set.
/// </summary>
public class SourceSet
{
    
    /// <summary>
    /// The unique identifier for the source set.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the source set, = the name of the source set's folder.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// The path of the source set's uploaded zip file.
    /// </summary>
    public string Path { get; set; } = null!;
    
    /// <summary>
    /// The name of the user who uploaded the source set.
    /// </summary>
    public string UserName { get; set; } = null!;
    
    /// <summary>
    /// The date the source set was uploaded.
    /// </summary>
    public DateTime UploadedDate { get; set; }
    
    /// <summary>
    /// The set files associated with the source set.
    /// </summary>
    public virtual ICollection<SetFile> SetFiles { get; } = new List<SetFile>();
    
    /// <summary>
    /// The property files associated with the source set.
    /// </summary>
    public virtual ICollection<PropertyFile> PropertyFiles { get; } = new List<PropertyFile>();
    
    /// <summary>
    /// The user who uploaded the source set.
    /// </summary>
    public virtual User User { get; set; } = null!;
}

