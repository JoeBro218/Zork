using System;
using Newtonsoft.Json;
using System.IO;

namespace Zork
{
    public class Game
    {
        private int Moves = 0;
        private int Score = 0;
        public World World { get; set;}

        [JsonIgnore]
        public Player Player { get; private set;}

        public string WelcomeMessage { get; set; }

        public string ExitMessage { get; set; }

        [JsonIgnore]
        public bool IsRunning { get; set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }


        public void Run()
        {
            Console.WriteLine(WelcomeMessage);

            IsRunning = true;
            Room previousRoom = null;
            while (IsRunning)
            {
                Console.WriteLine(Player.Location);
                if (previousRoom != Player.Location)
                {
                    Console.WriteLine(Player.Location.Description);
                    previousRoom = Player.Location;
                }
                Console.Write("> ");

                Commands command = ToCommand(Console.ReadLine().Trim());

                switch (command)
                {
                    case Commands.QUIT:
                        IsRunning = false;
                        Moves += 1;
                        Console.WriteLine($"Your final score is {Score} and the amount of moves you made are {Moves}.");
                        Console.WriteLine(ExitMessage);
                        break;

                    case Commands.LOOK:
                        Console.WriteLine(Player.Location.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        Console.WriteLine(Player.Move((Directions)command) ? $"You moved {command}." : "The way is shut.");
                        Moves += 1;
                        break;

                    case Commands.REWARD:
                        Random RanNum = new Random();
                        int Num = RanNum.Next(1, 11);
                        string message = "place holder";
                        if (Num == 1)
                        {
                            message = "A rock. It's worthless.";
                        }
                        else if (Num > 1 && Num < 10)
                        {
                            message = "Woah! Free Coin!";
                            Score += 1;
                        }
                        else if (Num >= 10)
                        {
                            message = "Shinny Gold Egg!";
                            Score += 10;
                        }
                        Moves += 1;
                        Console.WriteLine(message);
                        break;

                    case Commands.SCORE:
                        Moves += 1;
                        Console.WriteLine($"Your score is {Score} and you have made {Moves} moves.");
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();
            
            return game;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}
