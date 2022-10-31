namespace API.Models.UserWork
{
    public interface IPasswordHasher
    {
        public string HashString(string password);
    }
}
