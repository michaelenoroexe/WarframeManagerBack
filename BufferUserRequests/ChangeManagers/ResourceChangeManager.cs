using Shared;

namespace BufferUserRequests.ChangeManagers
{
    /// <summary>
    /// Manager of users resources.
    /// </summary>
    internal sealed class ResourceChangeManager : ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>
    {
        /// <summary>
        /// Storage of user changes.
        /// </summary>
        private readonly Dictionary<string, int> _storage;
        /// <summary>
        /// Get instance of ResourceChangeManager.
        /// </summary>
        public ResourceChangeManager() => _storage = new Dictionary<string, int>();
        /// <summary>
        /// Edit or add item in buffer.
        /// </summary>
        public void Edit(KeyValuePair<string, int> item) => _storage[item.Key] = item.Value;
        /// <summary>
        /// Get current state of storage.
        /// </summary>
        public Dictionary<string, int> GetCurrent() => _storage;
        /// <summary>
        /// Save state to object in argument.
        /// </summary>
        /// <param name="save">User resource item to store resource in.</param>
        public void Save(ref UserResources save)
        {
            foreach (KeyValuePair<string, int> item in _storage)
                save.Resources[item.Key] = item.Value;
        }
    }
}
