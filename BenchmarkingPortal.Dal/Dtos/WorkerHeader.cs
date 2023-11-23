using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class WorkerHeader
{
    public WorkerHeader()
    {
    }

    public WorkerHeader(Worker worker)
    {
        Id = worker.Id;
        AddedDate = worker.AddedDate;
        Address = worker.Address;
        Port = worker.Port;
        ComputerGroupId = worker.ComputerGroupId;
        Cpu = worker.Cpu;
        CpuModel = worker.CpuModel;
        Ram = worker.Ram;
        Name = worker.Name;
        Login = worker.Login;
        Password = worker.Password;
        UserName = worker.UserName;
    }

    public int Id { get; init; }

    public string? Name { get; init; }

    public int Ram { get; set; }

    public int Cpu { get; set; }
    
    public string? CpuModel { get; set; }

    public string? Login { get; init; }

    public string? Password { get; init; }

    public string? Address { get; init; }

    public int Port { get; init; }

    public int ComputerGroupId { get; set; }

    public DateTime AddedDate { get; init; }

    public string UserName { get; init; } = null!;
}