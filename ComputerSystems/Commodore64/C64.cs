using MicroProcessor.Cpu6502;

namespace Commodore64 {
    public class C64 {



        public C64Memory Memory { get; private set; }
        public Cpu Cpu { get; private set; }

        public C64() {
            Memory = new C64Memory();
            Cpu = new Cpu(Memory);

            Cpu.Reset();
        }

    }
}
