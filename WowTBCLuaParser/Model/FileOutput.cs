using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WowTBCLuaParser.Model
{
    public class FileOutput
    {
        private readonly List<string> _output;

        public FileOutput()
        {
            _output = new List<string>();
        }

        public void ProcessOutput(string header, IEnumerable<string> output)
        {
            _output.Add(WriteToScreen(header));
            _output.AddRange(output.Select(thing => WriteToScreen(thing)));
        }

        public void AddSeparator(string text = null)
        {
            if (text == null)
            {
                text = Environment.NewLine;
            }

            _output.Add(WriteToScreen(text));
        }

        public void AddText(string text)
        {
            _output.Add(WriteToScreen(text));
        }

        public void WriteToFile(string filename)
        {
            File.WriteAllLines(filename, _output);
        }

        private string WriteToScreen(string output)
        {
            Console.WriteLine(output); // display to screen
            return output;
        }
    }
}