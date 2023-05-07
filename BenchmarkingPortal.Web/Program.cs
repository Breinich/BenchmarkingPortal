using BenchmarkingPortal;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Bll.Features.User.Queries;
using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Bll.Features.Worker.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using BenchmarkingPortal.Dal.SeedService;
using BenchmarkingPortal.Web.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddMemoryCache();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

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
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
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

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DeleteBenchmarkCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(StartBenchmarkCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UpdateBenchmarkCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllBenchmarksQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetResultPathQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateComputerGroupCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllComputerGroupsQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateConfigurationCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DeleteExecutableCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UploadNewExecutableCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllExecutablesQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DeleteSourceSetCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UploadNewSourceSetCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllSourceSetsQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AddWorkerCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(RemoveWorkerCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UpdateWorkerCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllWorkersQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllUsersQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DeleteUserCommand).Assembly);
});

var app = builder.Build();

await app.MigrateDatabaseAsync<BenchmarkingDbContext>();


app.UseExceptionHandler("/Error");

if (!app.Environment.IsDevelopment())
{
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //app.UseDeveloperExceptionPage();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
