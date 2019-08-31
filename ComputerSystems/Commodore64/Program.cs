using Commodore64;
using System;
using System.IO;
using System.Threading;
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

            var form = new FormSimpleCharacterBufferViewer(c64);

            form.KeyPress += (object sender, KeyPressEventArgs e) => {
                //PETSCII a = 65 dec
                //ASCII   a = 97 dec

                e.Handled = true;

                var key = (byte)e.KeyChar;

                switch (key) {

                    case (byte)Keys.Escape:
                        c64.Cpu.Memory[145] = 0x7F;
                        return;

                    default:
                        key = (key >= 97) ? (byte)(key - 32) : (byte)key;
                        break;

                }

                c64.Cpu.Memory[0x0277] = key;
                c64.Cpu.Memory[0xC6] = 1;
            };

            form.KeyUp += (object sender, KeyEventArgs e) => {
                switch (e.KeyCode) {
                    case Keys.Escape:
                        c64.Cpu.Memory[145] = 0xFF;
                        break;
                }
            };

            var romBasic = File.ReadAllBytes(@"basic.rom");
            var romChar = File.ReadAllBytes(@"char.rom");
            var romKernal = File.ReadAllBytes(@"kernal.rom");
            c64.Cpu.LoadMemory(romBasic, 0xA000);
            c64.Cpu.LoadMemory(romChar, 0xD000);
            c64.Cpu.LoadMemory(romKernal, 0xE000);
            c64.Cpu.Reset();

            new Thread(() => {
                while (true) {
                    c64.Cpu.Step();

                    //if (c64.Cpu.TotalInstructions % 100000 == 0) {
                    //    Debug.WriteLine($"{c64.Cpu.TotalInstructions}");
                    //    Debug.WriteLine($"{c64.Cpu.PC:X2}");
                    //}
                }

            }).Start();

            new Thread(() => {
                while (true) {
                    c64.Cpu.Memory[0xD012] = 0;
                    Thread.Sleep(20);
                }
            }).Start();

            Application.Run(form);
        }
    }
}
