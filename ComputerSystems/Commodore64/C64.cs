using Commodore64.Cartridge;
using Commodore64.Cia;
using Commodore64.Keyboard;
using Commodore64.Properties;
using Commodore64.Sid.NAudioImpl;
using Commodore64.Vic;
using Extensions.Byte;
using Hardware.Mos6526Cia;
using MicroProcessor.Cpu6502;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Commodore64.Vic.VicIi;

namespace Commodore64
{
    public class C64
    {

        public const int CLOCK_PAL = 985248;
        public const int CLOCK_NTSC = 1022727;
        public const int CLOCK_VICII_PAL = 7881984;
        public const int CLOCK_VICII_NTSC = 8181816;


        private bool _isRunnning = false;
        private TaskCompletionSource<bool> _tcsStop;

        public double CpuClockSpeedMultiplier = 1f;
        public double CpuClockSpeedMultiplierMin = 0.00001f;
        public double CpuClockSpeedMultiplierMax = 3f;

        public double CpuClockSpeedHz { get; set; } = CLOCK_PAL;
        public double CpuPeriodMilliseconds => ((1f / (CpuClockSpeedHz * CpuClockSpeedMultiplier)) * 1000f);

        public double CpuClockSpeedRealHz = 0f;
        public double CpuPeriodMillisecondsReal = 0f;

        public double CpuClockSpeedPercent => (CpuClockSpeedRealHz / CLOCK_PAL) * 100f;


        public Cia1 Cia { get; private set; }
        public Cia2 Cia2 { get; private set; }
        public VicIi Vic { get; private set; }
        public NAudioSid Sid { get; set; }
        public C64Bus Memory { get; private set; }
        public Cpu Cpu { get; private set; }
        public ICartridge Cartridge { get; set; }

        public IC64KeyboardInputProvider C64KeyboardInputProvider { get; set; }

        public C64()
        {

        }

        public void Initialize()
        {
            RemoveEventHandlers();

            Cia = new Cia1();
            Cia2 = new Cia2();
            Vic = new VicIi(TvSystem.PAL)
            {
                C64 = this
            };

            Sid = new NAudioSid();

            Memory = new C64Bus(Cia, Cia2, Vic, Sid);
            Cpu = new Cpu(Memory);

            if (Cartridge != null) Memory.InsertCartridge(Cartridge);

            AddEventHandlers();
        }

        public void PatchKernalRomTextColor(byte color)
        {
            Memory._romKernal.IsReadOnly = false;
            Memory._romKernal[0x0535] = color;
            Memory._romKernal.IsReadOnly = true;
        }

        public void PowerOn()
        {
            if (_isRunnning) return;

            Initialize();

            // Apply ROM patches
            if (Settings.Default.KernalWhiteTextColor) PatchKernalRomTextColor(0x01);

            Cpu.Reset();
            Sid.Play();

            _isRunnning = true;
            _tcsStop = new TaskCompletionSource<bool>();

            var swCpuClock = Stopwatch.StartNew();

            var t = new Thread(() =>
            {
                while (_isRunnning)
                {

                    // CPU clock
                    if (swCpuClock.Elapsed.TotalMilliseconds >= CpuPeriodMilliseconds)
                    {
                        CpuPeriodMillisecondsReal = swCpuClock.Elapsed.TotalMilliseconds;
                        CpuClockSpeedRealHz = 1 / (CpuPeriodMillisecondsReal / 1000.0f);

                        swCpuClock.Restart();

                        // Clock CIA 1
                        Cia.Clock();

                        // Cycle VIC-II
                        Vic.Cycle();

                        // Cycle the CPU
                        Cpu.Cycle();
                    }
                }

                _tcsStop.SetResult(true);

            });

            t.Start();
        }

        public Task<bool> PowerOff()
        {
            if (_isRunnning == false) return Task.FromResult(true);

            Sid.Stop();

            _isRunnning = false;
            return _tcsStop.Task;
        }


        private void AddEventHandlers()
        {
            Cia.ReadDataPortB += CiaReadDataPortB;
            Cia.Interrupt += CiaInterrupt;
            Vic.OnGenerateRasterLineInterrupt += Vic_OnGenerateRasterLineInterrupt;
        }

        private void RemoveEventHandlers()
        {
            if (Cia != null) Cia.ReadDataPortB -= CiaReadDataPortB;
            if (Cia != null) Cia.Interrupt -= CiaInterrupt;
            if (Vic != null) Vic.OnGenerateRasterLineInterrupt -= Vic_OnGenerateRasterLineInterrupt;
        }

        private void CiaReadDataPortB(object sender, EventArgs e)
        {
            // Keyboard Scanning
            if (Cia.DataDirectionA == 0xFF)
            {
                // The BASIC keyboard scanning routine checks if any key is pressed at all.
                // This is done by checking all rows at once.
                if (Cia.DataPortA == 0)
                {
                    Cia.DataPortB = 0x00;
                    return;
                }

                // No keys should be pressed when all rows are unset.
                if (Cia.DataPortA == 0xFF)
                {
                    Cia.DataPortB = 0xFF;
                    return;
                }

                var rowIndexes = ((byte)~Cia.DataPortA).GetSetBitsIndexes();

                foreach (var ri in rowIndexes)
                {
                    byte data = 0;

                    for (int ci = 0; ci < 8; ci++)
                    {
                        var key = C64Keyboard.Matrix[ri, ci];
                        if (key == Keys.None) continue;

                        if (C64KeyboardInputProvider != null && C64KeyboardInputProvider.IsKeyDown(key))
                        {
                            data |= (byte)(1 << ci);
                        }
                    }

                    Cia.DataPortB = (byte)~data;
                }
            }
        }

        private void CiaInterrupt(object sender, EventArgs e)
        {
            Cpu.Interrupt();
        }

        private void Vic_OnGenerateRasterLineInterrupt(object sender, EventArgs e)
        {
            Cpu.Interrupt();
        }
    }
}
