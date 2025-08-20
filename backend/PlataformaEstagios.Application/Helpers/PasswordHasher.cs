using Microsoft.AspNetCore.Identity;

namespace PlataformaEstagios.Application.Helpers
{
    public class PasswordHasher
    {
        public static string Encrypt(string password)
        {
            var encripter = new PasswordHasher<string>();
            var hashedPasswrod = encripter.HashPassword(null, password);

            return hashedPasswrod;
        }

        public static bool Decrypt(string password, string storedPassword)
        {
            var decripter = new PasswordHasher<string>();
            var result = decripter.VerifyHashedPassword(null, storedPassword, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}