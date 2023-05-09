using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
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

    [BindProperty] public string? StatusMessage { get; set; }
    [BindProperty] public List<BenchmarkHeader> FinishedBenchmarks { get; set; }

    
    public Finished(IMediator mediator)
    {
        _mediator = mediator;
        FinishedBenchmarks = new List<BenchmarkHeader>();
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            FinishedBenchmarks = (await _mediator.Send(new GetAllBenchmarksQuery()
            {
                Finished = true
            })).ToList();

            return Page();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.InnerException?.Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _mediator.Send(new DeleteBenchmarkCommand()
            {
                Id = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            });

            StatusMessage = "Benchmark deleted successfully.";
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