﻿using Shared;

namespace API.Repositories
{
    public interface IUserManager
    {
        public Task AddUserAsync(IUser user, string password);

        public string LoginUser(IUser user);

        public Task ChangeUserPasswordAsync(IUser user, string newPassword);

        public Task DeleteUserAsync(IUser user);
    }
}
