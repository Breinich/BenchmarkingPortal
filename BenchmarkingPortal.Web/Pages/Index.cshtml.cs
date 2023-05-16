using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly SignInManager<User> _signInManager;

    public IndexModel(ILogger<IndexModel> logger, SignInManager<User> signInManager)
    {
        _logger = logger;
        _signInManager = signInManager;
    }

    public void OnGet()
    {
        Response.Redirect(_signInManager.IsSignedIn(User) ? "/Home" : "/Identity/Account/Login");
    }
}