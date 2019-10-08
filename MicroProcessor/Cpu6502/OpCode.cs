using MicroProcessor.Cpu6502.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroProcessor.Cpu6502 {

    public class OpCode : OpCodeDefinitionAttribute {

        public int OpCodeAddress { get; set; }
        public List<byte> Operands { get; set; } = new List<byte>();

        public Action GetAddress { get; set; }
        public Action Run { get; set; }


        public OpCode() : base() {

        }


        public override string ToString() {
            return ToString();
        }

        public string ToString(bool showComments = false) {
            var operands = $"{string.Join(" ", Operands.Select(x => $"{x:X2}"))}";
            var s = $"{OpCodeAddress:X4}\t{Code:X2} {operands.PadRight(5, ' ')}\t{Name}";

            switch (AddressingMode) {
                case Enums.AddressingMode.Accumulator:
                    s = $"{s} A";
                    break;

                case Enums.AddressingMode.Implied:
                    s = $"{s}";
                    break;

                case Enums.AddressingMode.Absolute:
                    s = $"{s} ${Operands[1]:X2}{Operands[0]:X2}";
                    break;

                case Enums.AddressingMode.AbsoluteX:
                    s = $"{s} ${Operands[1]:X2}{Operands[0]:X2}, X";
                    break;

                case Enums.AddressingMode.AbsoluteY:
                    s = $"{s} ${Operands[1]:X2}{Operands[0]:X2}, Y";
                    break;

                case Enums.AddressingMode.Immediate:
                    s = $"{s} #${Operands[0]:X2}";
                    break;

                case Enums.AddressingMode.Indirect:
                    s = $"{s} (${Operands[1]:X2}{Operands[0]:X2})";
                    break;

                case Enums.AddressingMode.XIndirect:
                    s = $"{s} (${Operands[0]:X2}, X)";
                    break;

                case Enums.AddressingMode.IndirectY:
                    s = $"{s} (${Operands[0]:X2}), Y";
                    break;

                case Enums.AddressingMode.Relative:
                    s = $"{s} ${(OpCodeAddress + Length + ((sbyte) Operands[0])):X2}";
                    break;

                case Enums.AddressingMode.Zeropage:
                    s = $"{s} ${Operands[0]:X2}";
                    break;

                case Enums.AddressingMode.ZeropageX:
                    s = $"{s} ${Operands[0]:X2}, X";
                    break;

                case Enums.AddressingMode.ZeropageY:
                    s = $"{s} ${Operands[0]:X2}, Y";
                    break;

                default:
                    break;
            }

            if (showComments) s = $"{s.PadRight(31, ' ')}\t// {Description}";

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
