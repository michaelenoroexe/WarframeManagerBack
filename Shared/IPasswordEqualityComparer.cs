namespace Shared
{
    /// <summary>
    /// Equality comparer for ordinary and hased string.
    /// </summary>
    public interface IPasswordEqualityComparer
    {
        /// <summary>
        /// Compare user inputed password with hashed in db.
        /// </summary>
        /// <returns>Are passwords equal or not.</returns>
        public bool Equals(string password, string hashedPassword);
    }
}
