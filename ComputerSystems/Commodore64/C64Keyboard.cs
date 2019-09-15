using System.Windows.Input;

namespace Commodore64 {
    public class C64Keyboard {

        public static Key[,] Matrix = new Key[8, 8] {
            { Key.Back, Key.Return, Key.Right, Key.F7, Key.F1, Key.F3, Key.F5, Key.Down },
            { Key.D3, Key.W, Key.A, Key.D4, Key.Z, Key.S, Key.E, Key.LeftShift },
            { Key.D5, Key.R, Key.D, Key.D6, Key.C, Key.F, Key.T, Key.X },
            { Key.D7, Key.Y, Key.G, Key.D8, Key.B, Key.H, Key.U, Key.V },
            { Key.D9, Key.I, Key.J, Key.D0, Key.M, Key.K, Key.O, Key.N },
            { Key.OemPlus, Key.P, Key.L, Key.OemMinus, Key.OemPeriod, Key.NumPad0, Key.NumPad0, Key.OemComma },
            { Key.NumPad0, Key.NumPad0, Key.OemComma, Key.Home, Key.RightShift, Key.NumPad0, Key.Up, Key.NumPad0 },
            { Key.D1, Key.Left, Key.LeftCtrl, Key.D2, Key.Space, Key.RightCtrl, Key.Q, Key.Pause},
        };

    }
}