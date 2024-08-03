using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

/// <summary>
/// A DTO for the SourceSet entity that only includes the properties that are displayed in the UI.
/// </summary>
public class SourceSetHeader
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SourceSetHeader() { }

    /// <summary>
    /// Constructor that initializes the DTO with the properties of the given SourceSet entity.
    /// </summary>
    /// <param name="sc">The SourceSet entity to copy the properties from.</param>
    public SourceSetHeader(SourceSet sc)
    {
        Id = sc.Id;
        Name = sc.Name;
        Path = sc.Path;
        UserName = sc.UserName;
        UploadedDate = sc.UploadedDate;
    }

    /// <summary>
    /// <see cref="SourceSet.Id"/>
    /// </summary>
    public int Id { get; init; }
    
    /// <summary>
    /// <see cref="SourceSet.Name"/>
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// <see cref="SourceSet.Path"/>
    /// </summary>
    public string? Path { get; set; }
    
    /// <summary>
    /// <see cref="SourceSet.UserName"/>
    /// </summary>
    public string? UserName { get; init; }
    
    /// <summary>
    /// <see cref="SourceSet.UploadedDate"/>
    /// </summary>
    public DateTime UploadedDate { get; set; }
    
    /// <summary>
    /// The root directory of the property files within the source set, relative to the user's sourceset directory.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Name"/> is not set.</exception>
    public string PropertyFilesPath => System.IO.Path.Join(Name ?? throw new ArgumentException(nameof(SourceSet) + "." + nameof(Name) + " is not set!"), "c", "properties");
    
    /// <summary>
    /// The root directory of the set files within the source set, relative to the the user's sourceset directory.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Name"/> is not set.</exception>
    public string SetFilesDir => System.IO.Path.Join(Name ?? throw new ArgumentException(nameof(SourceSet) + "." + nameof(Name) + " is not set!"), "c");
}
