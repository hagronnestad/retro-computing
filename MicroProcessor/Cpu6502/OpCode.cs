using MicroProcessor.Cpu6502.Attributes;
using System;
using System.Collections.Generic;

namespace MicroProcessor.Cpu6502 {

    public class OpCode : OpCodeDefinitionAttribute {

        public int OpCodeAddress { get; set; }
        public List<byte> Operands { get; set; } = new List<byte>();

        public Action GetAddress { get; set; }
        public Action Run { get; set; }

        public OpCode() : base() {

        }

        public override string ToString() {
            var s = $"{OpCodeAddress:X4} {Name}";

            switch (AddressingMode) {
                case Enums.AddressingMode.Accumulator:
                    return $"{s} A";

                case Enums.AddressingMode.Implied:
                    return $"{s}";

                case Enums.AddressingMode.Absolute:
                    return $"{s} 0x{Operands[1]:X2}{Operands[0]:X2}";

                case Enums.AddressingMode.AbsoluteX:
                    return $"{s} 0x{Operands[1]:X2}{Operands[0]:X2}, X";

                case Enums.AddressingMode.AbsoluteY:
                    return $"{s} 0x{Operands[1]:X2}{Operands[0]:X2}, Y";

                case Enums.AddressingMode.Immediate:
                    break;

                case Enums.AddressingMode.Indirect:
                    break;
                case Enums.AddressingMode.XIndirect:
                    break;
                case Enums.AddressingMode.IndirectY:
                    break;
                case Enums.AddressingMode.Relative:
                    break;
                case Enums.AddressingMode.Zeropage:
                    break;
                case Enums.AddressingMode.ZeropageX:
                    break;
                case Enums.AddressingMode.ZeropageY:
                    break;
                default:
                    break;
            }

            return s;
        }

        public static OpCode FromOpCodeDefinitionAttribute(Action action, Action getAddress, OpCodeDefinitionAttribute a) {
            return new OpCode {
                GetAddress = getAddress,
                Run = action,
                AddCycleIfBoundaryCrossed = a.AddCycleIfBoundaryCrossed,
                AddressingMode = a.AddressingMode,
                Code = a.Code,
                Cycles = a.Cycles,
                Description = a.Description,
                Length = a.Length,
                Name = a.Name
            };
        }
    }

}
