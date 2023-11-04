using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SetFile.Queries;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireApprovedUser)]
public class Home : PageModel
{
    private readonly IMediator _mediator;

    private readonly string _tempConfigDataListKey = "TempConfigDataList";
    private readonly string _tempConstraintDataListKey = "TempConstraintDataList";

    public Home(IMediator mediator)
    {
        _mediator = mediator;
    }

    [TempData] public string? StatusMessage { get; set; }

    [BindProperty] public EditInputModel EditInput { get; set; } = default!;

    [BindProperty] public CreateInputModel CreateInput { get; set; } = default!;

    public List<BenchmarkHeader> UnfinishedBenchmarks { get; set; } = new();
    public List<SelectListItem> Executables { get; set; } = new();
    public List<SelectListItem> SetFiles { get; set; } = new();
    public List<SelectListItem> ComputerGroups { get; set; } = new();
    public List<string> Headers { get; set; } = new();


    public async Task<IActionResult> OnGet()
    {
        try
        {
            OnPostDeleteSession();

            Headers = new List<string>
            {
                "Name", "Started", "Status", "Priority", "RAM", "CPU", "CPU model", "Executable", "Set File", "Progress",
                "Actions"
            };

            UnfinishedBenchmarks = (await _mediator.Send(new GetAllBenchmarksQuery
            {
                Finished = false
            })).ToList();

            await LoadFormData();

            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public ContentResult OnPostAddConfigItem(Scope scope, string? key, string? value)
    {
        if (key == null || value == null || key == "" || value == "")
            return new ContentResult
            {
                Content = "Key or value is empty.",
                ContentType = "application/json"
            };

        var configurationItems = HttpContext.Session
                                     .GetComplexData<List<TempConfigData>>(_tempConfigDataListKey) ??
                                 new List<TempConfigData>();

        var newItem = new TempConfigData
        {
            Scope = scope,
            Key = key,
            Value = value
        };
        var different = true;
        foreach (var item in configurationItems)
            if (!item.CheckDifference(newItem))
            {
                different = false;
                break;
            }

        if (!different)
            return new ContentResult
            {
                Content = "Item already exists.",
                ContentType = "application/json"
            };

        configurationItems.Add(newItem);

        HttpContext.Session.SetComplexData(_tempConfigDataListKey, configurationItems);

        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(new { Key = key, Value = value }),
            ContentType = "application/json"
        };
    }

    public JsonResult OnPostDeleteConfigItem(Scope scope, string key, string value)
    {
        var configurationItems = HttpContext.Session
                                     .GetComplexData<List<TempConfigData>>(_tempConfigDataListKey) ??
                                 new List<TempConfigData>();

        if (configurationItems.Remove(new TempConfigData
            {
                Scope = scope,
                Key = key,
                Value = value
            }))
        {
            HttpContext.Session.SetComplexData(_tempConfigDataListKey, configurationItems);

            return new JsonResult(new { success = true, responseText = "Item removed." });
        }

        return new JsonResult(new { success = false, responseText = "Item not found." });
    }

    public ContentResult OnPostAddConstraint(string? premise, string? consequence)
    {
        if (premise == null || consequence == null || premise == "" || consequence == "")
            return new ContentResult
            {
                Content = "Premise or consequence is empty.",
                ContentType = "application/json"
            };

        var constraintItems = HttpContext.Session
                                  .GetComplexData<List<TempConstraintData>>(_tempConstraintDataListKey) ??
                              new List<TempConstraintData>();

        var newConstraint = new TempConstraintData
        {
            Premise = premise,
            Consequence = consequence
        };
        var different = true;
        foreach (var item in constraintItems)
            if (!item.CheckDifference(newConstraint))
            {
                different = false;
                break;
            }

        if (!different)
            return new ContentResult
            {
                Content = "Item already exists.",
                ContentType = "application/json"
            };

        constraintItems.Add(newConstraint);

        HttpContext.Session.SetComplexData(_tempConstraintDataListKey, constraintItems);

        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(new { Premise = premise, Consequence = consequence }),
            ContentType = "application/json"
        };
    }

    public JsonResult OnPostDeleteConstraint(string premise, string consequence)
    {
        var constraintItems = HttpContext.Session
                                  .GetComplexData<List<TempConstraintData>>(_tempConstraintDataListKey) ??
                              new List<TempConstraintData>();

        if (constraintItems.Remove(new TempConstraintData
            {
                Premise = premise,
                Consequence = consequence
            }))
        {
            HttpContext.Session.SetComplexData(_tempConstraintDataListKey, constraintItems);

            return new JsonResult(new { success = true, responseText = "Item removed." });
        }

        return new JsonResult(new { success = false, responseText = "Item not found." });
    }

