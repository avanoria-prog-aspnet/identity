using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Identity;
using Presentation.WebApp.Models.Authentication;


namespace Presentation.WebApp.Controllers;

public class AuthenticationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
    ) : Controller
{
    [HttpGet]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInForm form, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
            return View(form);
        }

        var user = await userManager.FindByEmailAsync(form.Email);
        if (user is null)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
            return View(form);
        }

        var result = await signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Account");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "User account has been temporary locked");
            return View(form);
        }

        ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
        return View(form);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public new async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("SignIn", "Authentication");
    }
}