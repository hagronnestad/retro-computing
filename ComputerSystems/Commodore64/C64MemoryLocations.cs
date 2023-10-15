namespace Commodore64
{
    public static class C64MemoryLocations
    {

        /// <summary>
        /// Current output device number
        /// Default: 0x03, screen
        /// </summary>
        public const ushort CURRENT_OUTPUT_DEVICE = 0x009A;

        /// <summary>
        /// Background color (only bits #0-#3)
        /// </summary>
        public const ushort SCREEN_BACKGROUND_COLOR = 0xD021;

        /// <summary>
        /// Length of keyboard buffer
        /// Values:
        /// 0x00: Buffer is empty
        /// 0x01 - 0x0A: Buffer length
        /// </summary>
        public const ushort KEYBOARD_BUFFER_LENGTH = 0x00C6;

        /// <summary>
        /// Stop key indicator
        /// Values:
        /// 0x7F: Stop key is pressed
        /// 0xFF: Stop key is not pressed
        /// </summary>
        public const ushort STOP_KEY_INDICATOR = 0x0091;

        /// <summary>
        /// Read: Current raster line (bits #0-#7)
        /// Write: Raster line to generate interrupt at (bits #0-#7)
        /// </summary>
        public const ushort CURRENT_RASTER_LINE = 0xD012;

        public const ushort POINTER_TO_BEGINNING_OF_BASIC_AREA_LO = 0x002B;
        public const ushort POINTER_TO_BEGINNING_OF_BASIC_AREA_HI = 0x002C;
    }
}
