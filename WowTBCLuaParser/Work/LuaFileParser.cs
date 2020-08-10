using System;
using System.Linq;
using WowTBCLuaParser.Lua;
using WowTBCLuaParser.Model;

namespace WowTBCLuaParser.Work
{
    public class LuaFileParser
    {
        private FileLoader _fileLoader;
        private readonly FileResults _fileResults;
        private readonly string _factionKey = string.Empty;

        public LuaFileParser(string factionKey)
        {
            _factionKey = factionKey;
            _fileResults = new FileResults();
        }

        public void ProcessFile(string filepath)
        {
            _fileLoader = new FileLoader(filepath);
            _fileResults.Clear();
            _fileResults.Faction = _factionKey.Substring(_factionKey.LastIndexOf(".") + 1);

            foreach (var race in _fileLoader.GetTable(_factionKey))
            {
                string raceKey = $"{_factionKey}.{race.Key}";

                foreach (var className in _fileLoader.GetTable(raceKey))
                {
                    ProcessClass($"{raceKey}.{className.Key}");
                }
            }
        }

        public void ProcessClass(string classNameKey)
        {
            var fields = classNameKey.Split('.').TakeLast(3);
            var people = _fileLoader.GetTable(classNameKey);

            foreach (var person in people)
            {
                var personDetails = _fileLoader.GetTable($"{classNameKey}.{person.Key}").ToArray();

                int.TryParse(personDetails[0].Value?.ToString(), out int characterLevel);                

                _fileResults.Characters.Add(new Character {
                    Name = person.Key.ToString(),
                    Level = characterLevel,
                    Faction = fields?.ElementAt(0),
                    Race = fields?.ElementAt(1),
                    ClassName = fields?.ElementAt(2),
                    Guild = personDetails[1].Value?.ToString()
                });
            }
        }

        public void OutputResults(string filename)
        {
            var fileOutput = new FileOutput();
            
            fileOutput.AddText($"Processing Faction: {_fileResults.Faction}");
            fileOutput.AddText($"Processing Ran: {DateTime.Now}");


            fileOutput.AddSeparator();
            fileOutput.ProcessOutput("Race Counts", _fileResults.Characters.CountByRace());
            fileOutput.AddSeparator();
            fileOutput.ProcessOutput("MaxLevel Counts", _fileResults.Characters.CountMaxLevels());
            fileOutput.AddSeparator();
            fileOutput.ProcessOutput("Class Counts", _fileResults.Characters.CountByClass());
            fileOutput.AddSeparator();
            fileOutput.ProcessOutput("Guild Counts", _fileResults.Characters.CountByGuild());
            fileOutput.AddSeparator();
            fileOutput.ProcessOutput("Characters Between 10 and 70", _fileResults.Characters.Between10and70());
            //fileOutput.AddSeparator();
            //fileOutput.ProcessOutput("Characters Under Level 10", _fileResults.Characters.UnderLevel10());

            fileOutput.WriteToFile(filename);
        }
    }
}