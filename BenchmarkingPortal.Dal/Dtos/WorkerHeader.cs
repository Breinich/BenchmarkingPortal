using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class WorkerHeader
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Ram { get; set; }

    public int Cpu { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int Storage { get; set; }

    public string? Address { get; set; }

    public int Port { get; set; }

    public int ComputerGroupId { get; set; }

    public DateTime AddedDate { get; set; }

    public WorkerHeader() { }

    public WorkerHeader(Worker worker)
    {

        AddedDate = worker.AddedDate;
        Address = worker.Address;
        Port = worker.Port;
        ComputerGroupId = worker.ComputerGroupId;
        Cpu = worker.Cpu;
        Ram = worker.Ram;
        Storage = worker.Storage;
        Name = worker.Name;
        Username = worker.Username;
        Password = worker.Password;
    }
}
