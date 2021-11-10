using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DefaultGameFilename = "Zork.json";
            string GameFilename = args.Length > 0 ? args[(int)CommandLineArgument.GameFilename] : DefaultGameFilename;

            ConsoleOutputService output = new ConsoleOutputService();
            Game game = Game.Load(GameFilename, output);
            game.Run();

        }

        private enum CommandLineArgument
        {
            GameFilename = 0
        }
    }
}
