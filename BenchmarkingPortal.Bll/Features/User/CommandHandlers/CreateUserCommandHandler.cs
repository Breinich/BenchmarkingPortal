using System.Collections;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BenchmarkingPortal.Bll.Features.User.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserHeader>
{
    private readonly SignInManager<Dal.Entities.User> _signInManager;
    private readonly UserManager<Dal.Entities.User> _userManager;
    
    public CreateUserCommandHandler(SignInManager<Dal.Entities.User> signInManager, UserManager<Dal.Entities.User> userManager, ILogger<CreateUserCommandHandler> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    public async Task<UserHeader> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Dal.Entities.User { UserName = request.UserName, Email = request.Email };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Roles.Guest);

            await _signInManager.SignInAsync(user, false);
            
            return new UserHeader(user);
        }
        
        List<ArgumentException> argumentExceptions = new();
        foreach (var error in result.Errors) 
            argumentExceptions.Add(new ArgumentException(error.Description));
        
        throw new AggregateException(argumentExceptions);
    }
}