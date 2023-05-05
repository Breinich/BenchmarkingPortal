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

        await SeedTestUsersAsync();
    }

    private async Task SeedTestUsersAsync()
    {
        if (_userManager.Users.Count() < 10)
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var user = new User
                {
                    UserName = $"TestGuest{i}",
                    Email = $"test{i}@guest.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var pass = $"{i}.769+87656_{i * random.Next(100)}_ikhFDGHhoihf";

                var createResult = await _userManager.CreateAsync(user, pass);

                if (!createResult.Succeeded)
                {
                    throw new ApplicationException("Test user could not be created: " +
                                                   string.Join(", ",
                                                       createResult.Errors
                                                           .Select(e => e.Description)));

                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Guest);

                if (!addToRoleResult.Succeeded)
                {
                    throw new ApplicationException("Test user could not be added to role: " +
                                                   string.Join(", ",
                                                       addToRoleResult.Errors
                                                           .Select(e => e.Description)));
                }
            }

            for (int i = 0; i < 10; i++)
            {
                var user = new User
                {
                    UserName = $"TestUser{i}",
                    Email = $"test{i}@user.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var pass = $"{i}.769+87656_{i * random.Next(100)}_ikhZTUJJihf";

                var createResult = await _userManager.CreateAsync(user, pass);

                if (!createResult.Succeeded)
                {
                    throw new ApplicationException("Test user could not be created: " +
                                                   string.Join(", ",
                                                       createResult.Errors
                                                           .Select(e => e.Description)));

                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.User);

                if (!addToRoleResult.Succeeded)
                {
                    throw new ApplicationException("Test user could not be added to role: " +
                                                   string.Join(", ",
                                                       addToRoleResult.Errors
                                                           .Select(e => e.Description)));
                }
            }
        }
    }
}