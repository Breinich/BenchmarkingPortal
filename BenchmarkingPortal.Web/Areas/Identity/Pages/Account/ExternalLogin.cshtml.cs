// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BenchmarkingPortal.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// using Octokit;

namespace BenchmarkingPortal.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly ILogger<ExternalLoginModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public ExternalLoginModel(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IUserStore<User> userStore,
        ILogger<ExternalLoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ProviderDisplayName { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    public string Email { get; set; }
    public string Name { get; set; }

    public bool FtsrgMember { get; init; }

    public IActionResult OnGet()
    {
        return RedirectToPage("./Login");
    }

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("./ExternalLogin", "Callback", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity!.Name,
                info.LoginProvider);
            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut) return RedirectToPage("./Lockout");

        // If the user does not have an account, then ask the user to create an account.
        ReturnUrl = returnUrl;
        ProviderDisplayName = info.ProviderDisplayName;
        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            Email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
            Name = info.Principal.FindFirstValue(ClaimTypes.Name);
        /* with Ocktokit NuGet package
                var github = new GitHubClient(new ProductHeaderValue("BenchmarkingPortal"));
                var ftsrgOrg = await github.Organization.Get("ftsrg");
                var user = await github.User.Get(Name);
                FtsrgMember = user.Company == ftsrgOrg.Name;*/
        Input = new InputModel
        {
            UserName = Name,
            Email = Email
        };
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information during confirmation.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        if (ModelState.IsValid)
        {
            var user = new User { UserName = Input.UserName, Email = Input.Email };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Guest);

                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                    await _signInManager.SignInAsync(user, false, info.LoginProvider);
                    return LocalRedirect(returnUrl);
                }
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        ProviderDisplayName = info.ProviderDisplayName;
        ReturnUrl = returnUrl;
        return Page();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; init; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; init; }
    }
}