using Shared;

namespace BufferUserRequests.ChangeManagers
{
    internal class ItemChangeManager : ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>
    {
        /// <summary>
        /// Storage of user changes.
        /// </summary>
        private Dictionary<string, int> _storage;
        /// <summary>
        /// Get instance of ItemChangeManager.
        /// </summary>
        public ItemChangeManager()
        {
            _storage = new Dictionary<string, int>();
        }
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
            {
                save.Items[item.Key] = item.Value;
            }
        }
    }
}
