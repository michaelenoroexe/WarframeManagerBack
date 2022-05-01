using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Models;
using API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API
{
    public static class JwtAuthentication
    {
        public static string SecurityKey { get; set; } = "ouNtF8Xds1jE55/d+iVZ99u0f2U6lQ+AHdiPFwjVW3o=";
        public static string ValidIssuer { get; set; } = "https://localhost:4200/";
        public static string ValidAudience { get; set; } = "https://localhost:7132/";

        public static SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Convert.FromBase64String(SecurityKey));
        public static SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        public static string GenerateToken(User user)
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

        private static string GetUserLoginFromToken(HttpRequest request)
        {
            var bearer =
                request.Headers.ToArray().First(h => h.Key == "Authorization")
                    .Value.First().Substring(7);

            var jwtHandler = new JwtSecurityTokenHandler();
            var readableToken = jwtHandler.CanReadToken(bearer);
            if (readableToken != true) return "Error: No bearer in the header";

            var token = jwtHandler.ReadJwtToken(bearer);
            var claims = token.Claims;

            var userLoginClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);

            return userLoginClaim == null ? "Error: Token does not contain an Login claim." : userLoginClaim.Value;
        }

        public static async Task<User> GetUserFromTokenAsync(HttpRequest request)
        {
            var _userRepository = new UserRepository();
            var login = GetUserLoginFromToken(request);
            return await _userRepository.FindUserAsync(login);
        }
    }
}
