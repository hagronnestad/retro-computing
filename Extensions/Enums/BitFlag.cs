using System;

namespace Extensions.Enums {

    [Flags]
    public enum BitFlag : byte {
        BIT_0 = 0b00000001,
        BIT_1 = 0b00000010,
        BIT_2 = 0b00000100,
        BIT_3 = 0b00001000,
        BIT_4 = 0b00010000,
        BIT_5 = 0b00100000,
        BIT_6 = 0b01000000,
        BIT_7 = 0b10000000,
    }

}
