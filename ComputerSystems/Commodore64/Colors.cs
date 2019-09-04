using System.Collections.Generic;
using System.Drawing;

namespace Commodore64 {
    public class Colors {

        public static Color Black = Color.FromArgb(0, 0, 0);
        public static Color White = Color.FromArgb(255, 255, 255);
        public static Color Red = Color.FromArgb(136, 0, 0);
        public static Color Cyan = Color.FromArgb(170, 255, 238);
        public static Color VioletPurple = Color.FromArgb(204, 68, 204);
        public static Color Green = Color.FromArgb(0, 204, 85);
        public static Color Blue = Color.FromArgb(0, 0, 170);
        public static Color Yellow = Color.FromArgb(238, 238, 119);
        public static Color Orange = Color.FromArgb(221, 136, 85);
        public static Color Brown = Color.FromArgb(102, 68, 0);
        public static Color LightRed = Color.FromArgb(255, 119, 119);
        public static Color DarkGrey = Color.FromArgb(51, 51, 51);
        public static Color Grey = Color.FromArgb(119, 119, 119);
        public static Color LightGreen = Color.FromArgb(170, 255, 102);
        public static Color LightBlue = Color.FromArgb(0, 136, 255);
        public static Color LightGrey = Color.FromArgb(187, 187, 187);

        public static Dictionary<byte, Color> ColorMap = new Dictionary<byte, Color>() {
            { 0, Black },
            { 1, White },
            { 2, Red },
            { 3, Cyan },
            { 4, VioletPurple },
            { 5, Green },
            { 6, Blue },
            { 7, Yellow },
            { 8, Orange },
            { 9, Brown },
            { 10, LightRed },
            { 11, DarkGrey },
            { 12, Grey },
            { 13, LightGreen },
            { 14, LightBlue },
            { 15, LightGrey },
        };

        public static Color FromByte(byte number) {
            return ColorMap[number];
        }
    }
}
