using MediatR;

namespace BenchmarkingPortal.Bll.Features.User.Commands;

public class DeleteUserCommand : IRequest
{
    public string InvokerName { get; init; } = null!;
    public string UserName { get; init; } = null!;
}