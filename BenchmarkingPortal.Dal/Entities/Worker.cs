namespace BenchmarkingPortal.Dal.Entities;

public class Worker
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Ram { get; set; }
    public int Cpu { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Port { get; set; }
    public int CpuModelId { get; set; }
    public int ComputerGroupId { get; set; }
    public DateTime AddedDate { get; set; }
    public string UserName { get; set; } = null!;
    public virtual ComputerGroup ComputerGroup { get; set; } = null!;
    public virtual CpuModel CpuModel { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}