using Hardware.Memory;
using System.Drawing;

namespace Commodore64 {
    public class VicIi {

        public enum TvSystem {
            NTSC,
            PAL
        }

        public TvSystem CurrentTvSystem = TvSystem.PAL;

        public const int FULL_WIDTH = 504;
        public const int FULL_WIDTH_CYCLES = 63;
        public const int USABLE_WIDTH_BORDER = 403;
        public const int USABLE_WIDTH = 320;
        public const int USABLE_WIDTH_CYCLES = 40;

        public const int FULL_HEIGHT_NTSC = 262;
        public const int FULL_HEIGHT_PAL = 312;
        public const int USABLE_HEIGHT = 200;
        public const int USABLE_HEIGHT_BORDER = 284;
        public int CurrentLine = 0;
        public int CurrentLineCycle = 0;

        public int TotalCycles = 0;

        public Color[] ScreenBufferPixels = new Color[USABLE_WIDTH_BORDER * USABLE_HEIGHT_BORDER];

        public bool ScreenOn => (Read(0xD011) & 0b00010000) == 1;


        private readonly IMemory<byte> _bus;


        public VicIi(IMemory<byte> bus) {
            _bus = bus;
        }

        public void Cycle() {


            CurrentLineCycle++;

            if (CurrentLineCycle == 64) {
                CurrentLineCycle = 0;

                CurrentLine++;

                if ((CurrentTvSystem == TvSystem.PAL && CurrentLine == FULL_HEIGHT_PAL) || 
                    (CurrentTvSystem == TvSystem.NTSC && CurrentLine == FULL_HEIGHT_NTSC)) {

                    CurrentLine = 0;
                }
            }

            UpdateScreenBufferPixels();

            TotalCycles++;
        }

        private void UpdateScreenBufferPixels() {

        }

        public byte Read(int address) {
            return _bus[address];
        }
    }
}
