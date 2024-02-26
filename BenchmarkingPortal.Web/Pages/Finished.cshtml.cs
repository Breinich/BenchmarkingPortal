using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.Benchmark.Commands;
using BenchmarkingPortal.Bll.Features.Benchmark.Queries;
using BenchmarkingPortal.Bll.Features.CpuModel.Queries;
using BenchmarkingPortal.Bll.Features.Executable.Queries;
using BenchmarkingPortal.Bll.Features.Result.Commands;
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

    public Finished(IMediator mediator)
    {
        _mediator = mediator;
    }

    [TempData] public string? StatusMessage { get; set; }

    public List<BenchmarkHeader> FinishedBenchmarks { get; set; } = new();
    public Dictionary<int, string> CpuModelNames { get; set; } = new();
    public Dictionary<int, string> ExecutableNames { get; set; } = new();
    public List<string> Headers { get; set; } = new();

    public async Task<IActionResult> OnGet()
    {
        try
        {
            Headers = new List<string>
            {
                "Name", "Started", "RAM", "CPU", "CpuGroup", "Executable", "SetFile", "Actions"
            };

            FinishedBenchmarks = (await _mediator.Send(new GetAllBenchmarksQuery
            {
                Finished = true
            })).ToList();

            CpuModelNames = (await _mediator.Send(new GetAllCpuModelsQuery()))
                .ToDictionary(cm => cm.Id, cm => cm.Name ?? cm.Value ?? "No information");

            ExecutableNames = (await _mediator.Send(new GetAllExecutablesQuery()))
                .ToDictionary(x => x.Id, x => x.Name + ":" + x.Version);

            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.InnerException?.Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, string name, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteBenchmarkCommand
            {
                Id = id,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<Benchmark>().NoPrivilege)
            }, cancellationToken);

            StatusMessage = $"{name} deleted successfully.";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + e.InnerException?.Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDownload(string path, CancellationToken cancellationToken)
    {
        try
        {
            var (fileStream, contentType, fileName) = await _mediator.Send(new DownloadResultCommand
            {
                Path = path
            }, cancellationToken);

            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileName
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }
}