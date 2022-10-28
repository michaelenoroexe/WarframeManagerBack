namespace API.Models.UserWork.Changes
{
    /// <summary>
    /// Interface if view and input changes are different.
    /// </summary>
    /// <typeparam name="T">Input change type.</typeparam>
    /// <typeparam name="R">View type.</typeparam>
    /// <typeparam name="S">Type of object to save in.</typeparam>
    internal interface ISavableChangeManager<T,R,S> : IChangeManager<T,R>, ISavable<S> { }
    /// <summary>
    /// Interface if view and input changes are the same.
    /// </summary>
    /// <typeparam name="T">Input change type.</typeparam>
    /// <typeparam name="S">Type of object to save in.</typeparam>
    internal interface ISavableChangeManager<T, S> : IChangeManager<T>, ISavable<S> { }
}
