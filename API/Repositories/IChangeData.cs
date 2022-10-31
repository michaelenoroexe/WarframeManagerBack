using Shared;

namespace API.Repositories
{
    public interface IChangeData
    {
        public void UpdateResource(IUser user, KeyValuePair<string, int> res);

        public void UpdateItem(IUser user, KeyValuePair<string, int> item);

        public void UpdateCredits(IUser user, int credits);

        public void UpdateProfile(IUser user, UserInfo userInfo);
    }
}
