using System.Text;
using tusdotnet.Models;

namespace BenchmarkingPortal.Web.Pages;

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
}