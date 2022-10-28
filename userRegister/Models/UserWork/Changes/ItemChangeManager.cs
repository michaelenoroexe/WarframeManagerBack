using API.Models.Interfaces;

namespace API.Models.UserWork.Changes
{
    internal class ItemChangeManager : ISavableChangeManager<KeyValuePair<IResource, int>, Dictionary<IResource, int>, UserResources>
    {
        /// <summary>
        /// Storage of user changes.
        /// </summary>
        private Dictionary<IResource, int> _storage;
        /// <summary>
        /// Get instance of ItemChangeManager.
        /// </summary>
        public ItemChangeManager()
        {
            _storage = new Dictionary<IResource, int>();
        }
        /// <summary>
        /// Edit or add item in buffer.
        /// </summary>
        public void Edit(KeyValuePair<IResource, int> item) => _storage[item.Key] = item.Value;
        /// <summary>
        /// Get current state of storage.
        /// </summary>
        public Dictionary<IResource, int> GetCurrent() => _storage;
        /// <summary>
        /// Save state to object in argument.
        /// </summary>
        /// <param name="save">User resource item to store resource in.</param>
        public void Save(ref UserResources save)
        {
            foreach (KeyValuePair<IResource, int> item in _storage)
            {
                save.Items[item.Key.Id.ToString()] = item.Value;
            }
        }
    }
}
