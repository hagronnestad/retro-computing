using System;

namespace Cpu6502 {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AddressingModeAttribute : Attribute {
        public AddressingMode AddressingMode { get; set; }
    }

}
