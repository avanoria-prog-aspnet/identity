using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Data;
using Presentation.WebApp.Services.Security;


namespace Presentation.WebApp.Identity;


public class UserService(DataContext context, IPasswordHasher passwordHasher) : IUserService
{
    public async Task<ApplicationUser> CreateUserAsync(string email, string password)
    {
        var exists = await context.Users.AnyAsync(x => x.Email == email);
        if (exists)
            throw new InvalidOperationException("User already exists");

        var passwordResult = passwordHasher.HashPassword(password);

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            HashedPassword = passwordResult.Hash,
            HashPasswordSalt = passwordResult.Salt
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<ApplicationUser?> ValidateCredentialsAsync(string email, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user is null)
            return null;

        var isValid = passwordHasher.VerifyPassword(password, user.HashedPassword, user.HashPasswordSalt);
        return (isValid) ? user : null;
    }
}
