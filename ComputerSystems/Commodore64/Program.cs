using Commodore64;
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

            c64.Run();
            Application.Run(form);
        }
    }
}
