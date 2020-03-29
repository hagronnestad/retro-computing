using System;
using System.Drawing;
using Commodore64.Enums;
using Extensions.Byte;
using Extensions.Enums;

namespace Commodore64.Vic {
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

        // VIC-II REGISTER ADDRESSES 0xD0xx
        //private const byte REGISTER_0x_ = 0x;

        private const byte REGISTER_0x00_X_SPRITE_0 = 0x00;
        private const byte REGISTER_0x01_Y_SPRITE_0 = 0x01;
        private const byte REGISTER_0x02_X_SPRITE_1 = 0x02;
        private const byte REGISTER_0x03_Y_SPRITE_1 = 0x03;
        private const byte REGISTER_0x04_X_SPRITE_2 = 0x04;
        private const byte REGISTER_0x05_Y_SPRITE_2 = 0x05;
        private const byte REGISTER_0x06_X_SPRITE_3 = 0x06;
        private const byte REGISTER_0x07_Y_SPRITE_3 = 0x07;
        private const byte REGISTER_0x08_X_SPRITE_4 = 0x08;
        private const byte REGISTER_0x09_Y_SPRITE_4 = 0x09;
        private const byte REGISTER_0x0A_X_SPRITE_5 = 0x0A;
        private const byte REGISTER_0x0B_Y_SPRITE_5 = 0x0B;
        private const byte REGISTER_0x0C_X_SPRITE_6 = 0x0C;
        private const byte REGISTER_0x0D_Y_SPRITE_6 = 0x0D;
        private const byte REGISTER_0x0E_X_SPRITE_7 = 0x0E;
        private const byte REGISTER_0x0F_Y_SPRITE_7 = 0x0F;
        private const byte REGISTER_0x10_ = 0x10;
        private const byte REGISTER_0x11_SCREEN_CONTROL_1 = 0x11;
        private const byte REGISTER_0x12_RASTER_COUNTER = 0x12;
        private const byte REGISTER_0x16_SCREEN_CONTROL_2 = 0x16;

        private const byte REGISTER_0x19_INTERRUPT_STATUS = 0x19;
        private const byte REGISTER_0x1A_INTERRUPT_CONTROL = 0x1A;

        private const byte REGISTER_0x21_BACKGROUND_COLOR_0 = 0x21;
        private const byte REGISTER_0x22_BACKGROUND_COLOR_1 = 0x22;
        private const byte REGISTER_0x23_BACKGROUND_COLOR_2 = 0x23;
        private const byte REGISTER_0x24_BACKGROUND_COLOR_3 = 0x24;

        public Color[,] ScreenBufferPixels { get; }

        private readonly TvSystem _tvSystem;
        private readonly int _fullHeight;
        private int _rasterLineToGenerateInterruptAt = 0;

