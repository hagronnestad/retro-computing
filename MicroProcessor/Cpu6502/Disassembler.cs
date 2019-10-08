using MicroProcessor.Cpu6502.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MicroProcessor.Cpu6502 {

    public class Disassembler {

        public List<OpCodeDefinitionAttribute> OpCodes { get; private set; }
        public Dictionary<byte, OpCodeDefinitionAttribute> OpCodeCache { get; set; }

        public Disassembler() {

            OpCodes = typeof(Cpu)
                .GetMethods()
                .SelectMany(m => m.GetCustomAttributes(typeof(OpCodeDefinitionAttribute), true)
                .Select(a => a as OpCodeDefinitionAttribute))
                .ToList();

            OpCodeCache = OpCodes.ToDictionary(x => x.Code, x => x);

        }

        public Dictionary<int, OpCode> Disassemble(byte[] machineCode, int start = 0, int? length = null) {
            if (!length.HasValue) length = machineCode.Length;

            var disassembly = new Dictionary<int, OpCode>();

            for (int i = start; i < start + length; i++) {

                if (!OpCodeCache.ContainsKey(machineCode[i])) continue;

                var opCode = OpCode.FromOpCodeDefinitionAttribute(null, null, OpCodeCache[machineCode[i]]);

                opCode.OpCodeAddress = i;

                for (int j = 1; j < opCode.Length; j++) {
                    opCode.Operands.Add(machineCode[i + j]);
                }

                disassembly.Add(i, opCode);

                i += opCode.Length - 1;
            }

            return disassembly;
        }
    }

}
