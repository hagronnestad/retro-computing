﻿using System;
using System.IO;
using System.Linq;
using Hardware.Memory;
using MicroProcessor.Cpu6502;

namespace NesTest6502 {
    class Program {

        static MemoryBase<byte> Memory = new MemoryBase<byte>(0xFFFFFF);
        static Cpu Cpu = new Cpu(Memory);

        static string[] GoldenLog = File.ReadAllLines("nestest-simple.log");
        static int CurrentLogLine = 0;

        static bool Break = false;


        static void Main(string[] args) {
            Console.WriteLine("NES Test\n");

            Cpu.OnStep += Cpu_OnStep;

            // Load golden log
            

            // Load NES Test "ROM"
            var file = File.ReadAllBytes("nestest.nes");
            var data = file.Skip(0x10).ToArray();

            for (int i = 0; i < data.Length; i++) {
                Memory[0xC000 + i] = data[i];
            }

            // Set PC start
            Cpu.PC = 0xC000;

            Console.WriteLine("Running official op code test...");

            // Cycle CPU until all official op codes has been tested
            while (Break == false && Cpu.PC != 0xC6BD) {
                Cpu.Cycle();
            }
            Console.WriteLine("\nSUCCESS!\n");

        }

        private static void Cpu_OnStep(object sender, OpCode e) {
            var opCodeBytes = string.Join(" ", Memory._memory.Skip(e.OpCodeAddress).Take(e.Length).Select(x => $"{x:X2}")).PadRight(8);
            var logLine = $"{e.OpCodeAddress:X4}  {opCodeBytes}  {e.Name}  A:{Cpu.AR:X2} X:{Cpu.XR:X2} Y:{Cpu.YR:X2} P:{Cpu.SR.Register:X2} SP:{Cpu.SP:X2}";
            Console.WriteLine($"Current log line: {logLine}");
            Console.CursorTop--;

            if (logLine != GoldenLog[CurrentLogLine]) {
                Console.WriteLine($"MISMATCH!!!");
                Console.WriteLine($"RESULT: {logLine}");
                Console.WriteLine($"GOLDEN: {GoldenLog[CurrentLogLine]}");

                Break = true;
            }

            CurrentLogLine++;
        }
    }
}
