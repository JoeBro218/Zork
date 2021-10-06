using System;
using Newtonsoft.Json;
using System.Linq;

namespace Zork
{
    public class Player
    {
        public World World { get; }
        
        [JsonIgnore]
        public Room CurrentRoom
        {
            get
            {
                return World.Rooms[Location.Row, Location.Column];
            }
        }

        public Player(World world, string startingLocation)
        {
            World = world;

            for(int Row = 0; Row < World.Rooms.GetLength(0); Row++)
            {
                for (int Column = 0; Column < World.Rooms.GetLength(1);Column++)
                {
                    if (World.Rooms[Row, Column].Name.Equals(startingLocation, StringComparison.OrdinalIgnoreCase))
                    {
                        Location = (Row, Column);
                        return;
                    }
                }
            }
        }

        public bool Move(Commands commands)
        {
            Assert.IsTrue(Directions.Contains(commands), "Invalid direction.");

            bool didMove = false;

            switch (commands)
            {
                case Commands.NORTH when Location.Row > 0:
                    Location.Row--;
                    didMove = true;
                    break;

                case Commands.SOUTH when Location.Row < World.Rooms.GetLength(1) - 1:
                    Location.Row++;
                    didMove = true;
                    break;

                case Commands.EAST when Location.Column < World.Rooms.GetLength(1) - 1:
                    Location.Column++;
                    didMove = true;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    didMove = true;
                    break;
            }

            return didMove;
        }



        private static readonly Commands[] Directions =
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private (int Row, int Column) Location;
    }
}
