using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.SourceSet.Queries;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireApprovedUser)]
public class Finished : PageModel
{
    private readonly IMediator _mediator;

    [TempData]
    public string? StatusMessage { get; set; }
    
    public List<BenchmarkHeader> FinishedBenchmarks { get; set; } = new();
    
    public Dictionary<int, string> ExecutableNames { get; set; } = new();
    public Dictionary<int, string> SourceSetNames { get; set; } = new();


    public Finished(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            FinishedBenchmarks = (await _mediator.Send(new GetAllBenchmarksQuery()
            {
                Finished = true
            })).ToList();
            
            ExecutableNames = (await _mediator.Send(new GetAllExecutablesQuery()))
                .ToDictionary(x => x.Id, x => x.Name+":"+x.Version);
            
            SourceSetNames = (await _mediator.Send(new GetAllSourceSetsQuery()))
                .ToDictionary(x => x.Id, x => x.Name+": "+x.Version);

            return Page();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.InnerException?.Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, string name)
    {
        try
        {
            await _mediator.Send(new DeleteBenchmarkCommand()
            {
                Id = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = $"{name} deleted successfully.";
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.InnerException?.Message;

            return RedirectToPage();
        }
    }

    public IActionResult OnPostDownload(string path)
    {
        try
        {
            StatusMessage = "Download started.";
            return Page();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

}