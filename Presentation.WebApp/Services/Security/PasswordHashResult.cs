namespace Presentation.WebApp.Services.Security;

public record PasswordHashResult
(
    string Hash,
    string Salt
);