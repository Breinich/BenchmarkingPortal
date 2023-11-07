using System.Text;
using BenchmarkingPortal.Bll.Tus;
using MediatR;
using tusdotnet.Models;

namespace BenchmarkingPortal.Web.Endpoints;

public class DownloadFileEndpoint
{
    public static async Task HandleRoute(HttpContext context, IConfiguration configuration, IMediator mediator)
    {
        var fileId = (string)(context.Request.RouteValues["fileId"] ?? 
                     throw new ApplicationException("Missing fileId from route")); 
        
        string path;
        switch(fileId.Split(".").Last()){
            case "set":
                path = (configuration["Storage:SV-Benchmarks"] ?? 
                        throw new ApplicationException("Missing SV-Benchmark path configuration")) 
                       + Path.DirectorySeparatorChar + "c";
                break;
            case "zip":
                path = (configuration["Storage:Root"] ?? 
                        throw new ApplicationException("Missing root path configuration")) 
                       + Path.DirectorySeparatorChar + context.User.Identity?.Name;
                break;
            default:
                throw new ArgumentException("Invalid file extension.");
        }
        
        var store = new CustomTusDiskStore(path, mediator);
        
        var file = await store.GetFileAsync(fileId, context.RequestAborted);

        if (file == null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync($"File with id {fileId} was not found.", context.RequestAborted);
            return;
        }

        var fileStream = await file.GetContentAsync(context.RequestAborted);
        var metadata = await file.GetMetadataAsync(context.RequestAborted);

        context.Response.ContentType = GetContentTypeOrDefault(metadata);
        context.Response.ContentLength = fileStream.Length;

        if (metadata.TryGetValue("name", out var nameMeta))
            context.Response.Headers.Add("Content-Disposition",
                new[] { $"attachment; filename=\"{nameMeta.GetString(Encoding.UTF8)}\"" });

        await using (fileStream)
        {
            await fileStream.CopyToAsync(context.Response.Body, 81920, context.RequestAborted);
        }
    }

    private static string GetContentTypeOrDefault(Dictionary<string, Metadata> metadata)
    {
        if (metadata.TryGetValue("contentType", out var contentType)) return contentType.GetString(Encoding.UTF8);

        return "application/octet-stream";
    }
}