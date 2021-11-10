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

        public IOutputService Output { get; set; }

        [JsonIgnore]
        public bool IsRunning { get; set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }


        public void Run()
        {
            Output.WriteLine(WelcomeMessage);

            IsRunning = true;
            Room previousRoom = null;
            while (IsRunning)
            {
                Output.WriteLine(Player.Location);
                if (previousRoom != Player.Location)
                {
                    Output.WriteLine(Player.Location.Description);
                    previousRoom = Player.Location;
                }
                Output.Write("> ");

                Commands command = ToCommand(Console.ReadLine().Trim());

                switch (command)
                {
                    case Commands.QUIT:
                        IsRunning = false;
                        Moves += 1;
                        Output.WriteLine($"Your final score is {Score} and the amount of moves you made are {Moves}.");
                        Output.WriteLine(ExitMessage);
                        break;

                    case Commands.LOOK:
                        Output.WriteLine(Player.Location.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        Output.WriteLine(Player.Move((Directions)command) ? $"You moved {command}." : "The way is shut.");
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
                        Output.WriteLine(message);
                        break;

                    case Commands.SCORE:
                        Moves += 1;
                        Output.WriteLine($"Your score is {Score} and you have made {Moves} moves.");
                        break;

                    default:
                        Output.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        public static Game Load(string filename, IOutputService output)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();
            game.Output = output;
            
            return game;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}
