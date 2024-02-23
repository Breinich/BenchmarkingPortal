using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Features.UploadedFile.Commands;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireApprovedUser)]
public class Resources : PageModel
{
    private readonly IMediator _mediator;

    public Resources(IMediator mediator)
    {
        _mediator = mediator;
        
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
            var (fileStream, contentType, fileName) = await _mediator.Send(new DownloadUploadedFileCommand
            {
                FileId = fileId
            }, cancellationToken);
            
            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileName
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostUploadExecutableAsync()
    {
        try
        {
            var newExecutable = await _mediator.Send(new UploadNewExecutableCommand
            {
                Name = Path.ChangeExtension(ExecutableInput.Name, null),
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
                Name = Path.ChangeExtension(SetFileInput.Name, null),
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
        [RegularExpression(@"[A-Za-z0-9._-]+_[A-Za-z0-9._-]+\.zip$")]
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
        [RegularExpression(@"[A-Za-z0-9._-]+_[A-Za-z0-9._-]+\.set$")]
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