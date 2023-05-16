// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Areas.Identity.Pages.Account;

public class ResetPasswordModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("./ForgotPasswordConfirmation");
    }
}