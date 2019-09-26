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

            Application.Run(new FormC64Screen(new C64()));
        }
    }
}
