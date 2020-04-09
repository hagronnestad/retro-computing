namespace Commodore64.Cia.Enums {

    /// <summary>
    /// CIA2 REGISTER ADDRESSES 0xDD00-0xDD0F
    /// </summary>
    public enum Register : byte {
        R_0x00_PORT_A = 0x00,
        R_0x01_PORT_B = 0x01,
        R_0x02_PORT_A_DATA_DIRECTION = 0x02,
        R_0x03_PORT_B_DATA_DIRECTION = 0x03,

        R_0x04_TIMER_A_VALUE_1 = 0x04,
        R_0x05_TIMER_A_VALUE_2 = 0x05,
        R_0x06_TIMER_B_VALUE_1 = 0x06,
        R_0x07_TIMER_B_VALUE_2 = 0x07,

        R_0x08_TOD_TENTH_SECONDS = 0x08,
        R_0x09_TOD_SECONDS = 0x09,
        R_0x0A_TOD_MINUTES = 0x0A,
        R_0x0B_TOD_HOURS = 0x0B,

        R_0x0C_SERIAL_SHIFT_REGISTER = 0x0C,

        R_0x0D_INTERRUPT_CONTROL_AND_STATUS = 0x0D,

        R_0x0E_TIMER_A_CONTROL = 0x0E,
        R_0x0F_TIMER_B_CONTROL = 0x0F,
    }

}
