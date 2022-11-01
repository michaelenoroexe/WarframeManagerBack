using MongoDB.Bson;

namespace API.Models.Common.ItemComp
{
    /// <summary>
    /// Main projection of items.
    /// </summary>
    public interface IResource : IEquatable<IResource>, ICloneable
    {
        /// <summary>
        /// Id of item in database.
        /// </summary>
        public ObjectId Id { get; }
        /// <summary>
        /// String id of item.
        /// </summary>
        public string StringID { get; }
        /// <summary>
        /// Change number of owned number of item.
        /// </summary>
        /// <param name="number">New number of item.</param>
        public void SetOwned(int number);
    }
}
