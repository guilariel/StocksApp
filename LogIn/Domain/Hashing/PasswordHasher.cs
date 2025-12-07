using System.Security.Cryptography;

namespace LogIn.Domain.Hashing
{
    public class PasswordHasher
    {
        public static string Hasher(string password)
        {
            byte[] salt = new byte[16];
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');          // separa el salt y el hash guardado
            var salt = Convert.FromBase64String(parts[0]);  // convierte el salt
            var hash = Convert.FromBase64String(parts[1]);  // convierte el hash guardado

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] newHash = pbkdf2.GetBytes(32);           // genera un hash nuevo

            return newHash.SequenceEqual(hash);             // compara ambos hashes
        }
    }
}
