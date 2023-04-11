using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Web.Hosting;

public static class HostDataExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider; 
            var context = serviceProvider.GetRequiredService<TContext>(); 
            context.Database.Migrate();
        } 
        
        return host;
    }
}