    public async Task<IActionResult> OnPostPauseAsync(int id)
    {
        try
        {
            var benchmark = await _mediator.Send(new UpdateBenchmarkCommand
            {
                Id = id,
                Status = Status.Paused,
                Priority = EditInput.Priority,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = $"Benchmark {benchmark.Name} paused.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostResumeAsync(int id)
    {
        try
        {
            var benchmark = await _mediator.Send(new UpdateBenchmarkCommand
            {
                Id = id,
                Status = Status.Running,
                Priority = EditInput.Priority,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = $"Benchmark {benchmark.Name} resumed.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostSaveAsync(int id, Status status)
    {
        try
        {
            var benchmark = await _mediator.Send(new UpdateBenchmarkCommand
            {
                Id = id,
                Status = status,
                Priority = EditInput.Priority,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = $"Benchmark {benchmark.Name} saved.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, string name)
    {
        try
        {
            await _mediator.Send(new DeleteBenchmarkCommand
            {
                Id = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = $"Benchmark {name} deleted.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public IActionResult OnPostConnectAsync(int id)
    {
        try
        {
            StatusMessage = "Connected to the benchmark's console.";

            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostStartAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var configs = HttpContext.Session
                .GetComplexData<List<TempConfigData>>(_tempConfigDataListKey);
            
            var constraints = HttpContext.Session
                .GetComplexData<List<TempConstraintData>>(_tempConstraintDataListKey);

            HttpContext.Session.Remove(_tempConfigDataListKey);
            HttpContext.Session.Remove(_tempConstraintDataListKey);

            List<(Scope, string, string)>? configList = null;
            List<(string, string)>? constraintList = null;

            if (configs != null)
                configList = configs.Select(c => (c.Scope, c.Key, c.Value)).ToList();

            if (constraints != null)
                constraintList = constraints.Select(c => (c.Premise, c.Consequence)).ToList();

            var config = await _mediator.Send(new CreateConfigurationCommand
            {
                Configurations = configList,
                Constraints = constraintList
            });

            var benchmark = await _mediator.Send(new StartBenchmarkCommand
            {
                Name = CreateInput.Name,
                ExecutableId = CreateInput.ExecutableId,
                SetFilePath = CreateInput.SetFilePath,
                PropertyFilePath = CreateInput.PropertyFilePath,
                Priority = CreateInput.Priority,
                Ram = CreateInput.Ram,
                Cpu = CreateInput.Cpu,
                TimeLimit = CreateInput.TimeLimit,
                HardTimeLimit = CreateInput.HardTimeLimit,
                CpuModel = CreateInput.CpuModel,
                ConfigurationId = config.Id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = $"Benchmark {benchmark.Name} created.";

            return RedirectToPage();
        }
        catch (Exception e)
        {
            HttpContext.Session.Remove(_tempConfigDataListKey);
            HttpContext.Session.Remove(_tempConstraintDataListKey);

            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    private async Task LoadFormData()
    {
        try
        {
            Executables = (await _mediator.Send(new GetAllExecutablesQuery()))
                .Select(eh => new SelectListItem(eh.Name + ":" + eh.Version, eh.Id.ToString())).ToList();

            SetFiles = (await _mediator.Send(new GetAllSetFilesQuery()))
                .Select(sh => new SelectListItem(sh.Name + ":" + sh.Version, sh.Id.ToString())).ToList();

            ComputerGroups = (await _mediator.Send(new GetAllComputerGroupsQuery()))
                .Select(c => new SelectListItem(c.Id + ": " + c.Description, c.Id.ToString())).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            StatusMessage = "Error: " + (ex.InnerException ?? ex).Message;
        }
    }

    public JsonResult OnPostDeleteSession()
    {
        HttpContext.Session.Remove(_tempConstraintDataListKey);
        HttpContext.Session.Remove(_tempConfigDataListKey);

        return new JsonResult(new { success = true, responseText = "Session cleared." });
    }

    public class EditInputModel
    {
        [Display(Name = "Benchmark Name")] public int Priority { get; set; }
    }

    public class CreateInputModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Benchmark Name")]
        public string Name { get; set; } = null!;

        [Required]
        [DefaultValue(0)]
        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "RAM (GB)")]
        public int Ram { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "CPU (cores)")]
        public int Cpu { get; set; }

        [Range(1, 100000)]
        [Display(Name = "Time Limit (s)")]
        public int TimeLimit { get; set; }

        [Range(1, 100000)]
        [Display(Name = "Hard Time Limit (s)")]
        public int HardTimeLimit { get; set; }

        [Required]
        [Display(Name = "Executable")]
        public int ExecutableId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Set File Path")]
        public string SetFilePath { get; set; } = null!;

        [Required]
        [StringLength(250)]
        [Display(Name = "Property File Path")]
        public string PropertyFilePath { get; set; } = null!;

        [Display(Name = "Computer Group to run on")]
        public int ComputerGroupId { get; set; }
        
        [DefaultValue("-")]
        [StringLength(50)]
        [Display(Name = "CPU Model to run on")]
        public string CpuModel { get; set; } = null!;
    }

    public class TempConfigData
    {
        public Scope Scope { get; init; }
        public string Key { get; init; } = null!;
        public string Value { get; init; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (TempConfigData)obj;
            return Scope == other.Scope && Key == other.Key && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Scope, Key, Value);
        }

        public bool CheckDifference(TempConfigData configData)
        {
            switch (configData.Scope)
            {
                case Scope.Global:
                    return configData.Key != Key;
                case Scope.Local:
                    return configData.Key != Key || configData.Value != Value;
                default:
                    return false;
            }
        }
    }

    public class TempConstraintData
    {
        public string Premise { get; init; } = null!;
        public string Consequence { get; init; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (TempConstraintData)obj;
            return Premise == other.Premise && Consequence == other.Consequence;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Premise, Consequence);
        }

        public bool CheckDifference(TempConstraintData constraintData)
        {
            return constraintData.Premise != Premise || constraintData.Consequence != Consequence;
        }
    }
}