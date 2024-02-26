using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkingPortal.Dal.Entities;

public class PropertyFile
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string Version { get; set; } = null!;
    public int SourceSetId { get; set; }
    public DateTime UploadedDate { get; set; }
    public string UserName { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual SourceSet SourceSet { get; set; } = null!;
}
