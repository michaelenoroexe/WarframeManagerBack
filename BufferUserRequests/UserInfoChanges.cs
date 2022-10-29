using BufferUserRequests.ManStorage;
using Shared;

namespace BufferUserRequests
{
    public sealed class UserInfoChanges : IDisposable
    {
        // Variables to make class auto time exposeble
        private DateTime _lastAcces;
        private Task _savingToDB;
        private CancellationTokenSource _tokenSource = new();
        private int _delayBeforeSave = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
        private LinkedList<UserInfoChanges> _storagePlace;
        internal IUser User { get; init; }
        private IManagersStorage _managerStorage;
        private bool _disposedValue;
        /// <summary>
        /// Refresh last acces when add to any field.
        /// </summary>
        private void Refresh(Action action)
        {
            _lastAcces = DateTime.Now;
            action();
        }
        /// <summary>
        /// SetDelay before saving to DB.
        /// </summary>
        private Task Delay()
        {
            CancellationToken tok = _tokenSource.Token;
            // Generate task that will save and dispose object after some time with no connect
            return Task.Run(async () => {
                while ((DateTime.Now - _lastAcces).TotalMilliseconds < _delayBeforeSave && !tok.IsCancellationRequested)
                {
                    await Task.Delay(_delayBeforeSave - (int)(DateTime.Now - _lastAcces).TotalMilliseconds);
                }
            }, tok);   
        }
        private void SaveToDB()
        {
            if (!_disposedValue)
            {
                _managerStorage.Save();
                this.Dispose();
            }
        }
        /// <summary>
        /// Instantiate user changes.
        /// </summary>
        /// <param name="user">User that do changes.</param>
        public UserInfoChanges(IManagersStorage managerStorage, IUser user, LinkedList<UserInfoChanges> storagePlace)
        {
            User = user;
            _managerStorage = managerStorage;
            _storagePlace = storagePlace;
            _lastAcces = DateTime.Now;
            _savingToDB = Delay().ContinueWith(task => SaveToDB, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
        #region Setting in buffer
        /// <summary>
        /// Set new number of item to buffer.
        /// </summary>
        /// <param name="item">Item and item number to set in buffer.</param>
        public void ChangeItemList(KeyValuePair<string, int> item) => Refresh(() => _managerStorage.GetItemsManager().Edit(item));
        /// <summary>
        /// Set new number of resource to buffer.
        /// </summary>
        /// <param name="resource">Resource and resource number to set in buffer.</param>
        public void ChangeResourceList(KeyValuePair<string, int> resource) => Refresh(() => _managerStorage.GetResourceManager().Edit(resource));
        /// <summary>
        /// Set new user credit number to buffer.
        /// </summary>
        /// <param name="credNum">New resource number.</param>
        public void ChangeCreditNumber(int credNum) => Refresh(() => _managerStorage.GetCreditManager().Edit(credNum));
        /// <summary>
        /// Change user profile info in buffer.
        /// </summary>
        /// <param name="profileInfo">User profile info.</param>
        public void ChangeProfileInfo(UserInfo profileInfo) => Refresh(() => _managerStorage.GetProfileManager().Edit(profileInfo));
        #endregion
        #region Getting data from buffer
        /// <summary>
        /// Get user current items.
        /// </summary>
        public Dictionary<string, int>? GetCurrentItemList()
        {
            _lastAcces = DateTime.Now;
            return _managerStorage.TryGetItemsManager()?.GetCurrent();
        }
        /// <summary>
        /// Get user current resources.
        /// </summary>
        public Dictionary<string, int>? GetCurrentResourceList()
        {
            _lastAcces = DateTime.Now;
            return _managerStorage.TryGetResourceManager()?.GetCurrent();
        }
        /// <summary>
        /// Get user current credits.
        /// </summary>
        public int? GetCurrentCreditNumber()
        {
            _lastAcces = DateTime.Now;
            return _managerStorage.TryGetCreditManager()?.GetCurrent();
        }
        /// <summary>
        /// Get user current profile info.
        /// </summary>
        public UserInfo? GetCurrentProfileInfo()
        {
            _lastAcces = DateTime.Now;
            return _managerStorage.TryGetProfileManager()?.GetCurrent();
        }
        #endregion
        #region Dispose
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _tokenSource.Cancel();
                    _storagePlace.Remove(this);
                    _savingToDB.Wait(100);
                }
                _storagePlace = null!;
                _savingToDB = null!;
                _disposedValue = true;
            }
        }
        /// <summary>
        /// Dispose user changes resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
