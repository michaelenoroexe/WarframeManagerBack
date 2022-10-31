using System.Security.Claims;

namespace UserValidation.JWTValidation
{
    internal sealed class JWTUserConverter : IUserConverter<ClaimsPrincipal>
    {
        public IClientUser CreateUser(ClaimsPrincipal user)
        {
            string? login = user.Claims.FirstOrDefault()?.Value;
            if (login is null) throw new ArgumentException("Client dont have login in claims.");
            return new ClientUser(login);
        }
    }
}
