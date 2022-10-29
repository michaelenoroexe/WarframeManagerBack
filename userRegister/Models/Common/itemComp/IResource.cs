using MongoDB.Bson;

namespace API.Models.Common.ItemComp
{
    public interface IResource : IEqualityComparer<IResource>
    {
        public ObjectId Id { get; }
        public string StringID => Id.ToString();
        public int SetOwned(int number);
    }
}
