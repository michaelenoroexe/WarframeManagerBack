namespace BufferUserRequests
{
    /// <summary>
    /// Represent object that can save its state.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    public interface ISavable<S>
    {
        /// <summary>
        /// Save state to object in parameter.
        /// </summary>
        /// <param name="save">Object to save state in.</param>
        public void Save(ref S save);
    }
}
