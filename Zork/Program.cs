using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DefaultGameFilename = "Zork.json";
            string GameFilename = args.Length > 0 ? args[(int)CommandLineArgument.GameFilename] : DefaultGameFilename;
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(GameFilename));
            game.Run();
        }

        private enum CommandLineArgument
        {
            GameFilename = 0
        }
    }
}
