using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;

namespace BenchmarkingPortal.Bll.Features.Worker.CommandHandlers;

// ReSharper disable once UnusedType.Global
public class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand, WorkerHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly ICommandExecutor _commandExecutor;
    private readonly string _sshConfigPath;
    private readonly string _workerConfigPath;
    private readonly string _tab;

    public AddWorkerCommandHandler(BenchmarkingDbContext context, PathConfigs pathConfigs, ICommandExecutor commandExecutor)
    {
        _context = context;
        _commandExecutor = commandExecutor;
        _sshConfigPath = pathConfigs.SshConfig;
        _workerConfigPath = pathConfigs.WorkerConfig;
        _tab = pathConfigs.Tab;
    }


    public async Task<WorkerHeader> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        if(await _context.Workers.Where(w => w.Name == request.Name).AnyAsync(cancellationToken))
            throw new ArgumentException("A worker with the given name already exists.");
        
        var workerDto = new WorkerHeader
        {
            AddedDate = request.AddedDate,
            Port = request.Port,
            ComputerGroupId = request.ComputerGroupId,
            Name = request.Name,
            Login = request.Username,
            Password = request.Password,
            UserName = request.InvokerName
        };

        var workerUrl = await _context.ComputerGroups.Where(cg => cg.Id == request.ComputerGroupId)
            .Select(c => c.Hostname).FirstAsync(cancellationToken);
        
        // check connection with SSH.Net
        var client = new SftpClient(workerUrl, workerDto.Port, workerDto.Login, workerDto.Password);
        await client.ConnectAsync(cancellationToken);
        if (client.IsConnected)
        {
            // transfer the public ssh key of the server to the worker
            // and set up required packages and the environment based on verifiercloud/doc/Deployment.md
            // ... using SshPubKeyPath and workerDto's ssh creds
            client.Disconnect();
            // add worker info to the ssh config
            var sshEntryLines = new List<string>{"Host " + workerDto.Name,
                _tab + " StrictHostKeyChecking no",
                _tab + " HostName " + workerUrl,
                _tab + " Port " + workerDto.Port,
                _tab + " User vcloud"};
            await File.AppendAllLinesAsync(_sshConfigPath, sshEntryLines, cancellationToken);
        }
        else
            throw new ArgumentException("Failed to connect to the worker using the given credentials.");
        
        // add to the vcloud and write into config files
        await _commandExecutor.ExecuteAsync("worker", new []{
            "start", workerDto.Name, 
            "--useReverseTunnel", "true"
        });
        
        // parse the WorkerInformation file and add the hostname to it
        var workerConfigLines = (await File.ReadAllLinesAsync(_workerConfigPath, cancellationToken)).ToList();
        var workerExists = workerConfigLines.Any(line => line.Contains(workerDto.Name));

        if (!workerExists)
            await File.AppendAllLinesAsync(_workerConfigPath, new List<string> { _tab + _tab + "- " + workerDto.Name },
                cancellationToken);
        
        // gather info about the worker
        var online = false;
        var timeOut = DateTime.UtcNow.AddMinutes(1);
        while (!online || workerDto.CpuModelValue == null || workerDto.Cpu == 0 || workerDto.Ram == 0)
        {
            var result = await _commandExecutor.ExecuteAsync("info", new[] { "worker", workerDto.Name });
            var workerInfo = result.Split("\n").ToList();
            workerInfo.RemoveAll(string.IsNullOrWhiteSpace);
    
            foreach (var parts in workerInfo.Select(row => row.TrimStart()).Select(line => line.Split().ToList()))
            {
                parts.RemoveAll(string.IsNullOrWhiteSpace);
                switch (parts[0])
                {
                    case "Worker":
                        if (parts.Contains("OFFLINE"))
                            online = false;
                        else if (parts.Contains("ONLINE:AVAILABLE"))
                        {
                            online = true;
                        }
                        break;
                    case "System:":
                        workerDto.CpuModelValue = string.Join(" ", parts.Skip(1));
                        break;
                    case "Cores:":
                        workerDto.Cpu = int.Parse(parts[1]);
                        break;
                    case "Memory:":
                        workerDto.Ram = (int)float.Floor(float.Parse(parts[1]));
                        break;
                }
            }

            await Task.Delay(1000, cancellationToken);
            
            if (DateTime.UtcNow > timeOut)
                throw new ArgumentException("It is not known whether the worker has successfully been started or not.");
        }

        var cpuModel = await _context.CpuModels.Where(cm => cm.Value == workerDto.CpuModelValue)
            .FirstOrDefaultAsync(cancellationToken);
        if (cpuModel == null)
        {
            cpuModel = new Dal.Entities.CpuModel
            {
                Name = workerDto.CpuModelValue,
                Value = workerDto.CpuModelValue
            };
            _context.CpuModels.Add(cpuModel);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        var worker = new Dal.Entities.Worker
        {
            AddedDate = workerDto.AddedDate,
            Port = workerDto.Port,
            ComputerGroupId = workerDto.ComputerGroupId,
            Cpu = workerDto.Cpu,
            Ram = workerDto.Ram,
            CpuModelId = cpuModel.Id,
            Name = workerDto.Name,
            Login = request.Username,
            Password = workerDto.Password,
            UserName = request.InvokerName
        };

        _context.Workers.Add(worker);
        await _context.SaveChangesAsync(cancellationToken);

        return new WorkerHeader(worker);
    }
}