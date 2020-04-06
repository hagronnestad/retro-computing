using System;
using System.Drawing;
using Commodore64.Vic.Enums;
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

        public Color[,] ScreenBufferPixels { get; }

        private readonly TvSystem _tvSystem;
        private readonly int _fullHeight;
        public int _rasterLineToGenerateInterruptAt = 0;

        private bool _interruptLatchRasterLine = false;
        private bool _interruptLatchSpriteBackgroundCollision = false;
        private bool _interruptLatchSpriteSpriteCollision = false;
        private bool _interruptLatchLightPenSignal = false;
        private bool _interruptLatchAny =>
            _interruptLatchRasterLine ||
            _interruptLatchSpriteBackgroundCollision ||
            _interruptLatchSpriteSpriteCollision ||
            _interruptLatchLightPenSignal;


        public byte this[Register index] {
            get {
                switch (index) {

                    case Register.REGISTER_0x11_SCREEN_CONTROL_1:
                        // Bit #7 og 0x11 is set if current raster line > 255
                        _registers[(int)index] = _registers[(int)index].SetBit(BitFlag.BIT_7, CurrentLine > 255);
                        return _registers[(int)index];

                    // Current raster line (bits #0-#7).
                    // There's an additional bit used (bit #7) in 0x11 for values > 255
                    case Register.REGISTER_0x12_RASTER_COUNTER:
                        return CurrentLine > 255 ? (byte)(CurrentLine - 255) : (byte)CurrentLine;

                    // Interrupt status register.
                    case Register.REGISTER_0x19_INTERRUPT_REGISTER:
                        _registers[(int)index] = _registers[(int)index].SetBit(BitFlag.BIT_0, _interruptLatchRasterLine);
                        _registers[(int)index] = _registers[(int)index].SetBit(BitFlag.BIT_1, _interruptLatchSpriteBackgroundCollision);
                        _registers[(int)index] = _registers[(int)index].SetBit(BitFlag.BIT_2, _interruptLatchSpriteSpriteCollision);
                        _registers[(int)index] = _registers[(int)index].SetBit(BitFlag.BIT_3, _interruptLatchLightPenSignal);
                        _registers[(int)index] = _registers[(int)index].SetBit(BitFlag.BIT_7, _interruptLatchAny);
                        return _registers[(int)index];

                    default:
                        return _registers[(int)index];
                }
            }
            set {
                switch (index) {

                    // Raster line to generate interrupt at (bits #0-#7).
                    // There's an additional bit used (bit #7) in 0x11 for values > 255
                    case Register.REGISTER_0x12_RASTER_COUNTER:
                        _registers[(int)index] = value;
                        UpdateRasterLineToGenerateInterruptAt();
                        break;

                    // Raster line to generate interrupt at (bit #7).
                    // Bit #7 in this register is used as the 8th bit for the raster counter
                    // that's why we need to update the internal raster line variable if this
                    // register is modified
                    case Register.REGISTER_0x11_SCREEN_CONTROL_1:
                        _registers[(int)index] = value;
                        UpdateRasterLineToGenerateInterruptAt();
                        break;

                    // Interrupt status register.
                    // Setting bit 0-3 to 1 acknowledges the respective interrupt latch
                    case Register.REGISTER_0x19_INTERRUPT_REGISTER:
                        if (value.IsBitSet(BitFlag.BIT_0)) _interruptLatchRasterLine = false;
                        if (value.IsBitSet(BitFlag.BIT_1)) _interruptLatchSpriteBackgroundCollision = false;
                        if (value.IsBitSet(BitFlag.BIT_2)) _interruptLatchSpriteSpriteCollision = false;
                        if (value.IsBitSet(BitFlag.BIT_3)) _interruptLatchLightPenSignal = false;
                        _registers[(int)index] = value;
                        break;

                    default:
                        _registers[(int)index] = value;
                        break;
                }
            }
        }

        private void UpdateRasterLineToGenerateInterruptAt() {
            _rasterLineToGenerateInterruptAt = this[Register.REGISTER_0x12_RASTER_COUNTER];
            if (this[Register.REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_7)) {
                _rasterLineToGenerateInterruptAt += 255;
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

        public bool InVerticalBlank => CurrentLine >= 300 || CurrentLine <= 15;

        //public bool InBorder =>
        //    (CurrentLine >= USABLE_HEIGHT + (USABLE_HEIGHT_BORDER / 2) && CurrentLine <= (USABLE_HEIGHT_BORDER / 2)) ||
        //    (CurrentLineCycle >= USABLE_WIDTH + (USABLE_HEIGHT_BORDER / 2) && CurrentLineCycle <= (USABLE_HEIGHT_BORDER / 2));

        public ulong TotalCycles = 0;

        // Screen control register
        public bool ScreenControlRegisterScreenHeight => this[Register.REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_3); // False = 24 rows, True = 25 rows
        public bool ScreenControlRegisterScreenOffOn => this[Register.REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_4);
        public bool ScreenControlRegisterTextModeBitmapMode => this[Register.REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_5); // False = Text mode, True = Bitmap mode
        public bool ScreenControlRegisterExtendedBackgroundModeOn => this[Register.REGISTER_0x11_SCREEN_CONTROL_1].IsBitSet(BitFlag.BIT_6); // True = Extended background mode on

        // Interrupt control register
        public bool InterruptControlRegisterRasterInterruptEnabled => this[Register.REGISTER_0x1A_INTERRUPT_ENABLED].IsBitSet(BitFlag.BIT_0);
        public bool InterruptControlRegisterSpriteBackgroundCollisionInterruptEnabled => this[Register.REGISTER_0x1A_INTERRUPT_ENABLED].IsBitSet(BitFlag.BIT_1);
        public bool InterruptControlRegisterSpriteSpriteCollisionInterruptEnabled => this[Register.REGISTER_0x1A_INTERRUPT_ENABLED].IsBitSet(BitFlag.BIT_2);
        public bool InterruptControlRegisterLightPenInterruptEnabled => this[Register.REGISTER_0x1A_INTERRUPT_ENABLED].IsBitSet(BitFlag.BIT_3);



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
            // Every cycle draws 8 pixels to the screen

            // Generate raster interrupt if the current line equals interrupt line
            if (CurrentLineCycle == 0 && InterruptControlRegisterRasterInterruptEnabled && CurrentLine == _rasterLineToGenerateInterruptAt) {
                _interruptLatchRasterLine = true;
                OnGenerateRasterLineInterrupt?.Invoke(this, null);
                //Debug.WriteLine($"OnGenerateRasterLineInterrupt: {_rasterLineToGenerateInterruptAt}");
            }


            var p = GetScanlinePoint();

            if (IsInDisplay(p) && ScreenControlRegisterScreenOffOn) {

                switch (GetCurrentGraphicsMode()) {
                    case GraphicsMode.StandardCharacterMode:
                        RenderStandardCharacterMode();
                        break;

                    case GraphicsMode.MultiColorCharacterMode:
                        RenderMultiColorCharacterMode();
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
                }
            }

            TotalCycles++;
        }


        /// <summary>
        /// Render 40x25 Standard Character Mode
        /// </summary>
        private void RenderStandardCharacterMode() {
            var p = GetScanlinePoint();

            var line = (CurrentLine - DisplayFrame.Y) / 8;
            var column = (p.X - DisplayFrame.X) / 8;
            var charOffsetInMemory = line * 40 + column;

            var charRow = (CurrentLine - DisplayFrame.Y) % 8;

            var screenPointer = getScreenMemoryPointer();
            var characterPointer = getCharacterMemoryPointer();

            var charDataOffsetInMemory = vicRead((ushort)(screenPointer + charOffsetInMemory));
            var charRowData = vicRead((ushort)(characterPointer + charDataOffsetInMemory * 8 + charRow));

            var bgColor = Colors.FromByte((byte)(this[Register.REGISTER_0x21_BACKGROUND_COLOR_0] & 0b00001111));
            var fgColor = Colors.FromByte((byte)(C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + charOffsetInMemory] & 0b00001111));

            for (int col = 0; col <= 7; col++) {
                var pixel = charRowData.IsBitSet(7 - (BitIndex)col) ? fgColor : bgColor;
                ScreenBufferPixels[DisplayFrame.Y + line * 8 + charRow, DisplayFrame.X + column * 8 + col] = pixel;
            }
        }


        /// <summary>
        /// Render MultiColorCharacterMode
        /// </summary>
        private void RenderMultiColorCharacterMode() {
            var p = GetScanlinePoint();

            var line = (CurrentLine - DisplayFrame.Y) / 8;
            var column = (p.X - DisplayFrame.X) / 8;
            var charOffsetInMemory = line * 40 + column;

            var charRow = (CurrentLine - DisplayFrame.Y) % 8;

            var screenPointer = getScreenMemoryPointer();
            var characterPointer = getCharacterMemoryPointer();

            var charDataOffsetInMemory = vicRead((ushort)(screenPointer + charOffsetInMemory));
            var charRowData = vicRead((ushort)(characterPointer + charDataOffsetInMemory * 8 + charRow));

            var bgColor1 = Colors.FromByte((byte)(this[Register.REGISTER_0x21_BACKGROUND_COLOR_0] & 0b00001111));
            var bgColor2 = Colors.FromByte((byte)(this[Register.REGISTER_0x22_BACKGROUND_COLOR_1] & 0b00001111));
            var bgColor3 = Colors.FromByte((byte)(this[Register.REGISTER_0x23_BACKGROUND_COLOR_2] & 0b00001111));
            var fgColor = (byte)(C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + charOffsetInMemory] & 0b00001111);
            
            var pixelColor = Color.Red;

            for (int col = 0; col <= 7; col++) {

                if (fgColor.IsBitSet(BitFlag.BIT_3)) {

                    var bitPair = (charRowData >> (6 - col)) & 0b11;
                    switch (bitPair) {
                        case 0b00:
                            pixelColor = bgColor1;
                            break;
                        case 0b01:
                            pixelColor = bgColor2;
                            break;
                        case 0b10:
                            pixelColor = bgColor3;
                            break;
                        case 0b11:
                            var b = (fgColor >> 0) & 0b111;
                            pixelColor = Colors.FromByte((byte)b);
                            break;
                    }

                    ScreenBufferPixels[DisplayFrame.Y + line * 8 + charRow, DisplayFrame.X + column * 8 + col] = pixelColor;
                    ScreenBufferPixels[DisplayFrame.Y + line * 8 + charRow, DisplayFrame.X + column * 8 + col + 1] = pixelColor;

                    col++;

                } else {
                    var pixel = charRowData.IsBitSet(7 - (BitIndex)col) ? Colors.FromByte(fgColor) : bgColor1;
                    ScreenBufferPixels[DisplayFrame.Y + line * 8 + charRow, DisplayFrame.X + column * 8 + col] = pixel;
                }
            }
        }


        /// <summary>
        /// Render Border
        /// </summary>
        private void RenderBorder() {
            var bgColor = Colors.FromByte((byte)(this[Register.REGISTER_0x20_BORDER_COLOR] & 0b00001111));

            for (int i = 0; i < 8; i++) {
                ScreenBufferPixels[CurrentLine, CurrentLineCycle * 8 + i] = bgColor;
            }
        }

        public GraphicsMode GetCurrentGraphicsMode() {
            var ecm_bmm_r0x11_b65 = this[Register.REGISTER_0x11_SCREEN_CONTROL_1] >> 5 & 0b00000011;
            var mcm_r0x16_b4 = this[Register.REGISTER_0x16_SCREEN_CONTROL_2] >> 4 & 0b00000001;

            var graphicsMode = mcm_r0x16_b4 | ecm_bmm_r0x11_b65 << 1;
            return (GraphicsMode)graphicsMode;
        }

        public Point GetScanlinePoint() {
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

        public int getScreenMemoryPointer() {
            var bit4to7 = this[Register.REGISTER_0x18_MEMORY_POINTERS] >> 4 & 0b00001111;

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
            var bit1to3 = this[Register.REGISTER_0x18_MEMORY_POINTERS] >> 1 & 0b00000111;

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

            switch (C64.Memory.Read(0xDD00) & 0b00000011) {
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
