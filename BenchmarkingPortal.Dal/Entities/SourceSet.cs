using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkingPortal.Dal.Entities;
public class SourceSet
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Root { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public virtual ICollection<SetFile> SetFiles { get; } = new List<SetFile>();
    public virtual ICollection<PropertyFile> PropertyFiles { get; } = new List<PropertyFile>();
    public virtual User User { get; set; } = null!;
}

