using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Services;
using BenchmarkingPortal.Bll.Tus;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tusdotnet.Models;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireApprovedUser)]
public class Resources : PageModel
{
    private readonly IMediator _mediator;
    private readonly string _setFilesDir;
    private readonly string _workDir;

    public Resources(IMediator mediator, StoragePaths storagePaths)
    {
        _mediator = mediator;
        _setFilesDir = storagePaths.SetFilesDir;
        _workDir = storagePaths.WorkingDir;
        
        Executables = new List<ExecutableHeader>();
        SetFiles = new List<SetFileHeader>();
        ExecutableInput = new ExecutableInputModel();
        SetFileInput = new SetFileInputModel();
    }

    [TempData] public string? StatusMessage { get; set; }

    public List<ExecutableHeader> Executables { get; set; }
    public List<SetFileHeader> SetFiles { get; set; }
    public List<string> ExeHeaders { get; set; } = new();
    public List<string> SourceHeaders { get; set; } = new();

    [BindProperty] public ExecutableInputModel ExecutableInput { get; set; }

    [BindProperty] public SetFileInputModel SetFileInput { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ExeHeaders = new List<string>
            {
                "Name", "Owner Tool", "Uploaded Date", "Actions"
            };

            SourceHeaders = new List<string>
            {
                "Name", "Uploaded Date", "Actions"
            };

            Executables = (await _mediator.Send(new GetAllExecutablesQuery())).ToList();
            SetFiles = (await _mediator.Send(new GetAllSetFilesQuery())).ToList();

            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteExecutableAsync(int id, string name, string fileId)
    {
        try
        {
            await _mediator.Send(new DeleteExecutableCommand
            {
                ExecutableId = id,
                FileId = fileId,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Executable>().NoPrivilege)
            });
            StatusMessage = $"{name} deleted successfully.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteSetFileAsync(int id, string name, string fileId)
    {
        try
        {
            await _mediator.Send(new DeleteSetFileCommand
            {
                SetFileId = id,
                FileId = fileId,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<SetFile>().NoPrivilege)
            });
            StatusMessage = $"{name} deleted successfully.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDownloadSetFileAsync(string fileId, CancellationToken cancellationToken)
    {
        try
        {
            var store = new CustomTusDiskStore(_setFilesDir, _mediator);
            
            var file = await store.GetFileAsync(fileId, cancellationToken);

            if (file == null)
            {
                StatusMessage = $"Error: File with id {fileId} was not found.";
                return RedirectToPage();
            }

            var fileStream = await file.GetContentAsync(cancellationToken);
            var metadata = await file.GetMetadataAsync(cancellationToken);

            StatusMessage = "Download started.";

            return new FileStreamResult(fileStream, GetContentTypeOrDefault(metadata))
            {
                FileDownloadName = metadata.TryGetValue("name", out var nameMeta)
                    ? nameMeta.GetString(Encoding.UTF8)
                    : "download"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.Message;

            return RedirectToPage();
        }
    }
    
    public async Task<IActionResult> OnPostDownloadExecutableAsync(string fileId, CancellationToken cancellationToken)
    {
        try
        {
            var exe = await _mediator.Send(new GetExecutableByPathQuery
            {
                FileId = fileId
            }, cancellationToken);
            
            if (exe == null)
            {
                StatusMessage = $"Error: File with id {fileId} was not found.";
                return RedirectToPage();
            }
            
            var path = _workDir + Path.DirectorySeparatorChar + exe.UserName + Path.DirectorySeparatorChar + "tools" ;
            var store = new CustomTusDiskStore(path, _mediator);
            
            var file = await store.GetFileAsync(fileId, cancellationToken);

            if (file == null)
            {
                StatusMessage = $"Error: File with id {fileId} was not found.";
                return RedirectToPage();
            }

            var fileStream = await file.GetContentAsync(cancellationToken);
            var metadata = await file.GetMetadataAsync(cancellationToken);

            StatusMessage = "Download started.";

            return new FileStreamResult(fileStream, GetContentTypeOrDefault(metadata))
            {
                FileDownloadName = metadata.TryGetValue("name", out var nameMeta)
                    ? nameMeta.GetString(Encoding.UTF8)
                    : "download"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.Message;

            return RedirectToPage();
        }
    }

    private static string GetContentTypeOrDefault(Dictionary<string, Metadata> metadata)
    {
        return metadata.TryGetValue("contentType", out var contentType) ? contentType.GetString(Encoding.UTF8) : "application/octet-stream";
    }

    public async Task<IActionResult> OnPostUploadExecutableAsync()
    {
        try
        {
            var newExecutable = await _mediator.Send(new UploadNewExecutableCommand
            {
                Name = ExecutableInput.Name.TrimEnd(".zip".ToCharArray()),
                Version = ExecutableInput.Version,
                OwnerTool = ExecutableInput.OwnerTool,
                ToolVersion = ExecutableInput.ToolVersion,
                Path = ExecutableInput.FileUrl,
                UploadedDate = DateTime.UtcNow,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Executable>().NoPrivilege)
            });

            ExecutableInput = new ExecutableInputModel();

            StatusMessage = $"{newExecutable.Name} uploaded successfully.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            ExecutableInput = new ExecutableInputModel();

            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostUploadSetFileAsync()
    {
        try
        {
            var newSetFile = await _mediator.Send(new UploadNewSetFileCommand
            {
                Name = SetFileInput.Name.TrimEnd(".set".ToCharArray()),
                Version = SetFileInput.Version,
                Path = SetFileInput.FileUrl,
                UploadedDate = DateTime.UtcNow,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<SetFile>().NoPrivilege)
            });

            SetFileInput = new SetFileInputModel();

            StatusMessage = $"{newSetFile.Name} uploaded successfully.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            SetFileInput = new SetFileInputModel();

            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public class ExecutableInputModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._-]+:[A-Za-z0-9._-]+\.zip$")]
        [Display(Name = "Executable Name")]
        public string Name { get; init; } = null!;

        [Required]
        [Display(Name = "Executable Version")] 
        [DefaultValue("1.0")]
        public string? Version { get; init; }

        [Required]
        [Display(Name = "Owner Tool Name")]
        public string OwnerTool { get; init; } = null!;

        [Required]
        [Display(Name = "Tool Version")]
        public string ToolVersion { get; init; } = null!;

        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Executable Zip")]
        [RegularExpression(@".*\.zip$")]
        public string FileUrl { get; init; } = null!;
    }

    public class SetFileInputModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._-]+:[A-Za-z0-9._-]+\.set$")]
        [Display(Name = "Set File Name")]
        public string Name { get; init; } = null!;
        
        [Required]
        [Display(Name = "Set File Version")] 
        [DefaultValue("1.0")]
        public string? Version { get; init; }
        

        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Set File")]
        [RegularExpression(@".*\.set$")]
        public string FileUrl { get; init; } = null!;
    }
}