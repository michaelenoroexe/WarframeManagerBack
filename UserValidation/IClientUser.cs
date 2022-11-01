namespace UserValidation
{
    /// <summary>
    /// Representation of client provided user information.
    /// </summary>
    public interface IClientUser
    {
        /// <summary>
        /// Users login.
        /// </summary>
        string Login { get; }
        /// <summary>
        /// Users Password.
        /// </summary>
        string? Password { get; }
    }
}
