using MicroProcessor.Cpu6502.Enums;

namespace MicroProcessor.Cpu6502 {
    public class StatusRegister {
        public byte Register { get; set; }

        public bool Carry {
            get => Get(ProcessorStatusFlags.Carry);
            set => SetValue(ProcessorStatusFlags.Carry, value);
        }

        public bool Zero {
            get => Get(ProcessorStatusFlags.Zero);
            set => SetValue(ProcessorStatusFlags.Zero, value);
        }

        public bool IrqDisable {
            get => Get(ProcessorStatusFlags.IrqDisable);
            set => SetValue(ProcessorStatusFlags.IrqDisable, value);
        }

        public bool DecimalMode {
            get => Get(ProcessorStatusFlags.DecimalMode);
            set => SetValue(ProcessorStatusFlags.DecimalMode, value);
        }

        public bool BreakCommand {
            get => Get(ProcessorStatusFlags.BreakCommand);
            set => SetValue(ProcessorStatusFlags.BreakCommand, value);
        }

        public bool Overflow {
            get => Get(ProcessorStatusFlags.Overflow);
            set => SetValue(ProcessorStatusFlags.Overflow, value);
        }

        public bool Negative {
            get => Get(ProcessorStatusFlags.Negative);
            set => SetValue(ProcessorStatusFlags.Negative, value);
        }

        public bool Reserved {
            get => Get(ProcessorStatusFlags.Reserved);
            set => SetValue(ProcessorStatusFlags.Reserved, value);
        }



        public bool Get(ProcessorStatusFlags flag) {
            return (Register & (byte)flag) == (byte)flag;
        }

        public void Set(ProcessorStatusFlags flag) {
            Register |= (byte)flag;
        }

        public void SetValue(ProcessorStatusFlags flag, bool value) {
            Register = value ? Register |= (byte)flag : Register &= (byte)~flag;
        }

        public void Clear(ProcessorStatusFlags flag) {
            Register &= (byte)~flag;
        }

        public void SetNegative(byte operand) {
            Negative = (operand & 0b10000000) == 0b10000000;
        }

        public void SetZero(byte operand) {
            Zero = operand == 0;
        }
    }
}
