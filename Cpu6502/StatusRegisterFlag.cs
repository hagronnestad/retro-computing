using System;

namespace Cpu6502 {
    [Flags]
    public enum ProcessorStatusFlags : byte {
        Carry = 1,
        Zero = 2,
        IrqDisable = 4,
        DecimalMode = 8,
        BreakCommand = 16,
        Reserved = 32,
        Overflow = 64,
        Negative = 128
    }
}
