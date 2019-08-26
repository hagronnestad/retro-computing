using System;

namespace Cpu6502 {
    public class OpCodeDefinition {
        public byte Code { get; set; }
        public string Name { get; set; }
        public ushort Length { get; set; }
        public ushort Cycles { get; set; }
        public bool AddCycleIfBoundaryCrossed { get; set; }

        public AddressingMode AddressingMode { get; set; }

        public string Description { get; set; }

        public Action<object[]> Action { get; set; }

        public OpCodeDefinition() {

        }

        public static OpCodeDefinition FromOpCodeAttribute(Action<object[]> action, OpCodeAttribute a) {
            return new OpCodeDefinition {
                Action = action,
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
