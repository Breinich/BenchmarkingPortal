using System.ComponentModel.DataAnnotations;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Queries;
using BenchmarkingPortal.Bll.Features.Configuration.Commands;
using BenchmarkingPortal.Bll.Features.CpuModel.Queries;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.PropertyFile.Queries;
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

    [BindProperty] public EditInputModel EditInput { get; init; } = default!;

    [BindProperty] public CreateInputModel CreateInput { get; init; } = new();

    public List<BenchmarkHeader> UnfinishedBenchmarks { get; set; } = new();
    public List<CpuModelHeader> CpuModels { get; set; } = new();
    
    public List<SelectListItem> Priorities { get; set; } = new();
    public List<SelectListItem> Executables { get; set; } = new();
    public List<SelectListItem> SetFiles { get; set; } = new();
    public List<SelectListItem> PrpFiles { get; set; } = new();
    public List<SelectListItem> ComputerGroups { get; set; } = new();
    public List<SelectListItem> CpuModelValues { get; set; } = new();
    public List<string> Headers { get; set; } = new();

    /// <summary>
    /// OnGet method for the Home page.
    /// </summary>
    /// <returns> Page </returns>
    public async Task<IActionResult> OnGet()
    {
        try
        {
            OnPostDeleteSession();

            Headers = new List<string>
            {
                "Name", "Started", "Status", "RAM", "CPU", "CPU model", "Executable", "Set File", "Progress", "Priority",
                "Actions"
            };

            UnfinishedBenchmarks = (await _mediator.Send(new GetAllBenchmarksQuery
            {
                Finished = false
            })).ToList();

            CpuModels = (await _mediator.Send(new GetAllCpuModelsQuery())).ToList();
            
            Priorities = Enum.GetValues(typeof(Priority))
                .Cast<Priority>()
                .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                .ToList();
            
            CreateInput.Priority = Priority.LOW;
            CreateInput.Ram = 4;
            CreateInput.Cpu = 8;
            CreateInput.TimeLimit = 900;
            CreateInput.HardTimeLimit = 960;

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

    /// <summary>
    /// Adds a configuration item to the session.
    /// </summary>
    /// <param name="scope"> scope of the configuration item </param>
    /// <param name="key"> key of the configuration item </param>
    /// <param name="value"> value of the configuration item </param>
    /// <returns> JSON response </returns>
    public IActionResult OnPostAddConfigItem(Scope scope, string? key, string? value)
    {
        if (string.IsNullOrEmpty(key))
            return new JsonResult(new { success = false, responseText = "Key is empty." });
        
        if (key.Length > 50)
            return new JsonResult(new { success = false, responseText = "Key is too long." });
        
        if (value is { Length: > 50 })
            return new JsonResult(new { success = false, responseText = "Value is too long." });
        
        value ??= "";

        var configurationItems = HttpContext.Session
                                     .GetComplexData<List<TempConfigData>>(_tempConfigDataListKey) ??
                                 new List<TempConfigData>();

        var newItem = new TempConfigData
        {
            Scope = scope,
            Key = key,
            Value = value,
            Id = (scope, key, value).GetHashCode()
        };

        if (configurationItems.Contains(newItem))
            return new JsonResult(new { success = false, responseText = "Item already exists." });

        configurationItems.Add(newItem);

        HttpContext.Session.SetComplexData(_tempConfigDataListKey, configurationItems);

        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(new { newItem.Id, newItem.Key, newItem.Value}),
            ContentType = "application/json"
        };
    }

    /// <summary>
    /// Deletes a configuration item from the session.
    /// </summary>
    /// <param name="id"> id of the configuration item </param>
    /// <returns> JSON response </returns>
    public IActionResult OnPostDeleteConfigItem(int id)
    {
        var configurationItems = HttpContext.Session
                                     .GetComplexData<List<TempConfigData>>(_tempConfigDataListKey) ??
                                 new List<TempConfigData>();

        if (configurationItems.Remove(new TempConfigData { Id = id }))
        {
            HttpContext.Session.SetComplexData(_tempConfigDataListKey, configurationItems);

            return new JsonResult(new { success = true, responseText = "Item removed." });
        }

        return new JsonResult(new { success = false, responseText = "Item not found." });
    }

    /// <summary>
    /// Adds a constraint item to the session.
    /// </summary>
    /// <param name="expression"> constraint expression </param>
    /// <returns> JSON response </returns>
    public IActionResult OnPostAddConstraint(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            return new JsonResult(new{success = false, responseText = "Expression is empty."});

        if (expression.Length > 255)
            return new JsonResult(new{success = false, responseText = "Expression is too long."});
        

        var constraintItems = HttpContext.Session
                                  .GetComplexData<List<TempConstraintData>>(_tempConstraintDataListKey) ??
                              new List<TempConstraintData>();

        var newConstraint = new TempConstraintData
        {
            Id = expression.GetHashCode(),
            Expression = expression
        };

        if (constraintItems.Contains(newConstraint))
            return new JsonResult(new{success = false, responseText = "Item already exists."});

        constraintItems.Add(newConstraint);

        HttpContext.Session.SetComplexData(_tempConstraintDataListKey, constraintItems);

        return new ContentResult
        {
            Content = JsonConvert.SerializeObject(new { newConstraint.Id, newConstraint.Expression}),
            ContentType = "application/json"
        };
    }

    /// <summary>
    /// Deletes a constraint item from the session.
    /// </summary>
    /// <param name="id"> id of the constraint item </param>
    /// <returns> JSON response </returns>
    public IActionResult OnPostDeleteConstraint(int id)
    {
        var constraintItems = HttpContext.Session
                                  .GetComplexData<List<TempConstraintData>>(_tempConstraintDataListKey) ??
                              new List<TempConstraintData>();

        if (constraintItems.Remove(new TempConstraintData{ Id = id }))
        {
            HttpContext.Session.SetComplexData(_tempConstraintDataListKey, constraintItems);

            return new JsonResult(new { success = true, responseText = "Item removed." });
        }

        return new JsonResult(new { success = false, responseText = "Item not found." });
    }

    /// <summary>
    /// Saves the benchmark with the given id and status.
    /// </summary>
    /// <param name="id"> benchmark id </param>
    /// <param name="status"> benchmark status </param>
    /// <returns> Page </returns>
    /// <exception cref="ApplicationException"> No privilege </exception>
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
                              throw new ApplicationException(ExceptionMessage<Benchmark>.NoPrivilege)
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

    /// <summary>
    /// Deletes the benchmark with the given id and name.
    /// </summary>
    /// <param name="id"> benchmark id </param>
    /// <param name="name"> benchmark name </param>
    /// <returns> Page </returns>
    /// <exception cref="ApplicationException"> No privilege </exception>
    public async Task<IActionResult> OnPostDeleteAsync(int id, string name)
    {
        try
        {
            await _mediator.Send(new DeleteBenchmarkCommand
            {
                Id = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<Benchmark>.NoPrivilege)
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

    /// <summary>
    /// Starts the benchmark with the given configuration and constraints.
    /// </summary>
    /// <returns> Page </returns>
    /// <exception cref="ApplicationException"> No privilege </exception>
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
            List<string>? constraintList = null;

            if (configs != null)
                configList = configs.Select(c => (c.Scope, c.Key!, c.Value ?? "")).ToList();

            if (constraints != null)
                constraintList = constraints.Select(c => c.Expression!).ToList();

            var config = await _mediator.Send(new CreateConfigurationCommand
            {
                Configurations = configList ?? new List<(Scope, string, string)>(),
                Constraints = constraintList,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<Configuration>.NoPrivilege),
                BenchmarkName = CreateInput.Name,
                Cpu = CreateInput.Cpu,
                Ram = CreateInput.Ram,
                TimeLimit = CreateInput.TimeLimit,
                HardTimeLimit = CreateInput.HardTimeLimit,
                ExecutableId = CreateInput.ExecutableId,
                SetFilePath = CreateInput.SetFilePath,
                PropertyFilePath = CreateInput.PropertyFilePath,
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
                CpuModelId = CreateInput.CpuModelId,
                CpuModelValue = CpuModels.Where(c => c.Id == CreateInput.CpuModelId)
                    .Select(c => c.Value).FirstOrDefault(),
                ConfigurationId = config.Id,
                ComputerGroupId = CreateInput.ComputerGroupId,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(ExceptionMessage<Benchmark>.NoPrivilege)
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

    /// <summary>
    /// Loads the form's needed data.
    /// </summary>
    private async Task LoadFormData()
    {
        try
        {
            Executables = (await _mediator.Send(new GetAllExecutablesQuery()))
                .Select(eh => new SelectListItem(eh.Name + ":" + eh.Version, eh.Id.ToString()))
                .ToList();

            SetFiles = (await _mediator.Send(new GetAllSetFileNamesQuery()))
                .Select(s => new SelectListItem(Path.GetFileName(s), s))
                .ToList();
            
            PrpFiles = (await _mediator.Send(new GetAllPropertyFileNamesBySourceSetQuery()))
                .Select(s => new SelectListItem(Path.GetFileName(s), s))
                .ToList();

            ComputerGroups = (await _mediator.Send(new GetAllComputerGroupsQuery()))
                .Select(c => new SelectListItem(c.Id + ": " + c.Description, c.Id.ToString()))
                .ToList();

            CpuModelValues = CpuModels
                .Select(c => new SelectListItem(c.Name + ": " + c.Value, c.Id.ToString()))
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            StatusMessage = "Error: " + (ex.InnerException ?? ex).Message;
        }
    }

    /// <summary>
    /// Empties the session storage.
    /// </summary>
    /// <returns> JSON response </returns>
    public IActionResult OnPostDeleteSession()
    {
        HttpContext.Session.Remove(_tempConstraintDataListKey);
        HttpContext.Session.Remove(_tempConfigDataListKey);

        return new JsonResult(new { success = true, responseText = "Session cleared." });
    }

    public class EditInputModel
    {
        [Display(Name = "Benchmark Priority")] 
        public Priority Priority { get; init; }
    }

    public class CreateInputModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Benchmark Name")]
        [RegularExpression("[A-Za-z0-9]+$")]
        public string Name { get; init; } = null!;

        [Required]
        [Display(Name = "Priority")]
        public Priority Priority { get; set; }

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
        public int ExecutableId { get; init; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Set File Path")]
        public string SetFilePath { get; init; } = null!;

        [Required]
        [StringLength(250)]
        [Display(Name = "Property File Path")]
        public string PropertyFilePath { get; init; } = null!;

        [Display(Name = "Computer Group to run on")]
        public int ComputerGroupId { get; init; }

        [Display(Name = "Cpu Model to run on")]
        public int CpuModelId { get; init; }
    }

    public class TempConfigData
    {
        public Scope Scope { get; init; }
        public string? Key { get; init; }
        public string? Value { get; init; }
        
        public int Id { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (TempConfigData)obj;
            return Scope == other.Scope && Key == other.Key && Value == other.Value || Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Key != null ? HashCode.Combine((int)Scope, Key, Value) : Id;
        }
    }

    public class TempConstraintData
    {
        public int Id { get; init; }
        
        public string? Expression { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (TempConstraintData)obj;
            return Expression != null && (Expression.Equals(other.Expression) || Id == other.Id);
        }

        public override int GetHashCode()
        {
            return Expression != null ? Expression.GetHashCode() : Id;
        }
    }
}