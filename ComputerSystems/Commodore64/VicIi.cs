using System.Drawing;

namespace Commodore64 {
    public class VicIi {

        private byte[] _registers = new byte[47];

        public byte this[int index] {
            get {
                return _registers[index];
            }
            set {
                _registers[index] = value;
            }
        }


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

        public bool ScreenOn => (this[0x11] & 0b00010000) == 1;

        //public bool IsInBorder =>

        public VicIi() {

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

            // TODO: Implement this properly (Raster Counter)
            this[0x12] = this[0x12] == 0 ? (byte)1 : (byte)0;

            TotalCycles++;
        }

        private void UpdateScreenBufferPixels() {

        }
    }
}
