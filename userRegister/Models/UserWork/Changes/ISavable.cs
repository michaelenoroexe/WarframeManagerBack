namespace API.Models.UserWork.Changes
{
    internal interface ISavable<S>
    {
        /// <summary>
        /// Save state to object in parameter.
        /// </summary>
        /// <param name="save">Object to save state in.</param>
        public void Save(ref S save);
    }
}
