﻿using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Benchmark.Commands;

public class StartBenchmarkCommand : IRequest<BenchmarkHeader>
{
    public string Name { get; set; } = null!;

    public int Priority { get; set; }

    public int Ram { get; set; }

    public int Cpu { get; set; }

    public int TimeLimit { get; set; }

    public int HardTimeLimit { get; set; }
    
    public string CpuModel { get; set; } = null!;

    public int ExecutableId { get; set; }

    public string SetFilePath { get; set; } = null!;

    public string PropertyFilePath { get; set; } = null!;

    public int ConfigurationId { get; set; }
    
    public int ComputerGroupId { get; set; }

    public string InvokerName { get; set; } = null!;
}