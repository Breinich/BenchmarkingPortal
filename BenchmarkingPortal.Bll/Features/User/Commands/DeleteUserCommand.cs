using MediatR;

namespace BenchmarkingPortal.Bll.Features.User.Commands;

public class DeleteUserCommand : IRequest
{
    public string InvokerName { get; set; } = null!;
    public string UserName { get; set; } = null!;
}