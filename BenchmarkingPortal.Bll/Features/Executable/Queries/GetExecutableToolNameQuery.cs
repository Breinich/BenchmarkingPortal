﻿using MediatR;

namespace BenchmarkingPortal.Bll.Features.Executable.Queries;

public class GetExecutableToolNameQuery : IRequest<string?>
{
    public int Id { get; set; }
}