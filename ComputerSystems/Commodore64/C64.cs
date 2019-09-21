using MicroProcessor.Cpu6502;
using System.Diagnostics;
using System.Threading;
using Extensions.Byte;
using System.Windows.Input;
using Hardware.Mos6526Cia;
using System.Threading.Tasks;

namespace Commodore64 {
    public class C64 {

        private bool _isRunnning = false;
        private TaskCompletionSource<bool> _tcsStop;

        public const int CLOCK_PAL = 985248;
        public const int CLOCK_NTSC = 1022727;
        public const int CLOCK_VICII_PAL = 7881984;
        public const int CLOCK_VICII_NTSC = 8181816;

        public Cia Cia { get; private set; }
        public C64Memory Memory { get; private set; }
        public Cpu Cpu { get; private set; }

        public C64() {
            Cia = new Cia();

            Cia.ReadDataPortB += (object sender, System.EventArgs e) => {
                if (Cia.DataDirectionA == 0xFF) ScanKeyboard();
            };

            Memory = new C64Memory(Cia);
            Cpu = new Cpu(Memory);

            Cpu.Reset();
        }

        public void Run() {
            if (_isRunnning) return;

            _isRunnning = true;
            _tcsStop = new TaskCompletionSource<bool>();

            var cpuClockSpeedPal = 1.0f / CLOCK_PAL;
            var swCpuClock = Stopwatch.StartNew();
            var swCiaInterrupt = Stopwatch.StartNew();


            new Thread(() => {
                while (_isRunnning) {

                    // CPU clock
                    if (swCpuClock.Elapsed.TotalMilliseconds > cpuClockSpeedPal) {

                        // Clock CIA 1
                        Cia.Clock();

                        // Cycle the CPU
                        Cpu.Cycle();

                        // I have to work out how to properly time this
                        Memory[C64MemoryLocations.CURRENT_RASTER_LINE] =
                            Memory[C64MemoryLocations.CURRENT_RASTER_LINE] == 0 ? (byte)1 : (byte)0;

                        swCpuClock.Restart();
                    }

                    // CIA interrupt
                    if (swCiaInterrupt.Elapsed.TotalMilliseconds > (1000 / 50)) {
                        Cpu.Interrupt();
                        swCiaInterrupt.Restart();
                    }

                }

                _tcsStop.SetResult(true);

            }) { ApartmentState = ApartmentState.STA }.Start();

        }

        public Task<bool> Stop() {
            _isRunnning = false;
            return _tcsStop.Task;
        }


        public void ScanKeyboard() {
            // The BASIC keyboard scanning routine checks if any key is pressed at all.
            // This is done by checking all rows at once.
            if (Cia.DataPortA == 0) {
                Cia.DataPortB = 0x00;
                return;
            }

            // No keys should be pressed when all rows are unset.
            if (Cia.DataPortA == 0xFF) {
                Cia.DataPortB = 0xFF;
                return;
            }

            var rowIndexes = ((byte)~Cia.DataPortA).GetSetBitsIndexes();

            foreach (var ri in rowIndexes) {
                byte data = 0;

                for (int ci = 0; ci < 8; ci++) {

                    var key = C64Keyboard.Matrix[ri, ci];
                    if (key == Key.None) continue;

                    if (Keyboard.IsKeyDown(key)) {
                        data |= (byte)(1 << ci);
                    }

                }

                Cia.DataPortB = (byte)~data;
            }
        }

    }
}
