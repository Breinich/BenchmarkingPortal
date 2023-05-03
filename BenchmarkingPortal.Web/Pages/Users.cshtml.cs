using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

[Authorize(Policy = "RequireAdminRole")]
public class Users : PageModel
{
    public void OnGet()
    {

    }
}