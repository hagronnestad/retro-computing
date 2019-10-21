using System;
using System.Drawing;
using Extensions.Byte;
using Extensions.Enums;

namespace Commodore64 {
    public class VicIi {

        /// <summary>
        /// http://www.zimmers.net/cbmpics/cbm/c64/vic-ii.txt
        /// </summary>

        public event EventHandler OnGenerateRasterLineInterrupt;
        public event EventHandler OnLastScanLine;

        // VIC-II has 64 registers (47 in use, $D02F-$D03F Unusable (17 bytes), $D040-$D3FF VIC-II register images (repeated every $40, 64 bytes))
        public byte[] _registers = new byte[0x40];

        //TODO: Temporary dependency
        public C64 C64;

        private const byte REGISTER_SCREEN_CONTROL_0x11 = 0x11;
        private const byte REGISTER_CURRENT_RASTER_LINE_0x12 = 0x12;
        private const byte REGISTER_RASTER_LINE_IRQ_0x12 = 0x12;

        private const byte REGISTER_INTERRUPT_STATUS_0x19 = 0x19;
        private const byte REGISTER_INTERRUPT_CONTROL_0x1A = 0x1A;

        public Color[,] ScreenBufferPixels { get; }

        private readonly TvSystem _tvSystem;
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

        public bool InVerticalBlank => CurrentLine >= 300 || CurrentLine <= 15;

        //public bool InBorder =>
        //    (CurrentLine >= USABLE_HEIGHT + (USABLE_HEIGHT_BORDER / 2) && CurrentLine <= (USABLE_HEIGHT_BORDER / 2)) ||
        //    (CurrentLineCycle >= USABLE_WIDTH + (USABLE_HEIGHT_BORDER / 2) && CurrentLineCycle <= (USABLE_HEIGHT_BORDER / 2));

        public int TotalCycles = 0;

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



        public Rectangle FullFrame;
        public Rectangle BorderFrame;
        public Rectangle DisplayFrame;


        public VicIi(TvSystem tvSystem) {
            _tvSystem = tvSystem;

            var fullHeight = tvSystem == TvSystem.PAL ? FULL_HEIGHT_PAL : FULL_HEIGHT_NTSC;

            FullFrame = new Rectangle(0, 0, FULL_WIDTH, fullHeight);
            BorderFrame = new Rectangle((FULL_WIDTH - USABLE_WIDTH_BORDER) / 2, (fullHeight - USABLE_HEIGHT_BORDER) / 2, USABLE_WIDTH_BORDER, USABLE_HEIGHT_BORDER);
            DisplayFrame = new Rectangle(BorderFrame.X + 42, BorderFrame.Y + 42, USABLE_WIDTH, USABLE_HEIGHT);

            ScreenBufferPixels = new Color[fullHeight, FULL_WIDTH];
        }

        public void Cycle() {

            // Every cycle draws 8 pixels to the screen

            var p = GetScanlinePoint();
            if (IsInBorder(p)) {
                RenderBorder(p);
            }


            CurrentLineCycle++;

            // Every line takes 63 cycles
            if (CurrentLineCycle == 64) {
                CurrentLineCycle = 0;

                CurrentLine++;

                // Generate raster interrupt if the current line equals interrupt line
                // TODO: Implement the Interrupt latch register ($D019) !!!!!!!
                if (InterruptControlRegisterRasterInterruptEnabled && (CurrentLine == _rasterLineToGenerateInterruptAt)) {
                    OnGenerateRasterLineInterrupt?.Invoke(this, null);
                }

                if ((CurrentTvSystem == TvSystem.PAL && CurrentLine == FULL_HEIGHT_PAL) ||
                    (CurrentTvSystem == TvSystem.NTSC && CurrentLine == FULL_HEIGHT_NTSC)) {

                    CurrentLine = 0;

                    OnLastScanLine?.Invoke(this, null);

                    // Frame based character mode rendering
                    UpdateScreenBufferPixels();
                }
            }


            TotalCycles++;
        }

