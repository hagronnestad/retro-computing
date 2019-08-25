using System;

namespace Cpu6502 {
    [Flags]
    public enum ProcessorStatusFlags : byte {
        Carry        = 0b00000001,
        Zero         = 0b00000010,
        IrqDisable   = 0b00000100,
        DecimalMode  = 0b00001000,
        BreakCommand = 0b00010000,
        Reserved     = 0b00100000,
        Overflow     = 0b01000000,
        Negative     = 0b10000000,
    }
}
