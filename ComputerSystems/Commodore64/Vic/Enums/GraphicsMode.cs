namespace Commodore64.Vic.Enums {

    /// <summary>
    /// GraphicsMode as defined by MCM (bit 4 in $D016), BMM (bit 5 in $D011) and ECM (bit 6 in $D011)
    /// Source: https://www.c64-wiki.com/wiki/Graphics_Modes#Implementation
    /// </summary>
    public enum GraphicsMode {
        StandardCharacterMode = 0b000,
        MultiColorCharacterMode = 0b001,
        StandardBitmapMode = 0b010,
        MulticolorBitmapMode = 0b011,
        ExtendedBackgroundColorMode = 0b100,
        UNOFFICIAL_ExtendedBackgroundColorMulticolorCharacterMode = 0b101,
        UNOFFICIAL_ExtendedBackgroundColorStandardBitmapMode = 0b110,
        UNOFFICIAL_ExtendedBackgroundColorMulticolorBitmapMode = 0b111
    }
}
