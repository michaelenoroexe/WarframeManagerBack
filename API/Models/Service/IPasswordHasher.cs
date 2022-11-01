namespace API.Models.Service
{
    /// <summary>
    /// Represent object that can hash strings.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hash string to store in DB.
        /// </summary>
        /// <param name="password">String to hash.</param>
        /// <returns>Hashed string.</returns>
        public string HashString(string password);
    }
}
