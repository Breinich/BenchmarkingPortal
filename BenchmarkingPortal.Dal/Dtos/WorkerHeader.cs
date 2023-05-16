using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class WorkerHeader
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Ram { get; set; }

    public int Cpu { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int Storage { get; set; }

    public string? Address { get; set; }

    public int Port { get; set; }

    public int ComputerGroupId { get; set; }

    public DateTime AddedDate { get; set; }
    
    public string UserName { get; set; } = null!;

    public WorkerHeader() { }

    public WorkerHeader(Worker worker)
    {
        Id = worker.Id;
        AddedDate = worker.AddedDate;
        Address = worker.Address;
        Port = worker.Port;
        ComputerGroupId = worker.ComputerGroupId;
        Cpu = worker.Cpu;
        Ram = worker.Ram;
        Storage = worker.Storage;
        Name = worker.Name;
        Login = worker.Login;
        Password = worker.Password;
        UserName = worker.UserName;
    }
}
