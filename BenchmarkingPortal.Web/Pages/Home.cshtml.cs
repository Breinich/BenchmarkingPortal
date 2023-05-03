﻿using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenchmarkingPortal.Web.Pages;

public class Home : PageModel
{
    private readonly SignInManager<User> _signInManager;
    public Home(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }
    public void OnGet()
    {
        if(!_signInManager.IsSignedIn(User))
        {
            Response.Redirect("/Identity/Account/Login");
        }
        else if(User.IsInRole(Roles.Guest))
        {
            Response.Redirect("/Identity/Account/AccessDenied");
        }
    }
}