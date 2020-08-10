using Microsoft.Extensions.Configuration;
using System;
using WowTBCLuaParser.Work;

namespace WowTBCLuaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Wow TBC LUA Census Parser";

            try
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("app.settings.json")
                    .AddEnvironmentVariables();

                IConfiguration config = configurationBuilder.Build();

                RunLuaParser(config);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to continue..");
            Console.ReadKey(true);
        }

        static void RunLuaParser(IConfiguration config)
        {
            // server
            var server = config.GetValue<string>("Server:Name");
            var faction = config.GetValue<string>("Server:Faction");

            // output
            var outputFilename = config.GetValue<string>("Output:Filename");

            // dev
            var databaseKey = config.GetValue<string>("Dev:DatabaseKey");
            var sourceFileName = config.GetValue<string>("Dev:luaFile");

            string filename = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{outputFilename}";
            LuaFileParser luaFileParser = new LuaFileParser($"{databaseKey}.{server}.{faction}");

            try
            {
                luaFileParser.ProcessFile(sourceFileName);
                luaFileParser.OutputResults(filename);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }
        }
    }
}