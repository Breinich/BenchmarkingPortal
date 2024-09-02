using System.Configuration;
using BenchmarkingPortal;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Bll.Features.Configuration.Queries;
using BenchmarkingPortal.Bll.Features.CpuModel.Queries;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
using BenchmarkingPortal.Bll.Features.Result.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Bll.Features.UploadedFile.Commands;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Bll.Features.User.Queries;
using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Bll.Features.Worker.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using BenchmarkingPortal.Dal.SeedService;
using BenchmarkingPortal.Web.Endpoints;
using BenchmarkingPortal.Web.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using tusdotnet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); });
builder.Services.AddMemoryCache();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddIdentity<User, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = false)
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
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
    options.User.RequireUniqueEmail = false;
});


builder.Services.AddDbContext<BenchmarkingDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnString"),
        x => x.MigrationsAssembly("BenchmarkingPortal.Migrations.Base")));

builder.Services.AddScoped<IRoleSeedService, RoleSeedService>();
builder.Services.AddScoped<IUserSeedService, UserSeedService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.LoginPath = "/Identity/Account/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromHours(1);
});

builder.Services.AddAuthentication().AddCookie(
        option =>
        {
            option.ExpireTimeSpan = TimeSpan.FromDays(1);
            option.Cookie.HttpOnly = true;
            option.SlidingExpiration = true;
        })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"] ?? throw new InvalidOperationException();
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"] ?? throw new InvalidOperationException();
        options.CallbackPath = "/github-oauth";
        options.Scope.Add("read:user");
        options.Scope.Add("user:email");
        options.Scope.Add("read:org");
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
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
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(DeleteBenchmarkCommand).Assembly,
        typeof(StartBenchmarkCommand).Assembly,
        typeof(UpdateBenchmarkCommand).Assembly,
        typeof(GetAllBenchmarksQuery).Assembly,
        typeof(CreateComputerGroupCommand).Assembly,
        typeof(GetAllComputerGroupsQuery).Assembly,
        typeof(CreateConfigurationCommand).Assembly,
        typeof(DeleteExecutableCommand).Assembly,
        typeof(UploadNewExecutableCommand).Assembly,
        typeof(GetAllExecutablesQuery).Assembly,
        typeof(UploadNewSetFileCommand).Assembly,
        typeof(GetAllSetFilesQuery).Assembly,
        typeof(AddWorkerCommand).Assembly,
        typeof(RemoveWorkerCommand).Assembly,
        typeof(UpdateWorkerCommand).Assembly,
        typeof(GetAllWorkersQuery).Assembly,
        typeof(GetAllUsersQuery).Assembly,
        typeof(UpdateUserCommand).Assembly,
        typeof(DeleteUserCommand).Assembly,
        typeof(GetAllComputerGroupsWithStatsQuery).Assembly,
        typeof(DeleteComputerGroupCommand).Assembly,
        typeof(UpdateComputerGroupCommand).Assembly,
        typeof(GetSetFileNamesBySourceSetIdQuery).Assembly,
        typeof(CreateUserCommand).Assembly,
        typeof(SetFileExistsByNameQuery).Assembly,
        typeof(ExecutableExistsByNameQuery).Assembly,
        typeof(GetExecutableByPathQuery).Assembly,
        typeof(GetSetFileByPathQuery).Assembly,
        typeof(GetPropertyFileNamesBySourceSetQuery).Assembly,
        typeof(GetExecutableByIdQuery).Assembly,
        typeof(GetConfigurationByIdQuery).Assembly,
        typeof(DeleteConfigurationCommand).Assembly,
        typeof(DownloadResultCommand).Assembly,
        typeof(DownloadUploadedFileCommand).Assembly,
        typeof(GetAllCpuModelsQuery).Assembly,
        typeof(UploadNewSourceSetCommand).Assembly,
        typeof(GetAllSourceSetsQuery).Assembly,
        typeof(DeleteSourceSetCommand).Assembly,
        typeof(GetSourceSetByIdQuery).Assembly,
        typeof(SourceSetExistsByNameQuery).Assembly,
        typeof(GetAllPropertyFilesQuery).Assembly
    ));

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = 1024L * 1024 * 1024 * 10;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(o => o.Limits.MaxRequestBodySize = 1024L * 1024 * 1024 * 10);

builder.Services.AddSingleton<PathConfigs>(_ => new PathConfigs
{
    WorkingDir = builder.Configuration["Storage:WorkingDir"] ?? 
                 throw new ConfigurationErrorsException("Missing working directory path configuration!"),
    ExecutableDir = "tools",
    SourceSetDir = "source-sets",
    ResultsDir = "results",
    SetFileDir = "c",
    PropertyFilesDir = Path.Join("c", "properties"),
    BenchmarkDir = "benchmarks",
    VcloudBenchmarkPath = Path.Join(builder.Configuration["Storage:WorkingDir"], "benchexec", "contrib", 
        "vcloud-benchmark.py"),
    VcloudDir = Path.Join(builder.Configuration["Storage:WorkingDir"], "benchexec", "contrib", 
        "vcloud"),
    WorkerConfig = builder.Configuration["Storage:WorkerConfig"] ?? 
                   throw new ConfigurationErrorsException("Missing worker config path configuration!"),
    SshConfig = builder.Configuration["Storage:SshConfig"] ??
                throw new ConfigurationErrorsException("Missing ssh config path configuration!"),
    SshPubKey = builder.Configuration["Storage:SshPubKey"] ??
               throw new ConfigurationErrorsException("Missing ssh public key path configuration!"),
    VcloudHost = builder.Configuration["VCloud:Hostname"] ?? 
                 throw new ConfigurationErrorsException("Missing vcloud hostname configuration!"),
    Tab = "    ",
});

builder.Services.AddSingleton<IBenchmarkQueue>(_ => new BenchmarkQueue());

builder.Services.AddSingleton<ICommandExecutor>(provider =>
{
    var commandExecutor = new VCloudCommandExecutor(provider.GetRequiredService<PathConfigs>(),
        provider.GetRequiredService<ILogger<VCloudCommandExecutor>>());
    commandExecutor.InitializeAsync().Wait();
    return commandExecutor;
});

builder.Services.AddHostedService<BenchmarkRunnerService>();

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

app.UseSession();


// Handle downloads (must be set before MapTus)
app.MapGet("/files/{fileId}", DownloadFileEndpoint.HandleRoute);

// Setup tusdotnet for the /files/ path.
app.MapTus("/files/", TusUtil.TusConfigurationFactory);

app.MapRazorPages();

app.Run();
