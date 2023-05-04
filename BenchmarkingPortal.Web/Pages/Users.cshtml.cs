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

    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    public readonly RoleManager<IdentityRole<int>> RoleManager;

    public UsersModel(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IMediator mediator)
    {
        _userManager = userManager;
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
        var subscribed = Input.Subscribed;
        var role = Input.Role;
        
        var user = await _userManager.FindByNameAsync(name);
        if (user == null)
        {
            StatusMessage = "Error: User not found";
            return RedirectToPage();
        }
        
        user.Subscription = subscribed;
        await _userManager.UpdateAsync(user);
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (!currentRoles.Contains(role))
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);
        }
        
        StatusMessage = "User updated";
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(string name)
    {
        var user = await _userManager.FindByNameAsync(name);
        if (user == null)
        {
            StatusMessage = "Error: User not found";
            return RedirectToPage();
        }
        
        await _userManager.DeleteAsync(user);
        
        StatusMessage = "User deleted";
        return RedirectToPage();
    }
}