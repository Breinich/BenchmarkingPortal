using System.ComponentModel.DataAnnotations;
using System.Text;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tusdotnet.Interfaces;
using tusdotnet.Models;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireApprovedUser)]
public class Resources : PageModel
{
    private readonly DefaultTusConfiguration _config;
    private readonly IMediator _mediator;

    public Resources(IMediator mediator, DefaultTusConfiguration config)
    {
        _mediator = mediator;
        _config = config;
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

    public async Task<IActionResult> OnPostDownloadAsync(string fileId, CancellationToken cancellationToken)
    {
        try
        {
            if (_config.Store is not ITusReadableStore store) return RedirectToPage();

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
        if (metadata.TryGetValue("contentType", out var contentType)) return contentType.GetString(Encoding.UTF8);

        return "application/octet-stream";
    }

    public async Task<IActionResult> OnPostUploadExecutableAsync()
    {
        try
        {
            var newExecutable = await _mediator.Send(new UploadNewExecutableCommand
            {
                Name = ExecutableInput.Name,
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
                Name = SetFileInput.Name,
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
        [RegularExpression(@"^[^\\/?%*:|""<>\.]+$")]
        [Display(Name = "Executable Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Executable Version")] public string? Version { get; set; }

        [Required]
        [Display(Name = "Owner Tool Name")]
        public string OwnerTool { get; set; } = null!;

        [Required]
        [Display(Name = "Tool Version")]
        public string ToolVersion { get; set; } = null!;

        [Required(ErrorMessage = "Please select a file.")]
        public string FileUrl { get; set; } = null!;
    }

    public class SetFileInputModel
    {
        [Required]
        [RegularExpression(@"^[^\\/?%*:|""<>\.]+$")]
        [Display(Name = "Source Set Name")]
        public string Name { get; set; } = null!;
        

        [Display(Name = "Source Set Version")] 
        public string? Version { get; set; }
        

        [Required(ErrorMessage = "Please select a file.")]
        public string FileUrl { get; set; } = null!;
    }
}