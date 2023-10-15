using Hardware.Memory;
using MicroProcessor.Cpu6502;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace NesTest6502
{
    class Program
    {

        static MemoryBase<byte> Memory = new MemoryBase<byte>(0xFFFFFF);
        static Cpu Cpu = new Cpu(Memory);

        static string[] GoldenLog = File.ReadAllLines("nestest-simple.log");
        static int CurrentLogLine = 0;

        static bool Break = false;


        static void Main(string[] args)
        {
            Console.WriteLine("NES Test");

            Cpu.OnStep += Cpu_OnStep;

            // Load golden log


            // Load NES Test "ROM"
            var file = File.ReadAllBytes("nestest.nes");
            var data = file.Skip(0x10).ToArray();

            for (int i = 0; i < data.Length; i++)
            {
                Memory[0xC000 + i] = data[i];
            }

            // Set PC start
            Cpu.PC = 0xC000;

            // Cycle CPU until all official op codes has been tested
            Console.WriteLine("\nRunning official op code test...");
            while (Break == false && Cpu.PC != 0xC6BD)
            {
                Cpu.Cycle();
                Thread.Sleep(TimeSpan.FromTicks(9000));
            }
            Console.WriteLine("\nSUCCESS!\n");


            // Cycle CPU until all unofficial op codes has been tested
            Console.WriteLine("\nRunning unofficial op code test...");
            while (Break == false && Cpu.PC != 0x0001)
            {
                try
                {
                    Cpu.Cycle();
                    Thread.Sleep(TimeSpan.FromTicks(9000));

                }
                catch (Exception)
                {
                    Console.WriteLine($"MISSING OP CODE: 0x{Memory._memory[Cpu.PC]:X2}");

                    Break = true;
                }
            }
            if (!Break) Console.WriteLine("\nSUCCESS!\n");

            Console.WriteLine($"PROGRESS: {CurrentLogLine}/{GoldenLog.Length} ({(CurrentLogLine * 100f / GoldenLog.Length):F1}%)");
        }

        private static void Cpu_OnStep(object sender, OpCode e)
        {
            var opCodeBytes = string.Join(" ", Memory._memory.Skip(e.OpCodeAddress).Take(e.Length).Select(x => $"{x:X2}")).PadRight(8);
            var logLine = $"{e.OpCodeAddress:X4}  {opCodeBytes} {e.Name.Replace("_", "*").PadLeft(4)}  A:{Cpu.AR:X2} X:{Cpu.XR:X2} Y:{Cpu.YR:X2} P:{Cpu.SR.Register:X2} SP:{Cpu.SP:X2}";
            Console.WriteLine($"Current log line: {logLine}");
            Console.CursorTop--;

            if (logLine != GoldenLog[CurrentLogLine])
            {
                Console.WriteLine($"MISMATCH!!!");
                Console.WriteLine($"RESULT: {logLine}");
                Console.WriteLine($"GOLDEN: {GoldenLog[CurrentLogLine]}");

                Break = true;
            }

            CurrentLogLine++;
        }
    }
}
