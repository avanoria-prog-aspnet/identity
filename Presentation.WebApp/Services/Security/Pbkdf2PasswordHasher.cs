using System.Security.Cryptography;

namespace Presentation.WebApp.Services.Security;

public interface IPasswordHasher
{
    PasswordHashResult HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword, string salt);
}

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int _saltSize = 32;
    private const int _keySize = 32;
    private const int _iterations = 600000;
    private const string _secretKey = "NjljMjZjNTctNThmMC04MzMxLWI2MDEtNWZmY2U5MGIwNzYy";

    public PasswordHashResult HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var modifiedPassword = $"{password}.{_secretKey}";

        var hash = Rfc2898DeriveBytes.Pbkdf2
        (
            modifiedPassword,
            salt,
            _iterations,
            HashAlgorithmName.SHA256,
            _keySize
        );

        return new PasswordHashResult
        (
            Convert.ToBase64String(hash),
            Convert.ToBase64String(salt)
        );
    }

    public bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        var _salt = Convert.FromBase64String(salt);
        var _expectedHash = Convert.FromBase64String(hashedPassword);
        var _modifiedPassword = $"{password}.{_secretKey}";

        var hash = Rfc2898DeriveBytes.Pbkdf2
        (
            _modifiedPassword,
            _salt,
            _iterations,
            HashAlgorithmName.SHA256,
            _expectedHash.Length
        );

        return CryptographicOperations.FixedTimeEquals(hash, _expectedHash);
    }
}
