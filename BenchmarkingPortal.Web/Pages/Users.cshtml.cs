using BenchmarkingPortal.Bll.Features.User.Commands;
using BenchmarkingPortal.Bll.Features.User.Queries;
using BenchmarkingPortal.Dal.Dtos;
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
    [TempData]
    public string? StatusMessage { get; set; }
    public IEnumerable<SelectListItem> Roles { get; set; }
    public List<UserHeader> UserList { get; set; }
    [BindProperty]
    public InputModel Input { get; set; }
    
    public class InputModel
    {
        public bool Subscribed { get; set; }
        public string? Role { get; set; }
    }

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
    
    public async Task<IActionResult> OnGet()
    {
        Roles = await RoleManager.Roles.Select(x => new SelectListItem()
        {
            Text = x.ToString(),
            Value = x.ToString()
        }).ToListAsync();
        
        UserList = (await _mediator.Send(new GetAllUsersQuery())).ToList();
        return Page();
    }
    
    public async Task<IActionResult> OnPostSaveAsync(string name)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var subscribed = Input.Subscribed;
        var role = Input.Role;

        try
        {
            var updatedUser = await _mediator.Send(new UpdateUserCommand()
            {
                UserName = name,
                Subscription = subscribed,
                Role = role,
                InvokerName = User.Identity?.Name ?? throw new ApplicationException("Authenticated user not found")
            });
            
            StatusMessage = "User updated";
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = e.InnerException?.Message ?? e.Message;
            
            return RedirectToPage();
        }
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(string name)
    {
        try
        {
            await _mediator.Send(new DeleteUserCommand()
            {
                UserName = name,
                InvokerName = User.Identity?.Name ?? throw new ApplicationException("Authenticated user not found")
            });
            
            StatusMessage = "User deleted";
            return RedirectToPage();
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
            StatusMessage = e.InnerException?.Message ?? e.Message;
            
            return RedirectToPage();
        }
    }
}