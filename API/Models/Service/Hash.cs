using Shared;
using System.Security.Cryptography;

namespace API.Models.Service
{
    /// <summary>
    /// Class to work with hashed strings.
    /// </summary>
    internal sealed class Hash : IPasswordEqualityComparer, IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 1000;

        public string HashString(string password)
        {
            byte[] salt;
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool Equals(string password, string hashedPassword) => HashString(password).Equals(hashedPassword, StringComparison.Ordinal);
    }
}
