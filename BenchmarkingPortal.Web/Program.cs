using System.Net;
using BenchmarkingPortal;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Bll.Features.User.Queries;
using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Bll.Features.Worker.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Entities;
using BenchmarkingPortal.Dal.SeedInterfaces;
using BenchmarkingPortal.Dal.SeedService;
using BenchmarkingPortal.Web;
using BenchmarkingPortal.Web.Endpoints;
using BenchmarkingPortal.Web.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Concatenation;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

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
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
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
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromDays(1);
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
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
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
        typeof(GetResultPathQuery).Assembly,
        typeof(CreateComputerGroupCommand).Assembly,
        typeof(GetAllComputerGroupsQuery).Assembly,
        typeof(CreateConfigurationCommand).Assembly,
        typeof(DeleteExecutableCommand).Assembly,
        typeof(UploadNewExecutableCommand).Assembly,
        typeof(GetAllExecutablesQuery).Assembly,
        typeof(DeleteSetFileCommand).Assembly,
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
        typeof(GetAllSetFileNamesQuery).Assembly
    ));

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = 1024L * 1024 * 1024 * 10;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(o => o.Limits.MaxRequestBodySize = 1024L * 1024 * 1024 * 10);

builder.Services.AddSingleton<TusDiskStorageOptionHelper>();
builder.Services.AddSingleton(services => CreateTusConfigurationForCleanupService(services));
builder.Services.AddHostedService<ExpiredFilesCleanupService>();

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
app.MapTus("/files/", TusConfigurationFactory);

app.MapRazorPages();

app.Run();

static DefaultTusConfiguration CreateTusConfigurationForCleanupService(IServiceProvider services)
{
    var path = services.GetRequiredService<TusDiskStorageOptionHelper>().StorageDiskPath;

    // Simplified configuration just for the ExpiredFilesCleanupService to show load order of configs.
    return new DefaultTusConfiguration
    {
        Store = new TusDiskStore(path),
        Expiration = new SlidingExpiration(TimeSpan.FromMinutes(5))
    };
}

static Task<DefaultTusConfiguration> TusConfigurationFactory(HttpContext httpContext)
{
    var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

    var diskStorePath = httpContext.RequestServices.GetRequiredService<TusDiskStorageOptionHelper>().StorageDiskPath;

    var config = new DefaultTusConfiguration
    {
        Store = new TusDiskStore(diskStorePath, true),
        MetadataParsingStrategy = MetadataParsingStrategy.AllowEmptyValues,
        UsePipelinesIfAvailable = true,
        Events = new Events
        {
            OnAuthorizeAsync = ctx =>
            {
                // Note: This event is called even if RequireAuthorization is called on the endpoint.
                // In that case this event is not required but can be used as fine-grained authorization control.
                // This event can also be used as a "on request started" event to prefetch data or similar.

                var user = ctx.HttpContext.User;
                if (!user.IsInRole(Roles.Admin) && !user.IsInRole(Roles.User))
                {
                    ctx.FailRequest(HttpStatusCode.Forbidden, "You must be logged in to upload files");
                    return Task.CompletedTask;
                }

                // Verify different things depending on the intent of the request.
                // E.g.:
                //   Does the file about to be written belong to this user?
                //   Is the current user allowed to create new files or have they reached their quota?
                //   etc etc
                switch (ctx.Intent)
                {
                    case IntentType.CreateFile:
                        break;
                    case IntentType.ConcatenateFiles:
                        break;
                    case IntentType.WriteFile:
                        break;
                    case IntentType.DeleteFile:
                        break;
                    case IntentType.GetFileInfo:
                        break;
                    case IntentType.GetOptions:
                        break;
                }

                return Task.CompletedTask;
            },

            OnBeforeCreateAsync = ctx =>
            {
                // Partial files are not complete so we do not need to validate
                // the metadata in our example.
                if (ctx.FileConcatenation is FileConcatPartial) return Task.CompletedTask;

                if (!ctx.Metadata.ContainsKey("name") || ctx.Metadata["name"].HasEmptyValue)
                    ctx.FailRequest("name metadata must be specified. ");

                if (!ctx.Metadata.ContainsKey("contentType") || ctx.Metadata["contentType"].HasEmptyValue)
                    ctx.FailRequest("contentType metadata must be specified. ");

                return Task.CompletedTask;
            },
            OnCreateCompleteAsync = ctx =>
            {
                logger.LogInformation($"Created file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                return Task.CompletedTask;
            },
            OnBeforeDeleteAsync = ctx =>
            {
                // Can the file be deleted? If not call ctx.FailRequest(<message>);
                return Task.CompletedTask;
            },
            OnDeleteCompleteAsync = ctx =>
            {
                logger.LogInformation($"Deleted file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                return Task.CompletedTask;
            },
            OnFileCompleteAsync = ctx =>
            {
                logger.LogInformation($"Upload of {ctx.FileId} completed using {ctx.Store.GetType().FullName}");
                // If the store implements ITusReadableStore one could access the completed file here.
                // The default TusDiskStore implements this interface:
                //var file = await ctx.GetFileAsync();
                return Task.CompletedTask;
            }
        },
        // Set an expiration time where incomplete files can no longer be updated.
        // This value can either be absolute or sliding.
        // Absolute expiration will be saved per file on create
        // Sliding expiration will be saved per file on create and updated on each patch/update.
        Expiration = new SlidingExpiration(TimeSpan.FromMinutes(5))
    };

    return Task.FromResult(config);
}