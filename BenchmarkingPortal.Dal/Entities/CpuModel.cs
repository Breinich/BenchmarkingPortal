using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkingPortal.Dal.Entities;

public class CpuModel
{
    public int Id { get; set; }

    public string Value { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}
