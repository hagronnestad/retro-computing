namespace Commodore64.Sid.Enums
{
    public enum SidControlRegisterBitFlags
    {
        // 0 = Voice off, Release cycle; 1 = Voice on, Attack-Decay-Sustain cycle.
        VOICE_GATE = 0b00000001,

        // 1 = Synchronization enabled.
        SYNCHRONIZATION = 0b00000010,

        // 1 = Ring modulation enabled.
        RING_MODULATION = 0b00000100,

        // 1 = Disable voice, reset noise generator.
        VOICE_DISABLE = 0b00001000,

        // 1 = Triangle waveform enabled.
        WAVEFORM_TRIANGLE = 0b00010000,

        // 1 = Saw waveform enabled.
        WAVEFORM_SAW = 0b00100000,

        // 1 = Rectangle waveform enabled.
        WAVEFORM_SQUARE = 0b01000000,

        // 1 = Noise enabled.
        WAVEFORM_NOISE = 0b10000000,
    }
}
