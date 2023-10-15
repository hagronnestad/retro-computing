namespace Commodore64.Sid.Enums
{
    public enum FilterControlFlags
    {
        // Voice #1 filtered.
        VOICE1_FILTERED = 0b00000001,

        // Voice #2 filtered.
        VOICE2_FILTERED = 0b00000010,

        // Voice #3 filtered.
        VOICE3_FILTERED = 0b00000100,

        // External voice filtered.
        VOICE_EXT_FILTERED = 0b00001000,
    }
}
