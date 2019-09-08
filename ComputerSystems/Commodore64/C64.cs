using MicroProcessor.Cpu6502;
using System.Diagnostics;
using System.Threading;

namespace Commodore64 {
    public class C64 {



        public C64Memory Memory { get; private set; }
        public Cpu Cpu { get; private set; }

        public C64() {
            Memory = new C64Memory();
            Cpu = new Cpu(Memory);

            Cpu.Reset();
        }

        public void Run() {
            // CLOCK_PAL = 985248 Hz
            // CLOCK_NTSC = 1022727 Hz

            // CLOCK_VICII_PAL = 7881984 Hz
            // CLOCK_VICII_NTSC = 8181816 Hz

            var cpuClockSpeedPal = 1.0f / 985248;
            var swCpuClock = Stopwatch.StartNew();
            var swCiaInterrupt = Stopwatch.StartNew();


            new Thread(() => {
                while (true) {

                    // CPU clock
                    if (swCpuClock.Elapsed.TotalMilliseconds > cpuClockSpeedPal) {

                        // Cycle the CPU
                        Cpu.Cycle();

                        // I have to work out how to properly time this
                        Memory[C64MemoryLocations.CURRENT_RASTER_LINE] =
                            Memory[C64MemoryLocations.CURRENT_RASTER_LINE] == 0 ? (byte)1 : (byte)0;

                        swCpuClock.Restart();
                    }

                    // CIA interrupt
                    if (swCiaInterrupt.Elapsed.TotalMilliseconds > (1000 / 60)) {
                        Cpu.Interrupt();

                        swCiaInterrupt.Restart();
                    }

                }

            }).Start();
        }

    }
}
