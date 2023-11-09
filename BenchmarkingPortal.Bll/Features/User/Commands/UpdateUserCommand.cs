using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.User.Commands;

public class UpdateUserCommand : IRequest<UserHeader>
{
    public string InvokerName { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public bool Subscription { get; init; }
    public string? Role { get; init; }
}