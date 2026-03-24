using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
