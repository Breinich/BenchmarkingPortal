using System.Runtime.CompilerServices;
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
        if (!await _roleManager.RoleExistsAsync(Roles.Adminsitrators))
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Adminsitrators });
    }
}