        public byte this[int index] {
            get {
                switch (index) {

                    case REGISTER_0x11_SCREEN_CONTROL_1:
                        // Bit #7 og 0x11 is set if current raster line > 255
                        _registers[index].SetBit(BitFlag.BIT_7, CurrentLine > 255);

                        return _registers[index];

                    // Current raster line (bits #0-#7).
                    // There's an additional bit used (bit #7) in 0x11 for values > 255
                    case REGISTER_0x12_RASTER_COUNTER:
                        return CurrentLine > 255 ? (byte)(CurrentLine - 255) : (byte)CurrentLine;

                    default:
                        return _registers[index];
                }
            }
            set {
                switch (index) {

                    case REGISTER_0x11_SCREEN_CONTROL_1:
                        _registers[index] = value;
                        UpdateRasterLineToGenerateInterruptAt();
                        break;

                    // Raster line to generate interrupt at (bits #0-#7).
                    // There's an additional bit used (bit #7) in 0x11 for values > 255
                    case REGISTER_0x12_RASTER_COUNTER:
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
            _rasterLineToGenerateInterruptAt = _registers[REGISTER_0x12_RASTER_COUNTER];
            if (_registers[REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_7)) _rasterLineToGenerateInterruptAt += 255;
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

        public ulong TotalCycles = 0;

        // Screen control register
        public bool ScreenControlRegisterScreenHeight => this[REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_3); // False = 24 rows, True = 25 rows
        public bool ScreenControlRegisterScreenOffOn => this[REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_4);
        public bool ScreenControlRegisterTextModeBitmapMode => this[REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_5); // False = Text mode, True = Bitmap mode
        public bool ScreenControlRegisterExtendedBackgroundModeOn => this[REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_6); // True = Extended background mode on

        // Interrupt control register
        public bool InterruptControlRegisterRasterInterruptEnabled => this[REGISTER_0x1A_INTERRUPT_CONTROL].IsBitSet(BitFlag.BIT_0);
        public bool InterruptControlRegisterSpriteBackgroundCollisionInterruptEnabled => this[REGISTER_0x1A_INTERRUPT_CONTROL].IsBitSet(BitFlag.BIT_1);
        public bool InterruptControlRegisterSpriteSpriteCollisionInterruptEnabled => this[REGISTER_0x1A_INTERRUPT_CONTROL].IsBitSet(BitFlag.BIT_2);
        public bool InterruptControlRegisterLightPenInterruptEnabled => this[REGISTER_0x1A_INTERRUPT_CONTROL].IsBitSet(BitFlag.BIT_3);



        public Rectangle FullFrame;
        public Rectangle BorderFrame;
        public Rectangle DisplayFrame;


        public VicIi(TvSystem tvSystem) {
            _tvSystem = tvSystem;
            _fullHeight = tvSystem == TvSystem.PAL ? FULL_HEIGHT_PAL : FULL_HEIGHT_NTSC;

            var fullHeight = tvSystem == TvSystem.PAL ? FULL_HEIGHT_PAL : FULL_HEIGHT_NTSC;

            FullFrame = new Rectangle(0, 0, FULL_WIDTH, fullHeight);
            BorderFrame = new Rectangle(48, 14, 403, USABLE_HEIGHT_BORDER);
            DisplayFrame = new Rectangle(88, 56, USABLE_WIDTH, USABLE_HEIGHT);

            ScreenBufferPixels = new Color[fullHeight, FULL_WIDTH];
        }

        public void Cycle() {

            // Generate raster interrupt if the current line equals interrupt line
            // TODO: Implement the Interrupt latch register ($D019) !!!!!!!
            if (InterruptControlRegisterRasterInterruptEnabled && CurrentLine == _rasterLineToGenerateInterruptAt) {
                OnGenerateRasterLineInterrupt?.Invoke(this, null);
            }



            // Every cycle draws 8 pixels to the screen

            //if (IsInDisplay()) {
            //    RenderCharacterMode();
            //}

            var p = GetScanlinePoint();

            if (IsInDisplay(p) && ScreenControlRegisterScreenOffOn) {

                switch (GetCurrentGraphicsMode()) {
                    case GraphicsMode.StandardCharacterMode:
                        RenderStandardCharacterMode();
                        break;

                    case GraphicsMode.MultiColorCharacterMode:
                        break;

                    case GraphicsMode.StandardBitmapMode:
                        break;

                    case GraphicsMode.MulticolorBitmapMode:
                        break;

                    case GraphicsMode.ExtendedBackgroundColorMode:
                        break;

                    case GraphicsMode.UNOFFICIAL_ExtendedBackgroundColorMulticolorCharacterMode:
                        break;
                    case GraphicsMode.UNOFFICIAL_ExtendedBackgroundColorStandardBitmapMode:
                        break;
                    case GraphicsMode.UNOFFICIAL_ExtendedBackgroundColorMulticolorBitmapMode:
                        break;
                    default:
                        break;
                }

            } else {

                if (IsInBorder(p)) {
                    RenderBorder();
                }

            }


            CurrentLineCycle++;

            // Every line takes 63 cycles
            if (CurrentLineCycle == 63) {
                CurrentLineCycle = 0;

                CurrentLine++;

                if (CurrentLine == _fullHeight) {
                    CurrentLine = 0;

                    OnLastScanLine?.Invoke(this, null);

                    // Frame based character mode rendering
                    //if (ScreenControlRegisterScreenOffOn) UpdateScreenBufferPixels();
                }
            }


            TotalCycles++;
        }

        private void RenderStandardCharacterMode() {
            // 40 x 25 characters

            var p = GetScanlinePoint();

            var bgColor = Colors.FromByte((byte)(this[REGISTER_0x21_BACKGROUND_COLOR_0] & 0b00001111));

            var charLine = (CurrentLine - DisplayFrame.Y) / 8;
            var charNumberInLine = (p.X - DisplayFrame.X) / 8;
            var charNumber = charLine * 40 + charNumberInLine;

            var petsciiCode = vicRead((ushort)(getScreenMemoryPointer() + charNumber));
            var fgColor = Colors.FromByte((byte)(C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + charNumber] & 0b00001111));

            var charRow = (CurrentLine - DisplayFrame.Y) % 8;
            var charRowData = vicRead((ushort)(getCharacterMemoryPointer() + petsciiCode * 8 + charRow));

            for (int col = 0; col <= 7; col++) {
                ScreenBufferPixels[DisplayFrame.Y + charLine * 8 + charRow, DisplayFrame.X + charNumberInLine * 8 + col] = charRowData.IsBitSet(7 - (BitIndex)col) ? fgColor : bgColor;
            }
        }

        private void RenderBorder() {

            var bgColor = Colors.FromByte((byte)(_registers[0x20] & 0b00001111));

            for (int i = 0; i < 8; i++) {
                ScreenBufferPixels[CurrentLine, CurrentLineCycle * 8 + i] = bgColor;
            }

        }

        public GraphicsMode GetCurrentGraphicsMode() {
            var ecm_bmm_r0x11_b65 = this[REGISTER_0x11_SCREEN_CONTROL_1] >> 5 & 0b00000011;
            var mcm_r0x16_b4 = this[REGISTER_0x16_SCREEN_CONTROL_2] >> 4 & 0b00000001;

            var graphicsMode = mcm_r0x16_b4 | ecm_bmm_r0x11_b65 << 1;
            return (GraphicsMode)graphicsMode;
        }

        private Point GetScanlinePoint() {
            var p = new Point {
                X = CurrentLineCycle * 8,
                Y = CurrentLine
            };

            return p;
        }

        private bool IsInBorder(Point p) {
            return BorderFrame.Contains(p);
        }

        private bool IsInDisplay(Point p) {
            return DisplayFrame.Contains(p);
        }


        public void UpdateScreenBufferPixels() {
            var bgColor = Colors.FromByte((byte)(C64.Vic._registers[0x21] & 0b00001111));

            for (var i = 0; i < 1000; i++) {
                var petsciiCode = vicRead((ushort)(getScreenMemoryPointer() + i));
                var fgColor = Colors.FromByte((byte)(C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + i] & 0b00001111));
                //var fgColor = Colors.FromByte((byte)(vicRead((ushort)(0x0800 + i)) & 0b00001111));

                var line = i / 40;
                var characterInLine = i % 40;

                for (int row = 0; row <= 7; row++) {
                    var charRow = vicRead((ushort)(getCharacterMemoryPointer() + petsciiCode * 8 + row));

                    for (int col = 0; col <= 7; col++) {
                        ScreenBufferPixels[DisplayFrame.Y + line * 8 + row, DisplayFrame.X + characterInLine * 8 + col] = charRow.IsBitSet(7 - (BitIndex)col) ? fgColor : bgColor;
                    }

                }

            }
        }

        public int getScreenMemoryPointer() {
            var bit4to7 = C64.Memory.Read(0xD018) >> 4 & 0b00001111;

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
            var bit1to3 = C64.Memory.Read(0xD018) >> 1 & 0b00000111;

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
                    return 0x3800;
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
