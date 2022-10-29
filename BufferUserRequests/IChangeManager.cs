using Shared;

namespace BufferUserRequests
{
    /// <summary>
    /// Change manager that store and manage one type of user change.
    /// </summary>
    public interface IChangeManager<T, R>
    {
        /// <summary>
        /// Edit item in manages buffer.
        /// </summary>
        public void Edit(T item);
        /// <summary>
        /// Get current state from buffer.
        /// </summary>
        public  R GetCurrent();
    }
    /// <summary>
    /// Change manager that store and manage one type of user change.
    /// </summary>
    public interface IChangeManager<T>
    {
        /// <summary>
        /// Edit item in manages buffer.
        /// </summary>
        public void Edit(T item);
        /// <summary>
        /// Get current state from buffer.
        /// </summary>
        public T GetCurrent();
    }
}
