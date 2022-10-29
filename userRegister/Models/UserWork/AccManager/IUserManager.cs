using Shared;

namespace API.Models.UserWork.AccManager
{
    internal interface IUserManager
    {
        public Task AddUserAsync(IUser user, string password);

        public string LoginUser(IUser user);

        public Task ChangeUserPassword(User user, string newPassword);

        public string DeleteUserAsync(IUser user);
    }
}
