namespace BenchmarkingPortal.Web;

public class TusDiskStorageOptionHelper
{
    public TusDiskStorageOptionHelper()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "App_Data", "tusfiles");
        if (!File.Exists(path))
            Directory.CreateDirectory(path);

        StorageDiskPath = path;
    }

    public string StorageDiskPath { get; }
}