﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Bll.Features.CpuModel.Queries;
using BenchmarkingPortal.Bll.Features.Worker.Commands;
using BenchmarkingPortal.Bll.Features.Worker.Queries;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireAdministratorRole)]
public class Workers : PageModel
{
    private readonly IMediator _mediator;

    public Workers(IMediator mediator)
    {
        _mediator = mediator;
        WorkerWorkerInput = new WorkerInputModel();
        ComputerGroupInput = new ComputerGroupInputModel();
    }

    [TempData] public string StatusMessage { get; set; } = null!;
    [BindProperty] public WorkerInputModel WorkerWorkerInput { get; init; }
    [BindProperty] public ComputerGroupInputModel ComputerGroupInput { get; init; }
    [BindProperty] public int ChangeComputerGroupId { get; init; }
    [BindProperty] public string? ChangeDescription { get; init; }
    [BindProperty] public int ChangeId { get; init; }

    public List<WorkerHeader> WorkerList { get; set; } = new();
    public List<ComputerGroupHeader> ComputerGroupList { get; set; } = new();
    public List<CpuModelHeader> CpuModelList { get; set; } = new();
    public List<SelectListItem> ComputerGroupSelectList { get; set; } = new();
    public List<string> WorkerModelHeaders { get; set; } = new();
    public List<string> CompGroupModelHeaders { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            WorkerModelHeaders = new List<string>
            {
                "Name", "Address", "CPU Model", "Computer Group", "Actions"
            };

            CompGroupModelHeaders = new List<string>
            {
                "Id", "Description", "Workers", "Benchmarks", "Actions"
            };

            WorkerList = (await _mediator.Send(new GetAllWorkersQuery())).ToList();

            ComputerGroupList = (await _mediator.Send(new GetAllComputerGroupsQuery())).ToList();

            CpuModelList = (await _mediator.Send(new GetAllCpuModelsQuery())).ToList();

            ComputerGroupSelectList = ComputerGroupList.Select(x => new SelectListItem
            {
                Text = x.Id.ToString(),
                Value = x.Id.ToString()
            }).ToList();

            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostSaveWorkerAsync(int id)
    {
        try
        {
            var result = await _mediator.Send(new UpdateWorkerCommand
            {
                WorkerId = id,
                ComputerGroupId = ChangeComputerGroupId
            });

            StatusMessage = $"Worker {result.Id} updated successfully";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteWorkerAsync(int id)
    {
        try
        {
            await _mediator.Send(new RemoveWorkerCommand
            {
                WorkerId = id
            });

            StatusMessage = $"Worker {id} deleted successfully";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostAddWorkerAsync()
    {
        try
        {
            var result = await _mediator.Send(new AddWorkerCommand
            {
                Name = WorkerWorkerInput.Name,
                Username = WorkerWorkerInput.Username,
                Password = WorkerWorkerInput.Password,
                Address = WorkerWorkerInput.Address,
                Port = WorkerWorkerInput.Port,
                ComputerGroupId = WorkerWorkerInput.ComputerGroupId,
                AddedDate = DateTime.UtcNow,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<Worker>.NoPrivilege)
            });

            StatusMessage = $"Worker with id:{result.Id} added successfully";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostAddComputerGroupAsync()
    {
        try
        {
            var result = await _mediator.Send(new CreateComputerGroupCommand
            {
                Description = ComputerGroupInput.Description
            });

            StatusMessage = $"Computer Group with id:{result.Id} added successfully";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteComputerGroupAsync(int id)
    {
        try
        {
            await _mediator.Send(new DeleteComputerGroupCommand
            {
                Id = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<ComputerGroup>.NoPrivilege)
            });

            StatusMessage = $"Computer Group {id} deleted successfully";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostEditComputerGroupAsync()
    {
        try
        {
            await _mediator.Send(new UpdateComputerGroupCommand
            {
                Id = ChangeId,
                Description = ChangeDescription,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<ComputerGroup>.NoPrivilege)
            });

            StatusMessage = $"Computer {ChangeId} Group edited successfully";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public class WorkerInputModel
    {
        [Required] 
        [DisplayName("Name")] 
        [RegularExpression("[A-Za-z0-9]+$")]
        public string Name { get; init; } = null!;

        [Required]
        [DisplayName("Host name or IP address")]
        public string Address { get; init; } = null!;

        [Required]
        [DisplayName("Port Number")]
        [Range(1, 65535)]
        public int Port { get; init; }

        [Required]
        [DisplayName("Username for the VM")]
        public string Username { get; init; } = null!;

        [Required]
        [DisplayName("Password for the VM")]
        [DataType(DataType.Password)]
        public string Password { get; init; } = null!;

        [Required]
        [DisplayName("Computer Group to add to")]
        public int ComputerGroupId { get; init; }
    }

    public class ComputerGroupInputModel
    {
        [Required]
        [DisplayName("Description")]
        public string Description { get; init; } = null!;
    }
}