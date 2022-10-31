namespace Shared
{
    public interface IPasswordEqualityComparer
    {
        /// <summary>
        /// Compare user inputed password with hashed in db.
        /// </summary>
        /// <returns>Are password equal or not.</returns>
        public bool Equals(string x, string y);
    }
}
