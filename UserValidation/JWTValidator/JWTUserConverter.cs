using System.Security.Claims;

namespace UserValidation.JWTValidation
{
    /// <summary>
    /// Converts user inputed clains to client user.
    /// </summary>
    internal sealed class JWTUserConverter : IUserConverter<ClaimsPrincipal>
    {
        /// <summary>
        /// Create user from claims.
        /// </summary>
        /// <param name="user">Users claims.</param>
        /// <returns>IClient user created from claims.</returns>
        /// <exception cref="ArgumentException">If inputed claims dont contain user login.</exception>
        public IClientUser CreateUser(ClaimsPrincipal user)
        {
            string? login = user.Claims.FirstOrDefault()?.Value;
            if (login is null) throw new ArgumentException("Client dont have login in claims.");
            return new ClientUser(login);
        }
    }
}
