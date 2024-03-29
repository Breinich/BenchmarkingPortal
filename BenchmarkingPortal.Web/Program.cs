using System.IO.Compression;
using System.Net;
using System.Text;
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
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Concatenation;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;

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
        typeof(GetAllSetFileNamesQuery).Assembly,
        typeof(CreateUserCommand).Assembly,
        typeof(SetFileExistsByNameQuery).Assembly,
        typeof(ExecutableExistsByNameQuery).Assembly,
        typeof(GetExecutableByPathQuery).Assembly,
        typeof(GetSetFileByPathQuery).Assembly,
        typeof(GetAllPropertyFileNamesBySourceSetQuery).Assembly,
        typeof(GetExecutableByIdQuery).Assembly,
        typeof(GetConfigurationByIdQuery).Assembly,
        typeof(DeleteConfigurationCommand).Assembly,
        typeof(DownloadResultCommand).Assembly,
        typeof(DownloadUploadedFileCommand).Assembly,
        typeof(GetAllCpuModelsQuery).Assembly
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
                 throw new ApplicationException("Missing working directory path configuration!"),
    VcloudBenchmarkPath = Path.Join(builder.Configuration["Storage:WorkingDir"], "benchexec", "contrib", 
        "vcloud-benchmark.py"),
    VcloudDirectory = Path.Join(builder.Configuration["Storage:WorkingDir"], "benchexec", "contrib", 
        "vcloud"),
    WorkerConfig = builder.Configuration["Storage:WorkerConfig"] ?? 
                   throw new ApplicationException("Missing worker config path configuration!"),
    SshConfig = builder.Configuration["Storage:SshConfig"] ??
                throw new ApplicationException("Missing ssh config path configuration!"),
    SshPubKey = builder.Configuration["Storage:SshPubKey"] ??
               throw new ApplicationException("Missing ssh public key path configuration!"),
    VcloudHost = builder.Configuration["VCloud:Hostname"] ?? 
                 throw new ApplicationException("Missing vcloud hostname configuration!"),
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
app.MapTus("/files/", TusConfigurationFactory);

app.MapRazorPages();

app.Run();

static Task<DefaultTusConfiguration> TusConfigurationFactory(HttpContext httpContext)
{
    var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

    if (httpContext.Request.Headers["extension"] == StringValues.Empty)
    {
        throw new ApplicationException("Missing extension path from request headers");
    }

    var diskStorePath = (httpContext.Request.Headers["extension"][0] ??
                         throw new ApplicationException("Missing extension path value from request headers"))
        switch
        {
            "zip" => Path.Join(httpContext.RequestServices.GetRequiredService<PathConfigs>().WorkingDir, 
                httpContext.User.Identity?.Name, "tools"),
            _ => throw new ArgumentException("Invalid extension path value from request headers")
        };
    
    Directory.CreateDirectory(diskStorePath);

    var config = new DefaultTusConfiguration
    {
        Store = new CustomTusDiskStore(diskStorePath, httpContext.RequestServices.GetRequiredService<IMediator>()),
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

            OnBeforeCreateAsync = async ctx =>
            {
                // Partial files are not complete so we do not need to validate the metadata
                if (ctx.FileConcatenation is FileConcatPartial) return;

                if (!ctx.Metadata.ContainsKey("name") || ctx.Metadata["name"].HasEmptyValue)
                    ctx.FailRequest("#Name metadata must be specified.#");
                
                var mediator = ctx.HttpContext.RequestServices.GetRequiredService<IMediator>();
                var fileName = ctx.Metadata["name"].GetString(Encoding.UTF8);
                
                if(await mediator.Send(new SetFileExistsByNameQuery
                   {
                       FileName = fileName
                   }) 
                   || 
                   await mediator.Send(new ExecutableExistsByNameQuery
                   {
                       FileName = fileName
                   })
                   || 
                   File.Exists(Path.Join(diskStorePath, fileName)))
                    ctx.FailRequest("#File with this name already exists.#");
                
                if (!fileName.EndsWith(".zip") && !fileName.EndsWith(".set"))
                    ctx.FailRequest("#Invalid file extension.#");
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
                if (ctx.FileId.Split(".").Last() == "zip")
                {
                    Directory.Delete(Path.Join(diskStorePath, Path.ChangeExtension(ctx.FileId, null)), true);
                }
                return Task.CompletedTask;
            },
            OnFileCompleteAsync = ctx =>
            {
                logger.LogInformation($"Upload of {ctx.FileId} completed using {ctx.Store.GetType().FullName}");
                // If the store implements ITusReadableStore one could access the completed file here.
                // The default TusDiskStore implements this interface:
                // var file = await ctx.GetFileAsync();

                if (ctx.FileId.Split(".").Last() == "zip")
                {
                    ZipFile.ExtractToDirectory(Path.Join(diskStorePath, ctx.FileId), diskStorePath, true);
                }
                
                File.Delete(Path.Join(diskStorePath, ctx.FileId + ".uploadlength"));
                File.Delete(Path.Join(diskStorePath, ctx.FileId + ".chunkstart"));
                File.Delete(Path.Join(diskStorePath, ctx.FileId + ".chunkcomplete"));
                File.Delete(Path.Join(diskStorePath, ctx.FileId + ".expiration"));

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