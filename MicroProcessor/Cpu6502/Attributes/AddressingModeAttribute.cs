using MicroProcessor.Cpu6502.Enums;
using System;

namespace MicroProcessor.Cpu6502.Attributes
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AddressingModeAttribute : Attribute
    {
        public AddressingMode AddressingMode { get; set; }
    }

}
