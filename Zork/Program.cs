﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Zork
{
    class Program
    {

        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            const string RoomDescriptionFilename = "Rooms.txt";
            InitializeRoomDescriptions(RoomDescriptionFilename);

            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while(command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);
                if(previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
                Console.Write("> ");

                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = CurrentRoom.Description;
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        outputString = Move(command) ? $"You moved {command}." : "The way is shut.";
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
            }
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool didMove = false;

            switch(command)
            {
                case Commands.NORTH when Location.Row > 0:
                    Location.Row--;
                    didMove = true;
                    break;

                case Commands.SOUTH when Location.Row < Rooms.GetLength(1) - 1:
                    Location.Row++;
                    didMove = true;
                    break;

                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
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

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static Room[,] Rooms = 
        {           
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") }
        };

        private static void InitializeRoomDescriptions(string RoomDescriptionFilename)
        {
            var roomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)
            {
                roomMap.Add(room.Name, room);
            }

            const string delimiter = "##";
            const int expectedFieldCount = 2;

            string[] lines = File.ReadAllLines(RoomDescriptionFilename);
            foreach (string line in lines)
            {
                string[] fields = line.Split(delimiter);
                Assert.IsTrue(fields.Length == expectedFieldCount, "Invalid record.");

                (string name, string description) = (fields[(int)Fields.Name], fields[(int)Fields.Description]);

                roomMap[name].Description = description;
            }

            //roomMap["Dense Woods"].Description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight.";
            //roomMap["North of House"].Description = "You are facing the north side of a white house. There is no door here, and all windows are barred.";
            //roomMap["Clearing"].Description = "You are in a clearing, with a forest surrounding you on the west and south.";
            //roomMap["Forest"].Description = "This is a forest, with trees in all diresctions around you.";
            //roomMap["West of House"].Description = "This is an open field west of a white house, with a boarded front door.";
            //roomMap["Behind House"].Description = "You are behind the white house. In one corner of the house there is a window which is slightly ajar.";
            //roomMap["Rocky Trail"].Description = "You are on a rock-strewn trail.";
            //roomMap["South of House"].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
            //roomMap["Canyon View"].Description = "You are at the top of the Great Canyon on its south wall.";
        }

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private enum Fields
        {
            Name =  0,
            Description = 1
        }

        private static (int Row, int Column) Location = (1, 1);
    }
}
