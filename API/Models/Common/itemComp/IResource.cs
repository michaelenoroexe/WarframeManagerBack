using MongoDB.Bson;

namespace API.Models.Common.ItemComp
{
    public interface IResource : IEquatable<IResource>, ICloneable
    {
        public ObjectId Id { get; }
        public string StringID { get; }
        public void SetOwned(int number);
    }
}
