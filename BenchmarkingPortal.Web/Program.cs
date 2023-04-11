using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Web.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BenchmarkingDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString(nameof(BenchmarkingDbContext))));

builder.Services.AddRazorPages();

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<BenchmarkingDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(o => o.Title = "Benchmarking API");

var app = builder.Build();

app.MigrateDatabase<BenchmarkingDbContext>();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
