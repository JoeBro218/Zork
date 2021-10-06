using System;
using Newtonsoft.Json;
using System.Linq;

namespace Zork
{
    public class Player
    {
        public World World { get; }
        
        [JsonIgnore]
        public Room CurrentRoom { get; set; }

        public Room PreviousRoom { get; set; }

        public Player(World world, string startinglocation)
        {
            World = world;

            for(int Row = 0; Row < World.Rooms.GetLength(0); Row++)
            {
                for (int Column = 0; Column < World.Rooms.GetLength(1);Column++)
                {
                    if (World.Rooms[Row, Column].Name.Equals(startinglocation, StringComparison.OrdinalIgnoreCase))
                    {
                        Location = (Row, Column);
                        return;
                    }
                }
            }
        }

        public bool Move(Directions direction)
        {
            bool IsVaildMove = CurrentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (IsVaildMove)
            {
                CurrentRoom = neighbor;
            }

            return IsVaildMove;
        }



        private static readonly Commands[] Directions =
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };
    }
}
