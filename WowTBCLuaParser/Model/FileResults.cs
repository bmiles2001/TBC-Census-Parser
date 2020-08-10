using System.Collections.Generic;

namespace WowTBCLuaParser.Model
{
    public class FileResults
    {
        public string Faction { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();

        public void Clear()
        {
            Characters.Clear();
        }
    }
}