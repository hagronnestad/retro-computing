using System;

namespace MicroProcessor {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AddressingModeAttribute : Attribute {
        public AddressingMode AddressingMode { get; set; }
    }

}
