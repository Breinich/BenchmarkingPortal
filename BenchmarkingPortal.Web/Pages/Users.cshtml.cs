using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Bll.Features.User.Queries;
using BenchmarkingPortal.Dal.Dtos;
using BenchmarkingPortal.Dal.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = Policies.RequireAdministratorRole)]
public class UsersModel : PageModel
{
    private readonly IMediator _mediator;
    public readonly RoleManager<IdentityRole<int>> RoleManager;

    public UsersModel(RoleManager<IdentityRole<int>> roleManager, IMediator mediator)
    {
        UserList = new List<UserHeader>();
        Roles = new List<SelectListItem>();
        _mediator = mediator;
        RoleManager = roleManager;

        Input = new InputModel();
    }

    [TempData] public string? StatusMessage { get; set; }

    public IEnumerable<SelectListItem> Roles { get; set; }
    public List<UserHeader> UserList { get; set; }
    public List<string> Headers { get; set; } = new();

    [BindProperty] public InputModel Input { get; init; }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            Headers = new List<string>
            {
                "Username", "Email", "Role", "Actions"
            };

            Roles = await RoleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString()
            }).ToListAsync();

            UserList = (await _mediator.Send(new GetAllUsersQuery())).ToList();
            return Page();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostSaveAsync(string name)
    {
        if (!ModelState.IsValid) return Page();

        var subscribed = Input.Subscribed;
        var role = Input.Role;

        try
        {
            var updatedUser = await _mediator.Send(new UpdateUserCommand
            {
                UserName = name,
                Subscription = subscribed,
                Role = role,
                InvokerName = User.Identity?.Name ??
                              throw new ApplicationException(new ExceptionMessage<User>().NoPrivilege)
            });

            StatusMessage = $"{updatedUser.UserName} updated";
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string name)
    {
        try
        {
            await _mediator.Send(new DeleteUserCommand
            {
                UserName = name,
                InvokerName = User.Identity?.Name ?? throw new ApplicationException("Authenticated user not found")
            });

            StatusMessage = $"{name} deleted";
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = "Error: " + (e.InnerException ?? e).Message;

            return RedirectToPage();
        }
    }

    public class InputModel
    {
        public bool Subscribed { get; init; }
        public string? Role { get; init; }
    }
}