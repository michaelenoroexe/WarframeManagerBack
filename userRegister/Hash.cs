using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace API
{
    public static class Program
    {
        private const int SaltSize = 16;
        private const int iterationCount = 100000;
        private const int HashSize = 32;
        public static string HashString(string password )
        {

            byte[] salt;
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            new RNGCryptoServiceProvider().GetNonZeroBytes(salt = new byte[SaltSize]);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterationCount,
                numBytesRequested: HashSize));
            return hashed;
        }

        public static bool Verify(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);

            var salt = new bytes[SaltSize];

            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            string toCheckHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterationCount,
                numBytesRequested = HashSize));
            return hashBytes == toCheckHashed;
        }
    }
}