using MicroProcessor.Cpu6502.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MicroProcessor.Cpu6502 {

    public class Disassembler {

        public List<OpCodeAttribute> OpCodes { get; private set; }
        public Dictionary<byte, OpCodeAttribute> OpCodeCache { get; set; }

        public Disassembler() {

            OpCodes = typeof(Cpu)
                .GetMethods()
                .SelectMany(m => m.GetCustomAttributes(typeof(OpCodeAttribute), true)
                .Select(a => a as OpCodeAttribute))
                .ToList();

            OpCodeCache = OpCodes.ToDictionary(x => x.Code, x => x);

        }

        public Dictionary<int, OpCodeAttribute> Disassemble(byte[] machineCode) {
            var disassembly = new Dictionary<int, OpCodeAttribute>();

            for (int i = 0; i < machineCode.Length; i++) {

                if (!OpCodeCache.ContainsKey(machineCode[i])) continue;

                var opCode = OpCodeCache[machineCode[i]];
                disassembly.Add(i, opCode);

                i += opCode.Length - 1;
            }

            return disassembly;
        }
    }

}
