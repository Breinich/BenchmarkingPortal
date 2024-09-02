using System.Security;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BenchmarkingPortal.Dal.SeedService;

public class UserSeedService(UserManager<User> userManager, IConfiguration configuration)
    : IUserSeedService
{
    public async Task SeedUserAsync()
    {
        if (!(await userManager.GetUsersInRoleAsync(Roles.Admin)).Any())
        {
            var user = new User
            {
                UserName = configuration["Users:AdminUserName"] ??
                           throw new SecurityException("Admin username not set in configuration."),
                Email = configuration["Users:AdminEmail"] ??
                        throw new SecurityException("Admin email not set in configuration."),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var pass = configuration["Users:AdminPassword"] ??
                       throw new SecurityException("Admin password not set in configuration.");
            var createResult = await userManager.CreateAsync(user, pass);

            if (!createResult.Succeeded)
                throw new SecurityException("Administrator could not be created: " +
                                            string.Join(", ",
                                                createResult.Errors
                                                    .Select(e => e.Description)));

            var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Admin);

            if (!addToRoleResult.Succeeded)
                throw new SecurityException("Administrator could not be added to role: " +
                                            string.Join(", ",
                                                addToRoleResult.Errors
                                                    .Select(e => e.Description)));
        }

        //await SeedTestUsersAsync();
    }

    // ReSharper disable once UnusedMember.Local
    private async Task SeedTestUsersAsync()
    {
        if (userManager.Users.Count() < 10)
        {
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    UserName = $"TestGuest{i}",
                    Email = $"test{i}@guest.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var pass = $"{i}.769+87656_{i * random.Next(100)}_ikhFDGHhoihf";

                var createResult = await userManager.CreateAsync(user, pass);

                if (!createResult.Succeeded)
                    throw new ApplicationException("Test user could not be created: " +
                                                   string.Join(", ",
                                                       createResult.Errors
                                                           .Select(e => e.Description)));

                var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Guest);

                if (!addToRoleResult.Succeeded)
                    throw new ApplicationException("Test user could not be added to role: " +
                                                   string.Join(", ",
                                                       addToRoleResult.Errors
                                                           .Select(e => e.Description)));
            }

            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    UserName = $"TestUser{i}",
                    Email = $"test{i}@user.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var pass = $"{i}.769+87656_{i * random.Next(100)}_ikhZTUJJihf";

                var createResult = await userManager.CreateAsync(user, pass);

                if (!createResult.Succeeded)
                    throw new ApplicationException("Test user could not be created: " +
                                                   string.Join(", ",
                                                       createResult.Errors
                                                           .Select(e => e.Description)));

                var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.User);

                if (!addToRoleResult.Succeeded)
                    throw new ApplicationException("Test user could not be added to role: " +
                                                   string.Join(", ",
                                                       addToRoleResult.Errors
                                                           .Select(e => e.Description)));
            }
        }
    }
}