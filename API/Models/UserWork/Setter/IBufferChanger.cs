using Shared;

namespace API.Models.UserWork.Setter
{
    internal interface IBufferChanger
    {
        /// <summary>
        /// Change user item number in buffer.
        /// </summary>
        /// <param name="user">User whose items needed to find.</param>
        void SetItemNum(IUser user, KeyValuePair<string, int> item);
        /// <summary>
        /// Change user resource number in buffer.
        /// </summary>
        /// <param name="user">User whose resource needed to change.</param>
        void SetResourcesNum(IUser user, KeyValuePair<string, int> resource);
        /// <summary>
        /// Change user credit number in buffer.
        /// </summary>
        /// <param name="user">User whose credit number needed to change.</param>
        void SetCreditNumber(IUser user, int credits);
        /// <summary>
        /// Change user profile info in buffer.
        /// </summary>
        /// <param name="user">User whose prfoile info needed to change.</param>
        void SetProfileInfo(IUser user, UserInfo profile);
    }
}
