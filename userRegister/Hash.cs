using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace API
{
    public static class Hash
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
            byte[] hashed =  (KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterationCount,
                numBytesRequested: HashSize));
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hashed, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);

            var salt = new byte[SaltSize];

            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            byte[] toCheckHashed = GetBytes(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterationCount,
                numBytesRequested: HashSize));

            string hashBytesString = string.Join("",hashBytes.Select((val)=>val.ToString()).ToArray());
            return hashBytesString == toCheckHashed;
        }
    }
}