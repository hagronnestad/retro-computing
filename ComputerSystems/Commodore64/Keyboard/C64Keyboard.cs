using System.Windows.Forms;

namespace Commodore64.Keyboard
{
    public class C64Keyboard
    {
        /// <summary>
        /// This keyboard matrix maps KeyCodes to rows/columns for the C64.
        /// </summary>
        public static Keys[,] Matrix = new Keys[8, 8] {
            { Keys.Back, Keys.Return, Keys.Right, Keys.F7, Keys.F1, Keys.F3, Keys.F5, Keys.Down },
            { Keys.D3, Keys.W, Keys.A, Keys.D4, Keys.Z, Keys.S, Keys.E, Keys.ShiftKey },
            { Keys.D5, Keys.R, Keys.D, Keys.D6, Keys.C, Keys.F, Keys.T, Keys.X },
            { Keys.D7, Keys.Y, Keys.G, Keys.D8, Keys.B, Keys.H, Keys.U, Keys.V },
            { Keys.D9, Keys.I, Keys.J, Keys.D0, Keys.M, Keys.K, Keys.O, Keys.N },
            { Keys.Oemplus, Keys.P, Keys.L, Keys.Oem4, Keys.OemPeriod, Keys.Oem3, Keys.Oem6, Keys.Oemcomma },
            { Keys.Oem102, Keys.Oem1, Keys.Oem7, Keys.Home, Keys.ShiftKey, Keys.Oem2, Keys.PageUp, Keys.OemMinus },
            { Keys.D1, Keys.Escape, Keys.ControlKey, Keys.D2, Keys.Space, Keys.Menu, Keys.Q, Keys.Pause },
        };

        // This shows the actual keyboard matrix for a C64 keyboard.
        //
        // public static Key[,] Matrix = new Key[8, 8] {
        //     { Insert/Delete, Return, Cursor Left/Right, F7, F1, F3, F5, Cursor Up/Down},
        //     { 3, W, A, 4, Z, S, E, Left Shift },
        //     { 5, R, D, 6, C, F, T, X },
        //     { 7, Y, G, 8, B, H, U, V },
        //     { 9, I, J, 0, M, K, O, N },
        //     { +, P, L, -, ., :, @, , },
        //     { £, *, ;, Clear/Home, Right Shift, =, Up Arrow, / },
        //     { 1, Left Arrow, Control, 2, Space, Commodore, Q, Run/Stop },
        // };

        // These mappings only apply to a Norwegian keyboard layout,
        // but the KeyCode should help to find the keys on other keyboard layouts.
        //
        // Oem102 = Key on the left side of "Z". Used for "£"-key.
        // Oem1 = Second key on the right side of "P". Used for "*"-key.
        // Oem2 = Third key on the right side of "L". Used for "="-key.
        // Oem3 = Key on the right side of "L". Used for ":"-key.
        // Oem4 = Second key on the right side of "0". Used for "-"-key.
        // Oem5 = Key on the left side of "1". Used for "<-"-key.
        // Oem6 = Key on the right side of "P". Used for "@"-key.
        // Oem7 = Second key on the right side of "L". Used for ";"-key.
        // OemMinus = Third key on the right side of "M". Used for "/"-key.
        // OemPlus = Key on the right side of "0". Used for "+"-key.
    }
}