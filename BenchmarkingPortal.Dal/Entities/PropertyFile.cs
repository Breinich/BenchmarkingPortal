namespace BenchmarkingPortal.Dal.Entities;

/// <summary>
/// Represents a property file uploaded by a user.
/// </summary>
public class PropertyFile
{
    /// <summary>
    /// The unique identifier for the property file.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the property file.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// The name of the uploaded file.
    /// </summary>
    public string Path { get; set; } = null!;
    
    /// <summary>
    /// The unique identifier of the source set that the property file belongs to.
    /// </summary>
    public int SourceSetId { get; set; }
    
    /// <summary>
    /// The date and time that the property file was uploaded.
    /// </summary>
    public DateTime UploadedDate { get; set; }
    
    /// <summary>
    /// The name of the user who uploaded the property file.
    /// </summary>
    public string UserName { get; set; } = null!;
    
    /// <summary>
    /// The user who uploaded the property file.
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// The source set that the property file belongs to.
    /// </summary>
    public virtual SourceSet SourceSet { get; set; } = null!;
}
