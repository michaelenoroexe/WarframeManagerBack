using BufferUserRequests.DBSaver;
using Microsoft.Extensions.Logging;
using Shared;

namespace BufferUserRequests.ManStorage
{
    internal sealed class ManagersStorageBuilder
    {
        private readonly Saver _saver;
        private readonly ILogger _logger;
        /// <summary>
        /// Instantiate storage builder.
        /// </summary>
        public ManagersStorageBuilder(Saver saver, ILogger logger)
        {
            _saver = saver;
            _logger = logger;
        }
        /// <summary>
        /// Create new instance of manager storage for user.
        /// </summary>
        /// <param name="user">User who needed</param>
        public IManagersStorage CreateManagersStorage(IUser user)
            => new ManagersStorage(user, _saver, _logger);
    }
}
