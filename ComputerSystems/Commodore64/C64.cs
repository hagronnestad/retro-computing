using MicroProcessor.Cpu6502;

namespace Commodore64 {
    public class C64 {

        public Cpu Cpu { get; set; }

        public C64() {
            Cpu = new Cpu();
        }

    }
}
