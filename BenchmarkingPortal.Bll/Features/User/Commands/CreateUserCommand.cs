using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.User.Commands;

public class CreateUserCommand : IRequest<UserHeader>
{
    public string UserName { get; init; } = null!;
    
    public string Email { get; init; } = null!;
    
    public string Password { get; init; } = null!;
}