using BenchmarkingPortal.Dal.SeedInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Web.Hosting;

public static class HostDataExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync();

            var roleSeeder = serviceProvider.GetRequiredService<IRoleSeedService>();
            await roleSeeder.SeedRoleAsync();

            var userSeeder = serviceProvider.GetRequiredService<IUserSeedService>();
            await userSeeder.SeedUserAsync();
        }

        return host;
    }
}