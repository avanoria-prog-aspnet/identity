using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController : Controller
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
        const string secretPassword = "BytMig123!";
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        if (form.Password != secretPassword)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect password");
            return View(form);
        }

        var claims = new List<Claim>() 
        { 
            new (ClaimTypes.Name, "hans.mattin-lassei")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
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