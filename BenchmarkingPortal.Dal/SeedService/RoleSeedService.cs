using BenchmarkingPortal.Dal.SeedInterfaces;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Dal.SeedService;

public class RoleSeedService :IRoleSeedService
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleSeedService(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedRoleAsync()
    {
        if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Admin });
        if (!await _roleManager.RoleExistsAsync(Roles.User))
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.User });
        if (!await _roleManager.RoleExistsAsync(Roles.Guest))
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Guest });
    }
}