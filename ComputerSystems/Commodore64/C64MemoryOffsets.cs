namespace Commodore64
{
    public static class C64MemoryOffsets
    {
        /// <summary>
        /// Default area of screen memory (1000 bytes)
        /// 0x0400 - 0x07E7
        /// </summary>
        public const ushort SCREEN_BUFFER = 0x0400;

        /// <summary>
        /// Color RAM (1000 bytes, only bits #0-#3).
        /// 0xD800 - 0xDBE7
        /// </summary>
        public const ushort SCREEN_COLOR_RAM = 0xD800;

        /// <summary>
        /// Keyboard buffer (10 bytes, 10 entries)
        /// 0x0277 - 0x0280
        /// </summary>
        public const ushort KEYBOARD_BUFFER = 0x0277;

        public const ushort DEFAULT_BASIC_AREA_START = 0x0801;
        public const ushort DEFAULT_BASIC_AREA_END = 0x9FFF;
    }
}
