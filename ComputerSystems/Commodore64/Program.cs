using Commodore64;
using Commodore64.Enums;
using System;
using System.Windows.Forms;

namespace ComputerSystem.Commodore64 {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var c64 = new C64();

            var form = new FormC64Screen(c64);
            //var form = new FormSimpleCharacterBufferViewer(c64);

            form.KeyPress += (object sender, KeyPressEventArgs e) => {
                // PETSCII a = 65 dec
                // ASCII   a = 97 dec

                e.Handled = true;

                var key = (byte)e.KeyChar;

                switch (key) {

                    case (byte)Keys.Escape:
                        c64.Cpu.Memory[C64MemoryLocations.STOP_KEY_INDICATOR] = C64MemoryValues.STOP_KEY_PRESSED;
                        return;

                    case (byte)Keys.Back:
                        key = (byte)PetsciiCode.InstDel;
                        break;

                    default:
                        key = (key >= 97) ? (byte)(key - 32) : key;
                        break;

                }

                c64.Cpu.Memory[C64MemoryOffsets.KEYBOARD_BUFFER] = key;
                c64.Cpu.Memory[C64MemoryLocations.KEYBOARD_BUFFER_LENGTH] = 1;
            };

            form.KeyUp += (object sender, KeyEventArgs e) => {
                switch (e.KeyCode) {
                    case Keys.Escape:
                        c64.Cpu.Memory[C64MemoryLocations.STOP_KEY_INDICATOR] = C64MemoryValues.STOP_KEY_NOT_PRESSED;
                        break;
                }
            };

            form.KeyDown += (object sender, KeyEventArgs e) => {
                e.Handled = true;

                byte key = 0;

                // No modifiers
                if (e.Modifiers == Keys.None) {
                    switch (e.KeyCode) {

                    }

                    // CTRL modifier
                } else if (e.Modifiers == Keys.Control) {
                    switch (e.KeyCode) {
                        case Keys.D0:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl0;
                            break;
                        case Keys.D1:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl1;
                            break;
                        case Keys.D2:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl2;
                            break;
                        case Keys.D3:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl3;
                            break;
                        case Keys.D4:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl4;
                            break;
                        case Keys.D5:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl5;
                            break;
                        case Keys.D6:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl6;
                            break;
                        case Keys.D7:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl7;
                            break;
                        case Keys.D8:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl8;
                            break;
                        case Keys.D9:
                            if (e.Control) key = (byte)PetsciiCode.Ctrl9;
                            break;
                    }

                    // Shift+CTRL modifier
                } else if (e.Modifiers == (Keys.Shift | Keys.Control)) {
                    switch (e.KeyCode) {
                        case Keys.D1:
                            if (e.Control) key = (byte)PetsciiCode.Commodore1;
                            break;
                        case Keys.D2:
                            if (e.Control) key = (byte)PetsciiCode.Commodore2;
                            break;
                        case Keys.D3:
                            if (e.Control) key = (byte)PetsciiCode.Commodore3;
                            break;
                        case Keys.D4:
                            if (e.Control) key = (byte)PetsciiCode.Commodore4;
                            break;
                        case Keys.D5:
                            if (e.Control) key = (byte)PetsciiCode.Commodore5;
                            break;
                        case Keys.D6:
                            if (e.Control) key = (byte)PetsciiCode.Commodore6;
                            break;
                        case Keys.D7:
                            if (e.Control) key = (byte)PetsciiCode.Commodore7;
                            break;
                        case Keys.D8:
                            if (e.Control) key = (byte)PetsciiCode.Commodore8;
                            break;
                    }

                    // Shift modifier
                } else if (e.Modifiers == Keys.Shift) {
                    switch (e.KeyCode) {
                        case Keys.Home:
                            key = (byte)PetsciiCode.ShiftClrHome;
                            break;

                        case Keys.Back:
                            key = (byte)PetsciiCode.ShiftInstDel;
                            break;
                    }
                }

                if (key == 0) return;
                c64.Cpu.Memory[C64MemoryOffsets.KEYBOARD_BUFFER] = key;
                c64.Cpu.Memory[C64MemoryLocations.KEYBOARD_BUFFER_LENGTH] = 1;
            };


            c64.Run();
            Application.Run(form);
        }
    }
}
