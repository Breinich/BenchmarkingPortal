using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.User.Commands;

public class UpdateUserCommand : IRequest<UserHeader>
{
    public string InvokerName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public bool Subscription { get; set; }
    public string? Role { get; set; }
}