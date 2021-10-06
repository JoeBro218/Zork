using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Zork
{
    public class Game
    {
        private int Moves = 0;
        private int Score = 0;
        public World World { get; set;}

        public string StartingLocation { get; set;}

        [JsonIgnore]
        public Player Player { get; private set;}

        public string WelcomeMessage { get; set;}

        public string ExitMessage { get; set;}

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Player = new Player(World, StartingLocation);
        }

        public void Run()
        {
            Console.WriteLine(WelcomeMessage);

            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Console.WriteLine(Player.CurrentRoom.Description);
                    previousRoom = Player.CurrentRoom;
                }
                Console.Write("> ");

                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = ExitMessage;
                        Console.WriteLine($"Your final score is {Score} and the amount of moves you made are {Moves}.");
                        Moves += 1;
                        break;

                    case Commands.LOOK:
                        outputString = Player.CurrentRoom.Description;
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        outputString = Player.Move(command) ? $"You moved {command}." : "The way is shut.";
                        Moves += 1;
                        break;

                    case Commands.REWARD:
                        Random RanNum = new Random();
                        int Num = RanNum.Next(1, 10);
                        string message = "place holder";
                        if(Num == 1)
                        {
                            message = "A rock. It's worthless.";
                        }
                        else if(Num > 1 && Num < 10)
                        {
                            message = "Woah! Free Coin!";
                            Score += 1;
                        }
                        else if(Num == 10)
                        {
                            message = "Shinny Gold Egg!";
                            Score += 10;
                        }
                        Moves += 1;
                        outputString = message;
                        break;

                    case Commands.SCORE:
                        Moves += 1;
                        outputString = $"Your score is {Score} and you have made {Moves} moves.";
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }
                Console.WriteLine(outputString);
            }
        }
        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}
