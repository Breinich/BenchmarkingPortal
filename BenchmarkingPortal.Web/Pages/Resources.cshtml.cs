using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Commands;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Bll.Features.UploadedFile.Commands;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

/// <summary>
/// The resources page model.
/// </summary>
/// <param name="mediator">The mediator.</param>
[Authorize(Policy = Policies.RequireApprovedUser)]
public class Resources(IMediator mediator) : PageModel
{
    /// <summary>
    /// The status message.
    /// </summary>
    [TempData] public string? StatusMessage { get; set; }

    /// <summary>
    /// List of executables.
    /// </summary>
    public List<ExecutableHeader> Executables { get; set; } = [];
    
    /// <summary>
    /// Dictionary of set files by source set id.
    /// </summary>
    public Dictionary<int, List<string>> SetFiles { get; set; } = new();
    
    /// <summary>
    /// List of source sets.
    /// </summary>
    public List<SourceSetHeader> SourceSets { get; set; } = [];
    
    /// <summary>
    /// List of executable table headers.
    /// </summary>
    public List<string> ExeHeaders { get; set; } = [];
    
    /// <summary>
    /// List of source set table headers.
    /// </summary>
    public List<string> SourceHeaders { get; set; } = [];
    
    /// <summary>
    /// List of set file table headers.
    /// </summary>
    public List<string> SetFileHeaders { get; set; } = [];

    /// <summary>
    /// The executable input model.
    /// </summary>
    [BindProperty] public ExecutableInputModel ExecutableInput { get; set; } = new();
    
    /// <summary>
    /// The set file input model.
    /// </summary>
    [BindProperty] public SetFileInputModel SetFileInput { get; set; } = new();
    
    /// <summary>
    /// The source set input model.
    /// </summary>
    [BindProperty] public SourceSetInputModel SourceSetInput { get; set; } = new();

    /// <summary>
    /// Gets all executables, set files, and source sets.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ExeHeaders = ["Name", "Owner Tool", "Uploaded Date", "Actions"];

            SourceHeaders = ["Name", "Uploaded Date", "Actions"];

            SetFileHeaders = ["File Name"];

            Executables = (await mediator.Send(new GetAllExecutablesQuery())).ToList();
            
