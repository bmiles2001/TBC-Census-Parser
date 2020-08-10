using NLua;
using System.Collections.Generic;
using System.IO;

namespace WowTBCLuaParser.Lua
{
    public class FileLoader
    {
        private NLua.Lua _lua;

        public FileLoader(string filepath)
        {
            bool fileExists = File.Exists(filepath);

            if (!fileExists)
            {
                throw new FileNotFoundException($"File not found at path: {Directory.GetCurrentDirectory()}\\{filepath}");
            }

            _lua = new NLua.Lua();
            _lua.DoFile(filepath);
        }

        public LuaTable GetLuaTable(string key)
        {
            return _lua.GetTable(key);
        }

        public IDictionary<object, object> GetTable(string key)
        {
            LuaTable luaTable = GetLuaTable(key);

            if (luaTable == null)
            {
                return new Dictionary<object, object>();
            }

            return _lua.GetTableDict(luaTable);
        }
    }
}