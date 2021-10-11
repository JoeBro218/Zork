using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DefaultGameFilename = "Zork.json";
            string GameFilename = args.Length > 0 ? args[(int)CommandLineArgument.GameFilename] : DefaultGameFilename;

            Game game = Game.Load(GameFilename);
            game.Run();

        }

        private enum CommandLineArgument
        {
            GameFilename = 0
        }
    }
}
