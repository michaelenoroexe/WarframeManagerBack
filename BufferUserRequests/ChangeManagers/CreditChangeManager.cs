using Shared;

namespace BufferUserRequests.ChangeManagers
{
    internal class CreditChangeManager : ISavableChangeManager<int, UserResources>
    {
        /// <summary>
        /// Storage of user changes.
        /// </summary>
        private int _storage;
        /// <summary>
        /// Get instance of CreditChangeManager.
        /// </summary>
        public CreditChangeManager()
        {
            _storage = 0;
        }
        /// <summary>
        /// Change storage to input value.
        /// </summary>
        public void Edit(int item) => _storage = item;
        /// <summary>
        /// Get current state of storage.
        /// </summary>
        public int GetCurrent() => _storage;
        /// <summary>
        /// Save state to object in argument.
        /// </summary>
        /// <param name="save">User resource item to store resource in.</param>
        public void Save(ref UserResources save) => save.Credits = _storage;
    }
}
