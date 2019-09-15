using System.Windows.Input;

namespace Commodore64 {
    public class C64Keyboard {

        /// <summary>
        /// This keyboard matrix maps KeyCodes to rows/columns for the C64.
        /// </summary>
        public static Key[,] Matrix = new Key[8, 8] {
            { Key.Back, Key.Return, Key.Right, Key.F7, Key.F1, Key.F3, Key.F5, Key.Down },
            { Key.D3, Key.W, Key.A, Key.D4, Key.Z, Key.S, Key.E, Key.LeftShift },
            { Key.D5, Key.R, Key.D, Key.D6, Key.C, Key.F, Key.T, Key.X },
            { Key.D7, Key.Y, Key.G, Key.D8, Key.B, Key.H, Key.U, Key.V },
            { Key.D9, Key.I, Key.J, Key.D0, Key.M, Key.K, Key.O, Key.N },
            { Key.OemPlus, Key.P, Key.L, Key.Oem4, Key.OemPeriod, Key.Oem3, Key.Oem6, Key.OemComma },
            { Key.Oem102, Key.Oem1, Key.Oem7, Key.Home, Key.RightShift, Key.Oem2, Key.PageUp, Key.OemMinus },
            { Key.D1, Key.Oem5, Key.LeftCtrl, Key.D2, Key.Space, Key.RightCtrl, Key.Q, Key.Pause },
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