        private void RenderBorder(Point scanlinePoint) {

            var bgColor = Colors.FromByte((byte)(_registers[0x20] & 0b00001111));

            for (int i = 0; i < 8; i++) {
                ScreenBufferPixels[scanlinePoint.Y, scanlinePoint.X + i] = bgColor;
            }

        }

        private Point GetScanlinePoint() {
            var p = new Point {
                Y = CurrentLine,
                X = CurrentLineCycle * 8
            };

            return p;
        }

        private bool IsInBorder(Point p) {
            if (DisplayFrame.Contains(p)) return false;
            return BorderFrame.Contains(p);
        }




        private void RenderCharacterMode() {
        
        }



        public void UpdateScreenBufferPixels() {
            var bgColor = Colors.FromByte((byte)(C64.Vic._registers[0x21] & 0b00001111));

            for (var i = 0; i < 1000; i++) {
                var petsciiCode = vicRead((ushort)(getScreenMemoryPointer() + i));
                var fgColor = Colors.FromByte((byte)(C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + i] & 0b00001111));
                //var fgColor = Colors.FromByte((byte)(vicRead((ushort)(0x0800 + i)) & 0b00001111));

                var line = (i / 40);
                var characterInLine = i % 40;

                for (int row = 0; row <= 7; row++) {
                    var charRow = vicRead((ushort)(getCharacterMemoryPointer() + (petsciiCode * 8) + row));

                    for (int col = 0; col <= 7; col++) {
                        ScreenBufferPixels[DisplayFrame.Y + (line * 8) + row, DisplayFrame.X + (characterInLine * 8) + col] = charRow.IsBitSet(7 - (BitIndex)col) ? fgColor : bgColor;
                    }

                }

            }
        }

        public int getScreenMemoryPointer() {
            var bit4to7 = (C64.Memory.Read(0xD018) >> 4) & 0b00001111;

            switch (bit4to7) {
                case 0b0000:
                    return 0x0000;
                case 0b0001:
                    return 0x0400;
                case 0b0010:
                    return 0x0800;
                case 0b0011:
                    return 0x0C00;
                case 0b0100:
                    return 0x1000;
                case 0b0101:
                    return 0x1400;
                case 0b0110:
                    return 0x1800;
                case 0b0111:
                    return 0x1C00;
                case 0b1000:
                    return 0x2000;
                case 0b1001:
                    return 0x2400;
                case 0b1010:
                    return 0x2800;
                case 0b1011:
                    return 0x2C00;
                case 0b1100:
                    return 0x3000;
                case 0b1101:
                    return 0x3400;
                case 0b1110:
                    return 0x3800;
                case 0b1111:
                    return 0x3C00;

            }

            throw new NotImplementedException();
        }

        public int getCharacterMemoryPointer() {
            var bit1to3 = (C64.Memory.Read(0xD018) >> 1) & 0b00000111;

            switch (bit1to3) {
                case 0b000:
                    return 0x0000;
                case 0b001:
                    return 0x0800;
                case 0b010:
                    return 0x1000;
                case 0b011:
                    return 0x1800;
                case 0b100:
                    return 0x2000;
                case 0b101:
                    return 0x2800;
                case 0b110:
                    return 0x3000;
                case 0b111:
                    return 0x38000;
            }

            throw new NotImplementedException();
        }

        public byte vicRead(ushort address) {

            var vicBankOffset = 0;

            switch (C64.Memory[0xDD00] & 0b00000011) {
                case 0b00000011:
                    vicBankOffset = 0;

                    if (address >= 0x1000 && address <= 0x1FFF) {
                        return C64.Memory._romCharacter[address - 0x1000];
                    }

                    break;

                case 0b00000010:
                    vicBankOffset = 0x4000;
                    break;

                case 0b00000001:
                    vicBankOffset = 0x8000;

                    if (address >= 0x1000 && address <= 0x1FFF) {
                        return C64.Memory._romCharacter[address - 0x1000];
                    }

                    break;

                case 0b00000000:
                    vicBankOffset = 0xC000;
                    break;
            }


            return C64.Memory.Read(vicBankOffset + address);
        }

    }
}
