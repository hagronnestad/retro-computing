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
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey();

            foreach (var item in p.OpCodes) {
                Console.WriteLine($"Name = {item.Name}, Code = 0x{item.Code:X} AddressingMode = {item.AddressingMode}");
            }

            Console.ReadKey();
        }
    }
}
