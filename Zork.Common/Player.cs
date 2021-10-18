using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zork
{
    public class Player
    {
        public World World { get; }
        
        [JsonIgnore]
        public Room Location { get; set; }

        [JsonIgnore]
        public string LocationName
        {
            get
            {
                return Location?.Name;
            }
            set
            {
                Location = World?.RoomsByName.GetValueOrDefault(value);
            }
        }

        public Player(World world, string startinglocation)
        {
            World = world;

            LocationName = startinglocation;
        }

        public bool Move(Directions direction)
        {
            bool IsVaildMove = Location.Neighbors.TryGetValue(direction, out Room neighbor);
            if (IsVaildMove)
            {
                Location = neighbor;
            }

            return IsVaildMove;
        }
    }
}
