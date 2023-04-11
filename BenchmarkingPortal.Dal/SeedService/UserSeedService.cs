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
        if (!(await _userManager.GetUsersInRoleAsync(Roles.Adminsitrators)).Any())
        {
            var user = new User
            {
                UserName = "admin",
                Email = "bajnokvencel@edu.bme.hu",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createResult = await _userManager.CreateAsync(user, "$AdministratorMaxiBear");
            var addToRoleReuslt = await _userManager.AddToRoleAsync(user, Roles.Adminsitrators);

            if (!createResult.Succeeded || !addToRoleReuslt.Succeeded)
            {
                throw new ApplicationException("Administrator could not be created: " +
                                               string.Join(", ",
                                                   createResult.Errors.Concat(addToRoleReuslt.Errors)
                                                       .Select(e => e.Description)));
            }
        }
    }
}