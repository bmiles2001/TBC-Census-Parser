using System.Collections.Generic;
using System.Linq;
using WowTBCLuaParser.Model;

namespace WowTBCLuaParser.Work
{
    public static class CharacterExtensionMethods
    {
        public static IEnumerable<string> CountByRace(this List<Character> characters)
        {
            return characters
                .GroupBy(x => new { x.Race })
                .Select(x => new CharacterMetrics
                {
                    Field = x.Key.Race,
                    Count = x.Count(),
                    Output = $"{x.Key.Race}: {x.Count()}"
                })
                .OrderBy(x => x.Field)
                .AddTotalRow();
        }

        public static IEnumerable<string> CountByClass(this List<Character> characters)
        {
            return characters
                .GroupBy(x => new { x.ClassName })
                .Select(x => new CharacterMetrics
                {
                    Field = x.Key.ClassName,
                    Count = x.Count(),
                    Output = $"{x.Key.ClassName}: {x.Count()}"
                })
                .OrderBy(x => x.Field)
                .AddTotalRow();
        }

        public static IEnumerable<string> CountByGuild(this List<Character> characters)
        {
            return characters
                .GroupBy(x => new { x.Guild })
                .Select(x => new CharacterMetrics
                {
                    Field = x.Key.Guild,
                    Count = x.Count(),
                    Output = x.Key.Guild == string.Empty ? $"Character not in guild: {x.Count()}" : $"{x.Key.Guild}: {x.Count()}"
                })
                .OrderByDescending(x => x.Count)
                .Where(x => x.Count >= 25) // Total Member Count >= 25
                .AddTotalRow();
        }

        public static IEnumerable<string> CountMaxLevels(this List<Character> characters)
        {
            return characters
                .Where(x => x.Level == 70)
                .GroupBy(x => new { x.ClassName })
                .Select(x => new CharacterMetrics
                {
                    Field = x.Key.ClassName,
                    Count = x.Count(),
                    Output = $"{x.Key.ClassName}: {x.Count()}"
                })
                .OrderBy(x => x.Field)
                .AddTotalRow();
        }

        public static IEnumerable<string> Between10and70(this List<Character> characters)
        {
            return characters
                .Where(x => x.Level >= 10 && x.Level <= 70)
                .GroupBy(x => new { x.Level })
                .OrderBy(x => x.Key.Level)
                .Select(x => $"Level {x.Key.Level}: {x.Count()}");
        }

        public static IEnumerable<string> UnderLevel10(this List<Character> characters)
        {
            return characters
                .Where(x => x.Level < 10)
                .OrderBy(x => x.Level)
                .Select(x => $"Level {x.Level}: {x.Name}");
        }

        // Adds the total row and cleans up the LINQ a bit
        private static IEnumerable<string> AddTotalRow(this IEnumerable<CharacterMetrics> characterMetrics)
        {
            CharacterMetrics totalRow = new CharacterMetrics
            {
                Output = $"Total: {characterMetrics.Sum(x => x.Count)}",
                Total = characterMetrics.Sum(x => x.Count)
            };

            return characterMetrics
                .Append(totalRow)
                .Select(x => x.Output);
        }
    }
}