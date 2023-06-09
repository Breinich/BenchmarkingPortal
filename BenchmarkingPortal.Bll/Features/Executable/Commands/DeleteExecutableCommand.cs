﻿using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Commands;

public class DeleteExecutableCommand : IRequest
{
    public int ExecutableId { get; set; }
    public string InvokerName { get; set; } = null!;
    public string FileId { get; set; } = null!;
}