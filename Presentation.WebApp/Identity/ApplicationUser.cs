using Microsoft.AspNetCore.Identity;

namespace Presentation.WebApp.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
