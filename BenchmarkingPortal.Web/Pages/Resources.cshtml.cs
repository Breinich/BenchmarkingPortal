using System.ComponentModel.DataAnnotations;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Executable.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SourceSet.Commands;
using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireApprovedUser)]
public class Resources : PageModel
{
    [TempData]
    public string? StatusMessage { get; set; }
    public List<ExecutableHeader> Executables { get; set; }
    public List<SourceSetHeader> SourceSets { get; set; }

    private readonly IMediator _mediator;
    
    [BindProperty]
    public ExecutableInputModel ExecutableInput { get; set; }
    public class ExecutableInputModel
    {
        [Required]
        [RegularExpression(@"^[^\\/?%*:|""<>\.]+$")]
        [Display(Name = "Executable Name")]
        public string Name { get; set; } = null!;
        
        [Display(Name = "Executable Version")]
        public string? Version { get; set; } = null!;
        
        [Required]
        [Display(Name = "Owner Tool Name")]
        public string OwnerTool { get; set; } = null!;
        
        [Required]
        [Display(Name = "Tool Version")]
        public string ToolVersion { get; set; } = null!;
    }
    
    [BindProperty]
    public SourceSetInputModel SourceSetInput { get; set; }
    
    public class SourceSetInputModel
    {
        [Required]
        [RegularExpression(@"^[^\\/?%*:|""<>\.]+$")]
        [Display(Name = "Source Set Name")]
        public string Name { get; set; } = null!;
        
        [Display(Name = "Source Set Version")]
        public string? Version { get; set; } = null!;
    }
    
    public Resources(IMediator mediator)
    {
        _mediator = mediator;
        Executables = new List<ExecutableHeader>();
        SourceSets = new List<SourceSetHeader>();
        
        ExecutableInput = new ExecutableInputModel();
        SourceSetInput = new SourceSetInputModel();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Executables = (await _mediator.Send(new GetAllExecutablesQuery())).ToList();
            SourceSets = (await _mediator.Send(new GetAllSourceSetsQuery())).ToList();
            
            return Page();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteExecutableAsync(int id, string name)
    {
        try
        {
            await _mediator.Send(new DeleteExecutableCommand()
            {
                ExecutableId = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Executable>().NoPrivilege)
            });
            StatusMessage = $"{name} deleted successfully.";
            
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
        
    }
    
    public async Task<IActionResult> OnPostDeleteSourceSetAsync(int id, string name)
    {
        try
        {
            await _mediator.Send(new DeleteSourceSetCommand()
            {
                SourceSetId = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<SourceSet>().NoPrivilege)
            });
            StatusMessage = $"{name} deleted successfully.";
            
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
        
    }

    public IActionResult OnPostDownload(string path)
    {
        try
        {
            StatusMessage = "Download started.";

            return RedirectToPage();
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
            var path = "\\executables\\" + ExecutableInput.Name + "." + (ExecutableInput.Version ??= "1.0");
            
            var newExecutable = await _mediator.Send(new UploadNewExecutableCommand()
            {
                Name = ExecutableInput.Name,
                Version = ExecutableInput.Version,
                OwnerTool = ExecutableInput.OwnerTool,
                ToolVersion = ExecutableInput.ToolVersion,
                Path = path,
                UploadedDate = DateTime.Now,
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
    
    public async Task<IActionResult> OnPostUploadSourceSetAsync()
    {
        try
        {
            var path = "\\sourcesets\\" + SourceSetInput.Name + "." + SourceSetInput.Version;
            var newSourceSet = await _mediator.Send(new UploadNewSourceSetCommand()
            {
                Name = SourceSetInput.Name,
                Version = SourceSetInput.Version,
                Path = path,
                UploadedDate = DateTime.Now,
                InvokerName = User.Identity?.Name ?? 
                              throw new ApplicationException(new ExceptionMessage<SourceSet>().NoPrivilege)
            });
            
            SourceSetInput = new SourceSetInputModel();
            
            StatusMessage = $"{newSourceSet.Name} uploaded successfully.";
            
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            SourceSetInput = new SourceSetInputModel();
            
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;
            
            return RedirectToPage();
        }
    }
}