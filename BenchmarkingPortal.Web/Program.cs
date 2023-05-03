using BenchmarkingPortal;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using BenchmarkingPortal.Dal.SeedService;
using BenchmarkingPortal.Web.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddIdentity<User, IdentityRole<int>>( options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BenchmarkingDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
// Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
// Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
// User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddDbContext<BenchmarkingDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString(nameof(BenchmarkingDbContext)),
        x => x.MigrationsAssembly("BenchmarkingPortal.Migrations.Base")));

builder.Services.AddScoped<IRoleSeedService, RoleSeedService>();
builder.Services.AddScoped<IUserSeedService, UserSeedService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAuthentication().AddCookie()
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"] ?? throw new InvalidOperationException();
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"] ?? throw new InvalidOperationException();
        options.CallbackPath = "/github-oauth";
        options.Scope.Add("read:user");
        options.Scope.Add("user:email");
        options.Scope.Add("read:org");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.RequireAdministratorRole,
        policy => policy.RequireRole(Roles.Admin));
    options.AddPolicy(Policies.RequireApprovedUser,
        policy => policy.RequireRole(Roles.User, Roles.Admin));
});


builder.Services.ConfigureApplicationCookie(options =>
{
// Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

await app.MigrateDatabaseAsync<BenchmarkingDbContext>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