            SourceSets = (await mediator.Send(new GetAllSourceSetsQuery())).ToList();
            foreach (var set in SourceSets)
            {
                if(!SetFiles.ContainsKey(set.Id))
                {
                    SetFiles[set.Id] = [];
                }
                SetFiles[set.Id] = (await mediator.Send( new GetSetFileNamesBySourceSetIdQuery
                {
                    SourceSetId = set.Id,
                })).Select(f => Path.GetFileName(f)).ToList();
            }

            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    /// <summary>
    /// Deletes an executable.
    /// </summary>
    /// <param name="id">The executable id.</param>
    /// <param name="name">The executable name.</param>
    /// <param name="fileId">The file id.</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">If the user is not authenticated.</exception>
    public async Task<IActionResult> OnPostDeleteExecutableAsync(int id, string name, string fileId)
    {
        try
        {
            await mediator.Send(new DeleteExecutableCommand
            {
                ExecutableId = id,
                FileId = fileId,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<Executable>.NoPrivilege)
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

    /// <summary>
    /// Deletes a set file.
    /// </summary>
    /// <param name="id">The source set id.</param>
    /// <param name="name">The set file name.</param>
    /// <param name="fileId">The file id.</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">If the user is not authenticated.</exception>
    public async Task<IActionResult> OnPostDeleteSourceSetAsync(int id, string name, string fileId)
    {
        try
        {
            await mediator.Send(new DeleteSourceSetCommand
            {
                Id = id,
                FileId = fileId,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<SetFile>.NoPrivilege)
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
    
    /// <summary>
    /// Downloads a file.
    /// </summary>
    /// <param name="fileId">The file id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>File stream result.</returns>
    public async Task<IActionResult> OnPostDownloadAsync(string fileId, CancellationToken cancellationToken)
    {
        try
        {
            var (fileStream, contentType, fileName) = await mediator.Send(new DownloadUploadedFileCommand
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

    /// <summary>
    /// Uploads a new executable.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException">If the user is not authenticated.</exception>
    public async Task<IActionResult> OnPostUploadExecutableAsync()
    {
        try
        {
            var newExecutable = await mediator.Send(new UploadNewExecutableCommand
            {
                Name = Path.ChangeExtension(ExecutableInput.Name, null),
                Version = ExecutableInput.Version,
                OwnerTool = ExecutableInput.OwnerTool,
                ToolVersion = ExecutableInput.ToolVersion,
                Path = ExecutableInput.FileUrl,
                UploadedDate = DateTime.UtcNow,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<Executable>.NoPrivilege)
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

    /// <summary>
    /// Uploads a new set file.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException">If the user is not authenticated.</exception>
    public async Task<IActionResult> OnPostUploadSetFileAsync()
    {
        try
        {
            var newSetFile = await mediator.Send(new UploadNewSetFileCommand
            {
                Name = Path.ChangeExtension(SetFileInput.Name, null),
                Path = SetFileInput.FileUrl,
                SourceSetId = SetFileInput.SourceSetId,
                UploadedDate = DateTime.UtcNow,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<SetFile>.NoPrivilege)
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
    
    /// <summary>
    /// Uploads a new source set.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException">If the user is not authenticated.</exception>
    public async Task<IActionResult> OnPostUploadSourceSetAsync()
    {
        try
        {
            var newSourceSet = await mediator.Send(new UploadNewSourceSetCommand
            {
                Name = Path.ChangeExtension(SourceSetInput.Name, null),
                Path = SourceSetInput.FileUrl,
                UploadedDate = DateTime.UtcNow,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<SourceSet>.NoPrivilege)
            });

            SourceSetInput = new SourceSetInputModel();

            StatusMessage = $"{newSourceSet.Name} uploaded successfully.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            SourceSetInput = new SourceSetInputModel();

            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    /// <summary>
    /// The executable input model.
    /// </summary>
    public class ExecutableInputModel
    {
        /// <summary>
        /// The executable name.
        /// </summary>
        [Required]
        [RegularExpression(@"[A-Za-z0-9._-]+_[A-Za-z0-9._-]+\.zip$")]
        [Display(Name = "Executable Name")]
        public string Name { get; init; } = null!;

        /// <summary>
        /// The executable version.
        /// </summary>
        [Required]
        [Display(Name = "Executable Version")] 
        [DefaultValue("1.0")]
        public string? Version { get; init; }

        /// <summary>
        /// The owner tool name.
        /// </summary>
        [Required]
        [Display(Name = "Owner Tool Name")]
        public string OwnerTool { get; init; } = null!;

        /// <summary>
        /// The owner tool version.
        /// </summary>
        [Required]
        [Display(Name = "Tool Version")]
        public string ToolVersion { get; init; } = null!;

        /// <summary>
        /// The executable zip filename.
        /// </summary>
        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Executable Zip")]
        [RegularExpression(@".*\.zip$")]
        public string FileUrl { get; init; } = null!;
    }

    /// <summary>
    /// The set file input model.
    /// </summary>
    public class SetFileInputModel
    {
        /// <summary>
        /// The set file name.
        /// </summary>
        [Required]
        [RegularExpression(@"[A-Za-z0-9._-]+_[A-Za-z0-9._-]+\.set$")]
        [Display(Name = "Set File Name")]
        public string Name { get; init; } = null!;
        
        /// <summary>
        /// The set file's set filename.
        /// </summary>
        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Set File")]
        [RegularExpression(@".*\.set$")]
        public string FileUrl { get; init; } = null!;

        /// <summary>
        /// The source set id.
        /// </summary>
        [Required]
        public int SourceSetId { get; init; }
    }

    /// <summary>
    /// The source set input model.
    /// </summary>
    public class SourceSetInputModel
    {
        /// <summary>
        /// The source set name.
        /// </summary>
        [Required]
        [RegularExpression(@"[A-Za-z0-9._-]+_[A-Za-z0-9._-]+\.zip$")]
        [Display(Name = "Source Set Name")]
        public string Name { get; init; } = null!;
        
        /// <summary>
        /// The source set's zip filename.
        /// </summary>
        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Source Set Zip")]
        [RegularExpression(@".*\.zip$")]
        public string FileUrl { get; init; } = null!;
    }
}