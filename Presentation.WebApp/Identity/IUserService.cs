namespace Presentation.WebApp.Identity;

public interface IUserService
{
    Task<ApplicationUser> CreateUserAsync(string email, string password);
    Task<ApplicationUser?> ValidateCredentialsAsync(string email, string password);
}
