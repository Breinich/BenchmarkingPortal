using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BenchmarkingPortal.Bll.Services;

public class VCloudCommandExecutor : ICommandExecutor
{
    private Process? _terminal;
    private string _ivyJarName = "ivy-2.5.0.jar";
    private string? _ivyPath;
    private string? _ivyDownloadUrl;
    private string? _vcloudDirAsync;
    
    private readonly ILogger<VCloudCommandExecutor> _logger;

    public VCloudCommandExecutor(PathConfigs pathConfigs, ILogger<VCloudCommandExecutor> logger)
    {
        _logger = logger;
        
        _ivyPath = Path.Combine(pathConfigs.VcloudDirectory, "lib", _ivyJarName);
        _ivyDownloadUrl = "https://www.sosy-lab.org/ivy/org.apache.ivy/ivy/" + _ivyJarName;
        _vcloudDirAsync = pathConfigs.VcloudDirectory;
    }

    public async Task InitializeAsync()
    {
        var workingDir = Path.Combine(_vcloudDirAsync!, "lib", "vcloud-jars");
        if (!File.Exists(Path.Combine(workingDir, "vcloud.jar")))
            await DownloadRequiredJarsAsync(CancellationToken.None);
        
        _terminal = new Process
        {
            StartInfo =
            {
                WorkingDirectory = workingDir,
                FileName = "java",
                Arguments = "-jar vcloud.jar client",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
            EnableRaisingEvents = true
        };

        _terminal.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _logger.LogInformation($"Output: {e.Data}");
            }
        };

        _terminal.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _logger.LogError($"Error: {e.Data}");
            }
        };

        _terminal.Exited += (_, _) =>
        {
            _logger.LogInformation("VCloud process exited.");
        };

        _terminal!.Start();

        _terminal.BeginOutputReadLine();
        _terminal.BeginErrorReadLine();

        await _terminal.StandardInput.WriteLineAsync("auth admin");
        await _terminal.StandardInput.WriteLineAsync("run trivial");
        
        _logger.LogInformation("VCloud tested.");
    }
    
    public async Task<string> ExecuteAsync(string command, IEnumerable<string> args)
    {
        if (_terminal == null)
            await InitializeAsync();
        
        var result = "";
        
        var gatherer = new DataReceivedEventHandler((_, eventArgs) =>
        {
            if (!string.IsNullOrEmpty(eventArgs.Data))
            {
                result += eventArgs.Data + "\n";
            }
        });

        _terminal!.OutputDataReceived += gatherer;
        _terminal.ErrorDataReceived += gatherer;
        
        await _terminal!.StandardInput.WriteAsync(command);
        foreach (var arg in args)
        {
            await _terminal.StandardInput.WriteAsync(" " + arg);
        }
        await _terminal.StandardInput.WriteLineAsync();

        // give n seconds to the vcloud client to execute the given command, until then we will gather the output
        await Task.Delay(5000);

        _terminal.OutputDataReceived -= gatherer;
        _terminal.ErrorDataReceived -= gatherer;

        return result;
    }

    public void Dispose()
    {
        if (_terminal == null) return;
        if (_terminal.HasExited) return;
        
        _terminal.StandardInput.WriteLine("exit");
        _terminal.WaitForExit(2000);
        _terminal.Close();
        
        _terminal.Dispose();
    }

    /// <summary>
    /// Downloads the required jars for vcloud to work using ivy.
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async Task DownloadRequiredJarsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Downloading required jars...");
        
        if (!File.Exists(_ivyPath))
        {
            using var client = new HttpClient();
            await using var s = await client.GetStreamAsync(_ivyDownloadUrl, cancellationToken);
            await using var fs = new FileStream(_ivyPath!, FileMode.OpenOrCreate);
            await s.CopyToAsync(fs, cancellationToken);
        }

        var downloading = new Process
        {
            StartInfo =
            {
                WorkingDirectory = _vcloudDirAsync,
                FileName = "java",
                ArgumentList = {"-jar", "lib/" + _ivyJarName, 
                    "-settings", "lib/ivysettings.xml", 
                    "-dependency", "org.sosy_lab", "vcloud", "0.+", 
                    "-confs", "runtime", 
                    "-mode", "dynamic", 
                    "-refresh", 
                    "-warn", 
                    "-retrieve", "lib/vcloud-jars/[artifact](-[classifier]).[ext]", 
                    "-overwriteMode", "different"},
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        downloading.Start();
        await downloading.WaitForExitAsync(cancellationToken);
    }
}