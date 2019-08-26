using System;
using System.Linq;

namespace Cpu6502 {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("6502.NET\n");

            Console.WriteLine("Creating CPU instance and caching opcodes...\n");
            var p = new Cpu6502();

            Console.WriteLine($"Implemented OpCodes: {p.OpCodes.Select(x => x.Name).Distinct().Count()} / 56");
            Console.WriteLine($"Implemented OpCodes including different adressing modes: {p.OpCodes.Count()} / 151");

            //Console.WriteLine("Press any key to continue...\n");
            //Console.ReadKey();

            foreach (var item in p.OpCodes) {
                Console.WriteLine($"Name = {item.Name}, Code = 0x{item.Code:X} AddressingMode = {item.AddressingMode}");
            }

            var programRelativeAddressing = new byte[] { 0xA9, 0x10, 0x85, 0x50, 0xC6, 0x50, 0xF0, 0x08, 0x4C, 0x01, 0x40, 0x00 };

            var program = programRelativeAddressing;

            Console.WriteLine();
            Console.WriteLine("Loading program into memory at 0x4000...");
            Array.Copy(program, 0, p.Memory, 0x4000, program.Length);

            Console.WriteLine("Set PC=0x4000");
            p.PC = 0x4000;

            Console.WriteLine("Press any key to step through program...\n");

            while (true) {
                Console.ReadKey();
                p.Step();
            }
        }
    }
}
