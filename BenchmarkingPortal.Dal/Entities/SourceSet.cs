using System;
using System.Collections.Generic;

namespace BenchmarkingPortal.Dal.Entities;

public partial class SourceSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Version { get; set; } = null!;

    public DateTime UploadedDate { get; set; }
    public int UserId { get; set; }


    public virtual User User { get; set; } = null!;
}
