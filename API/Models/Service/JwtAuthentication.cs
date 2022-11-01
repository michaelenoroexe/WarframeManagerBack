using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Models.Service
{
    public static class JwtAuthentication
    {
        public static string SecurityKey { get; }
        public static string ValidIssuer { get; }
        public static string ValidAudience { get; }

        static JwtAuthentication()
        {
            SecurityKey = "ouNtF8Xds1jE55/d+iVZ99u0f2U6lQ+AHdiPFwjVW3o=";
            ValidIssuer = "https://localhost:4200/";
            ValidAudience = "https://localhost:7132/";
        }

        public static SymmetricSecurityKey SymmetricSecurityKey => new(Convert.FromBase64String(SecurityKey));
        public static SigningCredentials SigningCredentials => new(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        public static string GenerateToken(IUser user)
        {
            var token = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Login)
                },
                expires: DateTime.UtcNow.AddDays(30),
                notBefore: DateTime.UtcNow,
                signingCredentials: SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
