using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Commodore64.Vic.Colors
{
    public class ColorManager
    {
        public static Dictionary<byte, Color> ColorMap;

        public static Color FromByte(byte number)
        {
            return ColorMap[number];
        }

        public static bool LoadPalette(PaletteDefinition pd)
        {
            if (pd == null) return false;

            ColorMap = new Dictionary<byte, Color>() {
                { 0, FromHexString(pd.Black) },
                { 1, FromHexString(pd.White) },
                { 2, FromHexString(pd.Red) },
                { 3, FromHexString(pd.Cyan) },
                { 4, FromHexString(pd.VioletPurple) },
                { 5, FromHexString(pd.Green) },
                { 6, FromHexString(pd.Blue) },
                { 7, FromHexString(pd.Yellow) },
                { 8, FromHexString(pd.Orange) },
                { 9, FromHexString(pd.Brown) },
                { 10, FromHexString(pd.LightRed) },
                { 11, FromHexString(pd.DarkGrey) },
                { 12, FromHexString(pd.Grey) },
                { 13, FromHexString(pd.LightGreen) },
                { 14, FromHexString(pd.LightBlue) },
                { 15, FromHexString(pd.LightGrey) },
            };

            return true;
        }

        private static Color FromHexString(string s)
        {
            int argb = int.Parse(s.Replace("#", ""), NumberStyles.HexNumber);
            return Color.FromArgb(argb);
        }
    }
}
