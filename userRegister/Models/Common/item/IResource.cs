using MongoDB.Bson;

namespace API.Models.Interfaces
{
    public interface IResource : IEqualityComparer<IResource>
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string[]? Location { get; set; }
        public string[] Type { get; set; }
        public bool Mastery { get; set; }
    }
}
