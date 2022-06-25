using MongoDB.Bson;

namespace API.Models
{
    public class Component : Resource
    {
        public int CreationTime { get; set; }
        public int Credits { get; set; }
        public List<Resource> NeededResources { get; set; }

        public Component() { }
        public Component(ObjectId id, string name, string type, int creationTime, int credits, List<Resource> neededRes, string location="", bool mastery=false) : base(id, name, type, location, mastery)
        {
            Id = id;
            Name = name;
            Location = location;
            Mastery = mastery;
            CreationTime = creationTime;
            Credits = credits;
            NeededResources = neededRes;
        }
        

    }
}
