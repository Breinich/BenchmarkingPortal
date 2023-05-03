using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = "RequireApprovedUser")]
public class Home : PageModel
{

    public Home()
    {
    }

    public void OnGet()
    {
    }
}