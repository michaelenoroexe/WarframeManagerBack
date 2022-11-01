using Shared;

namespace API.Models.UserWork.Setter
{
    /// <summary>
    /// Represend object that change state in buffer.
    /// </summary>
    internal interface IBufferChanger
    {
        /// <summary>
        /// Change user item number in buffer.
        /// </summary>
        /// <param name="user">User whose item number needed to change.</param>
        void SetItemNum(IUser user, KeyValuePair<string, int> item);
        /// <summary>
        /// Change user resource number in buffer.
        /// </summary>
        /// <param name="user">User whose resource number needed to change.</param>
        void SetResourcesNum(IUser user, KeyValuePair<string, int> resource);
        /// <summary>
        /// Change user credit number in buffer.
        /// </summary>
        /// <param name="user">User whose credits number needed to change.</param>
        void SetCreditNumber(IUser user, int credits);
        /// <summary>
        /// Change user profile info in buffer.
        /// </summary>
        /// <param name="user">User whose prfoile info needed to change.</param>
        void SetProfileInfo(IUser user, UserInfo profile);
    }
}
