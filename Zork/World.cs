using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Zork
{
    public class World
    {
        public Room[] Rooms{ get; set;}

        public Dictionary<string, Room> RoomsByName { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (Room room in Rooms)
            {
                RoomsByName.Add(room.Name, room);
            }

            foreach (Room room in Rooms)
            {
                room.UpdatingNeighbors(this);
            }
        }
    }
}
