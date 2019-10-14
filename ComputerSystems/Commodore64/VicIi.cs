using System;
using System.Drawing;
using Extensions.Byte;
using Extensions.Enums;

namespace Commodore64 {
    public class VicIi {

        public event EventHandler OnGenerateRasterLineInterrupt;

        // VIC-II has 64 registers (47 in use, $D02F-$D03F Unusable (17 bytes), $D040-$D3FF VIC-II register images (repeated every $40, 64 bytes))
        private byte[] _registers = new byte[0x40];


        private const byte REGISTER_SCREEN_CONTROL_0x11 = 0x11;
        private const byte REGISTER_CURRENT_RASTER_LINE_0x12 = 0x12;
        private const byte REGISTER_RASTER_LINE_IRQ_0x12 = 0x12;

        private const byte REGISTER_INTERRUPT_STATUS_0x19 = 0x19;
        private const byte REGISTER_INTERRUPT_CONTROL_0x1A = 0x1A;

        private int _rasterLineToGenerateInterruptAt = 0;

        public byte this[int index] {
            get {
                switch (index) {


                    case REGISTER_SCREEN_CONTROL_0x11:
                        // Bit #7 og 0x11 is set if current raster line > 255
                        _registers[index].SetBit(BitFlag.BIT_7, CurrentLine > 255);
                        
                        return _registers[index];

                    // Current raster line (bits #0-#7).
                    // There's an additional bit used (bit #7) in 0x11 for values > 255
                    case REGISTER_CURRENT_RASTER_LINE_0x12:
                        return CurrentLine > 255 ? (byte)(CurrentLine - 255) : (byte)CurrentLine;

                    default:
                        return _registers[index];
                }
            }
            set {
                switch (index) {

                    case REGISTER_SCREEN_CONTROL_0x11:
                        _registers[index] = value;
                        UpdateRasterLineToGenerateInterruptAt();
                        break;

                    // Raster line to generate interrupt at (bits #0-#7).
                    // There's an additional bit used (bit #7) in 0x11 for values > 255
                    case REGISTER_RASTER_LINE_IRQ_0x12:
                        _registers[index] = value;
                        UpdateRasterLineToGenerateInterruptAt();
                        break;

                    default:
                        _registers[index] = value;
                        break;
                }
            }
        }

        private void UpdateRasterLineToGenerateInterruptAt() {
            _rasterLineToGenerateInterruptAt = _registers[REGISTER_RASTER_LINE_IRQ_0x12];
            if (_registers[REGISTER_SCREEN_CONTROL_0x11].IsBitSet(BitFlag.BIT_7)) _rasterLineToGenerateInterruptAt += 255;
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

        // Screen control register
        public bool ScreenControlRegisterScreenHeight => this[REGISTER_SCREEN_CONTROL_0x11].IsBitSet(BitFlag.BIT_3); // False = 24 rows, True = 25 rows
        public bool ScreenControlRegisterScreenOffOn => this[REGISTER_SCREEN_CONTROL_0x11].IsBitSet(BitFlag.BIT_4);
        public bool ScreenControlRegisterTextModeBitmapMode => this[REGISTER_SCREEN_CONTROL_0x11].IsBitSet(BitFlag.BIT_5); // False = Text mode, True = Bitmap mode
        public bool ScreenControlRegisterExtendedBackgroundModeOn => this[REGISTER_SCREEN_CONTROL_0x11].IsBitSet(BitFlag.BIT_6); // True = Extended background mode on

        // Interrupt control register
        public bool InterruptControlRegisterRasterInterruptEnabled => this[REGISTER_INTERRUPT_CONTROL_0x1A].IsBitSet(BitFlag.BIT_0);
        public bool InterruptControlRegisterSpriteBackgroundCollisionInterruptEnabled => this[REGISTER_INTERRUPT_CONTROL_0x1A].IsBitSet(BitFlag.BIT_1);
        public bool InterruptControlRegisterSpriteSpriteCollisionInterruptEnabled => this[REGISTER_INTERRUPT_CONTROL_0x1A].IsBitSet(BitFlag.BIT_2);
        public bool InterruptControlRegisterLightPenInterruptEnabled => this[REGISTER_INTERRUPT_CONTROL_0x1A].IsBitSet(BitFlag.BIT_3);

        //public bool IsInBorder =>

        public VicIi() {

        }

        public void Cycle() {


            CurrentLineCycle++;

            if (CurrentLineCycle == 64) {
                CurrentLineCycle = 0;

                CurrentLine++;

                if (InterruptControlRegisterRasterInterruptEnabled && (CurrentLine == _rasterLineToGenerateInterruptAt)) {
                    OnGenerateRasterLineInterrupt?.Invoke(this, null);
                }

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
    }
}
