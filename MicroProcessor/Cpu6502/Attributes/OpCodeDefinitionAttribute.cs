using MicroProcessor.Cpu6502.Enums;
using System;

namespace MicroProcessor.Cpu6502.Attributes {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OpCodeDefinitionAttribute : Attribute {
        public byte Code { get; set; }
        public string Name { get; set; }
        public ushort Length { get; set; }
        public ushort Cycles { get; set; }
        public bool AddCycleIfBoundaryCrossed { get; set; }

        public AddressingMode AddressingMode { get; set; }

        public string Description { get; set; }

        public bool IsIllegal { get; set; } = false;

        public OpCodeDefinitionAttribute() {

        }
    }

}
