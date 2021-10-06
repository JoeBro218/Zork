using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork
{
    public class Room
    {
        public string Name { get;}
        public string Description { get; set; }

        [JsonIgnore]
        public Dictionary<Directions, Room> Neighbors { get; set; }

        [JsonProperty(PropertyName = "Neighbors")]
        public Dictionary<Directions, string> NeighborNames { get; set; }

        public Room(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        public void UpdatingNeighbors(World world)
        {
            Neighbors = new Dictionary<Directions, Room>();
            foreach(var (direction, name) in NeighborNames)
            {
                Neighbors.Add(direction, world.RoomsByName[name]);
            }
        }

        public override string ToString() => Name;
    }
}
