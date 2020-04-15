using MicroProcessor.Cpu6502;
using System.Diagnostics;
using System.Threading;
using Extensions.Byte;
using System.Windows.Input;
using Hardware.Mos6526Cia;
using System.Threading.Tasks;
using System;
using static Commodore64.Vic.VicIi;
using Commodore64.Vic;
using Commodore64.Cia;
using Commodore64.Cartridge;

namespace Commodore64 {
    public class C64 {

        public const int CLOCK_PAL = 985248;
        public const int CLOCK_NTSC = 1022727;
        public const int CLOCK_VICII_PAL = 7881984;
        public const int CLOCK_VICII_NTSC = 8181816;


        private bool _isRunnning = false;
        private TaskCompletionSource<bool> _tcsStop;

        public double CpuClockSpeed { get; set; } = 1.0f / CLOCK_PAL;


        public Cia1 Cia { get; private set; }
        public Cia2 Cia2 { get; private set; }
        public VicIi Vic { get; private set; }
        public C64Bus Memory { get; private set; }
        public Cpu Cpu { get; private set; }
        public ICartridge Cartridge { get; set; }

        public bool KeyboardActivated { get; set; } = false;

        public C64() {
            
        }

        public void Initialize() {
            RemoveEventHandlers();

            Cia = new Cia1();
            Cia2 = new Cia2();
            Vic = new VicIi(TvSystem.PAL) {
                C64 = this
            };
            Memory = new C64Bus(Cia, Cia2, Vic);
            Cpu = new Cpu(Memory);

            if (Cartridge != null) Memory.InsertCartridge(Cartridge);

            AddEventHandlers();
            Cpu.Reset();
        }

        public void PowerOn() {
            if (_isRunnning) return;

            Initialize();

            _isRunnning = true;
            _tcsStop = new TaskCompletionSource<bool>();

            var swCpuClock = Stopwatch.StartNew();
            
            var t = new Thread(() => {
                while (_isRunnning) {
                    
                    // CPU clock
                    if (swCpuClock.Elapsed.TotalMilliseconds > CpuClockSpeed) {

                        // Clock CIA 1
                        Cia.Clock();

                        // Cycle VIC-II
                        Vic.Cycle();

                        // Cycle the CPU
                        Cpu.Cycle();

                        swCpuClock.Restart();
                    }
                }

                _tcsStop.SetResult(true);

            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public Task<bool> PowerOff() {
            if (_isRunnning == false) return Task.FromResult(true);

            _isRunnning = false;
            return _tcsStop.Task;
        }


        private void AddEventHandlers() {
            Cia.ReadDataPortB += CiaReadDataPortB;
            Cia.Interrupt += CiaInterrupt;
            Vic.OnGenerateRasterLineInterrupt += Vic_OnGenerateRasterLineInterrupt;
        }

        private void RemoveEventHandlers() {
            if (Cia != null) {
                Cia.ReadDataPortB -= CiaReadDataPortB;
                Cia.Interrupt -= CiaInterrupt;
                Vic.OnGenerateRasterLineInterrupt -= Vic_OnGenerateRasterLineInterrupt;
            }
        }

        private void CiaReadDataPortB(object sender, EventArgs e) {
            if (Cia.DataDirectionA == 0xFF) ScanKeyboard();
        }

        private void CiaInterrupt(object sender, EventArgs e) {
            Cpu.Interrupt();
        }

        private void Vic_OnGenerateRasterLineInterrupt(object sender, EventArgs e) {
            Cpu.Interrupt();
        }


        public void ScanKeyboard() {
            if (!KeyboardActivated) return;

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
