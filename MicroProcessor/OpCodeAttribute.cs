using System;

namespace MicroProcessor {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OpCodeAttribute : Attribute {
        public byte Code { get; set; }
        public string Name { get; set; }
        public ushort Length { get; set; }
        public ushort Cycles { get; set; }
        public bool AddCycleIfBoundaryCrossed { get; set; }

        public AddressingMode AddressingMode { get; set; }

        public string Description { get; set; }

        public OpCodeAttribute() {

        }
    }

}
