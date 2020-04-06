using Commodore64.Cia.Enums;

namespace Commodore64.Cia {
    
    /// <summary>
    /// This should be a generic CIA chip class that could be shared between the C64s two CIA chips
    /// or at least be a base class for the common functionality and extended class with
    /// correctly named registers for convenience.
    /// TODO: ^ that and merge with Cia class in Mos6526Cia project
    /// </summary>

    public class Cia2 {

        public byte[] _registers = new byte[0x10];

        public byte this[Register index] {
            get {
                switch (index) {

                    default:
                        return _registers[(int)index];
                }
            }
            set {
                switch (index) {

                    default:
                        _registers[(int)index] = value;
                        break;
                }
            }
        }

    }
}
