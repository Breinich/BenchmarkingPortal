// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace BenchmarkingPortal.Web.Areas.Identity.Pages.Account
{
    public class LoginWithRecoveryCodeModel : PageModel
    {       public IActionResult OnGet()
        { 
            return Page();
        }

        public IActionResult OnPost()
        {
                return Page();
        }
    }
}
