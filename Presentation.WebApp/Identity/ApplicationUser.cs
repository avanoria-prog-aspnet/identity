namespace Presentation.WebApp.Identity;

public class ApplicationUser
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string HashPasswordSalt { get; set; } = null!;
}
