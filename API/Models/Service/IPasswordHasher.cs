namespace API.Models.Service
{
    public interface IPasswordHasher
    {
        public string HashString(string password);
    }
}
