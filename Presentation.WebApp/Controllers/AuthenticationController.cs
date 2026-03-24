using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Identity;
using Presentation.WebApp.Models.Authentication;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController(IUserService userService) : Controller
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

        var user = await userService.ValidateCredentialsAsync(form.Email, form.Password);
        if (user is null)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
            return View(form);
        }

        var claims = new List<Claim>()
        {             
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.Email),
            new (ClaimTypes.Email, user.Email),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = form.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
        };

        await HttpContext.SignInAsync
            (
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties
            );

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Account");
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