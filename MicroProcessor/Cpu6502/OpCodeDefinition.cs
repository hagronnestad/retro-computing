using MicroProcessor.Cpu6502.Attributes;
using MicroProcessor.Cpu6502.Enums;
using System;

namespace MicroProcessor.Cpu6502 {

    public class OpCodeDefinition {
        public byte Code { get; set; }
        public string Name { get; set; }
        public ushort Length { get; set; }
        public ushort Cycles { get; set; }
        public bool AddCycleIfBoundaryCrossed { get; set; }

        public AddressingMode AddressingMode { get; set; }

        public string Description { get; set; }

        public Action GetAddress { get; set; }
        public Action Run { get; set; }

        public OpCodeDefinition() {

        }

        public static OpCodeDefinition FromOpCodeAttribute(Action action, Action getAddress, OpCodeAttribute a) {
            return new OpCodeDefinition {
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
