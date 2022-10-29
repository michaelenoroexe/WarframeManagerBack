﻿using Shared;

namespace API.Models.UserWork.Interfaces
{
    internal interface IBufferChecker
    {
        /// <summary>
        /// Get user item list from buffer.
        /// </summary>
        /// <param name="user">User which items needed to find.</param>
        Dictionary<string, int>? GetItems(IUser user);
        /// <summary>
        /// Get user resource list from buffer.
        /// </summary>
        /// <param name="user">User which resource needed to find.</param>
        Dictionary<string, int>? GetResources(IUser user);
        /// <summary>
        /// Get user credit number from buffer.
        /// </summary>
        /// <param name="user">User which credit number needed to find.</param>
        int? GetCredits(IUser user);
        /// <summary>
        /// Get user profile info from buffer.
        /// </summary>
        /// <param name="user">User which prfoile info needed to find.</param>
        UserInfo? GetProfile(IUser user);
    }
}
