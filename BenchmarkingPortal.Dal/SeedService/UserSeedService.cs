using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Dal.SeedService;

public class UserSeedService : IUserSeedService
{
    private readonly UserManager<User> _userManager;

    public UserSeedService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedUserAsync()
    {
        if (!(await _userManager.GetUsersInRoleAsync(Roles.Admin)).Any())
        {
            var user = new User
            {
                UserName = "admin",
                Email = "bajnokvencel@edu.bme.hu",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createResult = await _userManager.CreateAsync(user, "$AdministratorMaxiBear");
            var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Admin);

            if (!createResult.Succeeded || !addToRoleResult.Succeeded)
            {
                throw new ApplicationException("Administrator could not be created: " +
                                               string.Join(", ",
                                                   createResult.Errors.Concat(addToRoleResult.Errors)
                                                       .Select(e => e.Description)));
            }
        }
    }
}