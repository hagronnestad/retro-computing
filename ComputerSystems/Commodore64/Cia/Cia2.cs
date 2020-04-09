using Commodore64.Cia.Enums;
using Extensions.Byte;
using Extensions.Enums;

namespace Commodore64.Cia {
    
    /// <summary>
    /// This should be a generic CIA chip class that could be shared between the C64s two CIA chips
    /// or at least be a base class for the common functionality and extended class with
    /// correctly named registers for convenience.
    /// TODO: ^ that and merge with Cia class in Mos6526Cia project
    /// </summary>

    public class Cia2 {

        public byte[] _registers = new byte[0x10];

        public Cia2() {
            _registers[0] = 0b00000011;
        }

        public byte this[Register index] {
            get { // Read
                var i = (int)index;

                switch (index) {

                    default:
                        return _registers[i];
                }
            }
            set { // Write
                var i = (int) index;

                switch (index) {
                    case Register.R_0x00_PORT_A:
                        // Apply data direction register
                        // Only bits which are set in the data direction register can be set in the port register

                        var ddrA = _registers[(byte)Register.R_0x02_PORT_A_DATA_DIRECTION];
                        
                        // Should have the same effect as the below code if I'm not mistaken
                        _registers[i] = (byte)((value & ddrA) | (_registers[i] & ~ddrA));

                        //if (ddrA.IsBitSet(BitFlag.BIT_0)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_0, value.IsBitSet(BitFlag.BIT_0));
                        //if (ddrA.IsBitSet(BitFlag.BIT_1)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_1, value.IsBitSet(BitFlag.BIT_1));
                        //if (ddrA.IsBitSet(BitFlag.BIT_2)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_2, value.IsBitSet(BitFlag.BIT_2));
                        //if (ddrA.IsBitSet(BitFlag.BIT_3)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_3, value.IsBitSet(BitFlag.BIT_3));
                        //if (ddrA.IsBitSet(BitFlag.BIT_4)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_4, value.IsBitSet(BitFlag.BIT_4));
                        //if (ddrA.IsBitSet(BitFlag.BIT_5)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_5, value.IsBitSet(BitFlag.BIT_5));
                        //if (ddrA.IsBitSet(BitFlag.BIT_6)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_6, value.IsBitSet(BitFlag.BIT_6));
                        //if (ddrA.IsBitSet(BitFlag.BIT_7)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_7, value.IsBitSet(BitFlag.BIT_7));
                        break;

                    case Register.R_0x01_PORT_B:
                        // Apply data direction register
                        // Only bits which are set in the data direction register can be set in the port register

                        var ddrB = _registers[(byte)Register.R_0x03_PORT_B_DATA_DIRECTION];

                        // Should have the same effect as the below code if I'm not mistaken
                        _registers[i] = (byte)((value & ddrB) | (_registers[i] & ~ddrB));

                        //if (ddrB.IsBitSet(BitFlag.BIT_0)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_0, value.IsBitSet(BitFlag.BIT_0));
                        //if (ddrB.IsBitSet(BitFlag.BIT_1)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_1, value.IsBitSet(BitFlag.BIT_1));
                        //if (ddrB.IsBitSet(BitFlag.BIT_2)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_2, value.IsBitSet(BitFlag.BIT_2));
                        //if (ddrB.IsBitSet(BitFlag.BIT_3)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_3, value.IsBitSet(BitFlag.BIT_3));
                        //if (ddrB.IsBitSet(BitFlag.BIT_4)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_4, value.IsBitSet(BitFlag.BIT_4));
                        //if (ddrB.IsBitSet(BitFlag.BIT_5)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_5, value.IsBitSet(BitFlag.BIT_5));
                        //if (ddrB.IsBitSet(BitFlag.BIT_6)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_6, value.IsBitSet(BitFlag.BIT_6));
                        //if (ddrB.IsBitSet(BitFlag.BIT_7)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_7, value.IsBitSet(BitFlag.BIT_7));
                        break;

                    default:
                        _registers[i] = value;
                        break;
                }
            }
        }


        public void Clock() {
        
        }
    }
}
