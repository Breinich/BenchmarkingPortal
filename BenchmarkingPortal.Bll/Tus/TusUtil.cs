using System.IO.Compression;
using System.Net;
using System.Text;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Bll.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using tusdotnet.Models;
using tusdotnet.Models.Concatenation;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;

namespace BenchmarkingPortal.Bll.Tus;

public class TusUtil
{
    public static string GetContentTypeOrDefault(Dictionary<string, Metadata> metadata, string defaultVal = "application/octet-stream")
    {
        return metadata.TryGetValue("contentType", out var contentType) ? 
            contentType.GetString(Encoding.UTF8) : defaultVal;
    }

    public static string GetContentNameOrDefault(Dictionary<string, Metadata> metadata, string defaultVal = "download")
    {
        return metadata.TryGetValue("name", out var nameMeta) ? nameMeta.GetString(Encoding.UTF8) : defaultVal;
    }

    private enum UploadType
    {
        Exe,
        Set,
        SourceSet
    }
    
    public static Task<DefaultTusConfiguration> TusConfigurationFactory(HttpContext httpContext)
    {
        var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<TusUtil>();
        var mediator = httpContext.RequestServices.GetRequiredService<IMediator>();
        var pathConfig = httpContext.RequestServices.GetRequiredService<PathConfigs>();

        if (httpContext.Request.Headers["type"] == StringValues.Empty)
        {
            throw new ApplicationException("Missing type information from request headers");
        }

        string diskStorePath;
        UploadType uploadType;
        
        // Determine the type of upload and set the disk store path accordingly
        switch (httpContext.Request.Headers["type"][0] ?? throw new ApplicationException("Missing type value from header"))
        {
            case "exe":
                diskStorePath = Path.Join(pathConfig.WorkingDir, httpContext.User.Identity?.Name, pathConfig.ExecutableDir);
                uploadType = UploadType.Exe;
                break;
            case "set":
                var sourceSetId = int.Parse(httpContext.Request.Headers["sourceSetId"][0] ??
                                        throw new ApplicationException("Missing sourceSetId value from request headers"));
                var sourceSet = mediator.Send(new GetSourceSetByIdQuery {Id = sourceSetId}).Result;
                if (sourceSet == null)
                    throw new ApplicationException("Source set not found");
                diskStorePath = Path.Join(pathConfig.WorkingDir, httpContext.User.Identity?.Name, pathConfig.SourceSetDir, 
                    sourceSet.Name ?? throw new ApplicationException("Source set root path not found"), pathConfig.SetFileDir);
                uploadType = UploadType.Set;
                break;
            case "sourceSet":
                diskStorePath = Path.Join(pathConfig.WorkingDir, httpContext.User.Identity?.Name, pathConfig.SourceSetDir);
                uploadType = UploadType.SourceSet;
                break;
            default:
                throw new ApplicationException("Invalid type value from header");
        }
                
        Directory.CreateDirectory(diskStorePath);

        var config = new DefaultTusConfiguration
        {
            Store = new CustomTusDiskStore(diskStorePath),
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
                    //   etc., etc.
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
                    // Partial files are not complete, so we do not need to validate the metadata
                    if (ctx.FileConcatenation is FileConcatPartial) return;

                    if (!ctx.Metadata.TryGetValue("name", out var value) || value.HasEmptyValue)
                        ctx.FailRequest("#Name metadata must be specified.#");

                    var fileName = ctx.Metadata["name"].GetString(Encoding.UTF8);
                    var fileExists = uploadType switch
                    {
                        UploadType.Exe => await mediator.Send(new ExecutableExistsByNameQuery { FileName = fileName }),
                        UploadType.Set => await mediator.Send(new SetFileExistsByNameQuery { FileName = fileName }),
                        UploadType.SourceSet => await mediator.Send(new SourceSetExistsByNameQuery { FileName = fileName }),
                        _ => false
                    };

                    if(fileExists || File.Exists(Path.Join(diskStorePath, fileName)))
                        ctx.FailRequest("#File with this name already exists.#");
                    
                    if (!fileName.EndsWith(".zip") && !fileName.EndsWith(".set"))
                        ctx.FailRequest("#Invalid file extension.#");
                },
                OnCreateCompleteAsync = ctx =>
                {
                    logger.LogInformation($"Created file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                    return Task.CompletedTask;
                },
                OnBeforeDeleteAsync = async ctx =>
                {
                    logger.LogInformation($"Deleting file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                    var itemExists = uploadType switch
                    {
                        UploadType.Exe => await mediator.Send(new ExecutableExistsByNameQuery { FileName = ctx.FileId }),
                        UploadType.Set => await mediator.Send(new SetFileExistsByNameQuery { FileName = ctx.FileId }),
                        UploadType.SourceSet => await mediator.Send(new SourceSetExistsByNameQuery { FileName = ctx.FileId }),
                        _ => false
                    };
                    if (itemExists)
                    {
                        logger.LogInformation($"File {ctx.FileId} is in use according to the DB and cannot be deleted");
                        ctx.FailRequest("File's entity is still registered in the DB so the file cannot be deleted");
                    }
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
                        new Task(() => { 
                            ZipFile.ExtractToDirectory(Path.Join(diskStorePath, ctx.FileId), diskStorePath, true);
                            logger.LogInformation($"{ctx.FileId} extracted successfully.");
                        }).Start();
                    }
                    
                    File.Delete(Path.Join(diskStorePath, ctx.FileId + ".uploadlength"));
                    File.Delete(Path.Join(diskStorePath, ctx.FileId + ".chunkstart"));
                    File.Delete(Path.Join(diskStorePath, ctx.FileId + ".chunkcomplete"));
                    File.Delete(Path.Join(diskStorePath, ctx.FileId + ".expiration"));

                    return Task.CompletedTask;
                }
            },
            // Set an expiration time, where incomplete files can no longer be updated.
            // This value can either be absolute or sliding.
            // Absolute expiration will be saved per file on create
            // Sliding expiration will be saved per file on create and updated on each patch/update.
            Expiration = new SlidingExpiration(TimeSpan.FromMinutes(5))
        };

        return Task.FromResult(config);
    }
}