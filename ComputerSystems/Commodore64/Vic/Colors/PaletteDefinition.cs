using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Commodore64.Vic.Colors
{
    public class PaletteDefinition
    {
        public string Black { get; set; }
        public string White { get; set; }
        public string Red { get; set; }
        public string Cyan { get; set; }
        public string VioletPurple { get; set; }
        public string Green { get; set; }
        public string Blue { get; set; }
        public string Yellow { get; set; }
        public string Orange { get; set; }
        public string Brown { get; set; }
        public string LightRed { get; set; }
        public string DarkGrey { get; set; }
        public string Grey { get; set; }
        public string LightGreen { get; set; }
        public string LightBlue { get; set; }
        public string LightGrey { get; set; }

        public static PaletteDefinition FromFile(string file = @"default.json")
        {
            if (string.IsNullOrWhiteSpace(file)) return null;

            var path = Path.Combine("Palettes", file);
            if (!File.Exists(path)) return null;

            var pd = JsonSerializer.Deserialize<PaletteDefinition>(File.ReadAllText(path));
            return pd;
        }

        public static PaletteDefinition FromVicePaletteFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return null;
            if (!File.Exists(file)) return null;

            var lines = File.ReadAllLines(file);

            var colors = new List<string>();

            foreach (var line in lines)
            {
                if (line.Trim().StartsWith('#')) continue;
                if (string.IsNullOrWhiteSpace(line.Trim())) continue;

                colors.Add(line.Replace(" ", ""));
            }

            if (colors.Count != 16) return null;

            return new PaletteDefinition()
            {
                Black = colors[0],
                White = colors[1],
                Red = colors[2],
                Cyan = colors[3],
                VioletPurple = colors[4],
                Green = colors[5],
                Blue = colors[6],
                Yellow = colors[7],
                Orange = colors[8],
                Brown = colors[9],
                LightRed = colors[10],
                DarkGrey = colors[11],
                Grey = colors[12],
                LightGreen = colors[13],
                LightBlue = colors[14],
                LightGrey = colors[15]
            };
        }

        public static PaletteDefinition ImportVicePaletteFile(string file)
        {
            var pd = FromVicePaletteFile(file);
            pd.ToFile($"{Path.GetFileNameWithoutExtension(file)}.json");
            return pd;
        }

        public void ToFile(string file)
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(Path.Combine("Palettes", file), json);
        }
    }
}
