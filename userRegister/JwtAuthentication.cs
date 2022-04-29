using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API
{
    public static class JwtAuthentication
    {
        public static string SecurityKey { get; set; }
        public static string ValidIssuer { get; set; }
        public static string ValidAudience { get; set; }

        public static SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Convert.FromBase64String(SecurityKey));
        public static SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        public static string GenerateToken(User user)
        {
            var token = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Login),
                },
                expires: DateTime.UtcNow.AddDays(30),
                notBefore: DateTime.UtcNow,
                signingCredentials: SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
