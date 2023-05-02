using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BenchmarkingPortal.Dal.SeedService;

public class UserSeedService : IUserSeedService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public UserSeedService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task SeedUserAsync()
    {
        if (!(await _userManager.GetUsersInRoleAsync(Roles.Admin)).Any())
        {
            var user = new User
            {
                UserName = _configuration["Users:AdminUserName"] ?? throw new ApplicationException("Admin username not set in configuration."),
                Email = _configuration["Users:AdminEmail"] ?? throw new ApplicationException("Admin email not set in configuration."),
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var pass = _configuration["Users:AdminPassword"] ?? throw new ApplicationException("Admin password not set in configuration.");
            var createResult = await _userManager.CreateAsync(user,  pass);

            if (!createResult.Succeeded)
            {
                throw new ApplicationException("Administrator could not be created: " +
                                                   string.Join(", ",
                                                       createResult.Errors
                                                          .Select(e => e.Description)));
                
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Admin);

            if (!addToRoleResult.Succeeded)
            {
                throw new ApplicationException("Administrator could not be added to role: " +
                                               string.Join(", ",
                                                   addToRoleResult.Errors
                                                       .Select(e => e.Description)));
            }
        }
    }
}