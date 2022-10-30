using MongoDB.Bson;
using Shared;

namespace UserValidation
{
    public class FullUser : IUser, IClientUser
    {
        private ObjectId _id;
        private string _login;
        private string _password;
        public ObjectId Id { get => _id; init => _id = value; }
        public string Login { get => _login; init => _login = value; }
        public string Password { get => _password; init => _password = value; }

        public FullUser(ObjectId id, string login, string password)
        {
            _id = id;
            _login = login;
            _password = password;
        }

        public bool Equals(IUser? other)
        {
            if (other is null) return false;
            return _id.ToString().GetHashCode() == other.Id.ToString().GetHashCode();
        }
    }
}
