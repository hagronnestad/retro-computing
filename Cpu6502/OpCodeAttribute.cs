using System;

namespace Cpu6502 {
    public class OpCodeAttribute : Attribute {

        public string Name { get; set; }
        public byte Code { get; set; }
        public ushort Length { get; set; }
        public ushort Cycles { get; set; }
        public bool AddCycleIfBoundaryCrossed { get; set; }

        public string Description { get; set; }
    }
}
