namespace Commodore64.Sid.Enums
{
    /// <summary>
    /// SID REGISTER ADDRESSES 0xD400-0xD7FF
    /// </summary>
    public enum SidRegister : int
    {
        VOICE1_FREQ_LOW = 0x00,
        VOICE1_FREQ_HIGH = 0x01,
        VOICE1_PULSE_WIDTH_LOW = 0x02,
        VOICE1_PULSE_WIDTH_HIGH = 0x03,
        VOICE1_CONTROL_REGISTER = 0x04,
        VOICE1_ATTACK_DECAY = 0x05,
        VOICE1_SUSTAIN_RELEASE = 0x06,

        VOICE2_FREQ_LOW = 0x07,
        VOICE2_FREQ_HIGH = 0x08,
        VOICE2_PULSE_WIDTH_LOW = 0x09,
        VOICE2_PULSE_WIDTH_HIGH = 0x0A,
        VOICE2_CONTROL_REGISTER = 0x0B,
        VOICE2_ATTACK_DECAY = 0x0C,
        VOICE2_SUSTAIN_RELEASE = 0x0D,

        VOICE3_FREQ_LOW = 0x0E,
        VOICE3_FREQ_HIGH = 0x0F,
        VOICE3_PULSE_WIDTH_LOW = 0x10,
        VOICE3_PULSE_WIDTH_HIGH = 0x11,
        VOICE3_CONTROL_REGISTER = 0x12,
        VOICE3_ATTACK_DECAY = 0x13,
        VOICE3_SUSTAIN_RELEASE = 0x14,

        FILTER_CUT_OFF_FREQ_LOW = 0x15,  // Bits 0-2
        FILTER_CUT_OFF_FREQ_HIGH = 0x16, // Bits 3-10
        FILTER_CONTROL = 0x17,
        VOLUME_FILTER_MODES = 0x18,

        PADDLE_X_VALUE = 0x19,
        PADDLE_Y_VALUE = 0x1A,

        VOICE3_WAVEFORM_OUTPUT = 0x1B,
        VOICE3_ADSR_OUTPUT = 0x1C,

        UNUSED_1 = 0x1D,
        UNUSED_2 = 0x1E,
        UNUSED_3 = 0x1F,
    }
}
