using Extensions.Enums;
using System.Collections.Generic;

namespace Extensions.Byte {

    public static class ByteExtensions {
        public static byte SetBit(this byte b, BitFlag flag, bool value) {
            return value ? b |= (byte)flag : b &= (byte)~flag;
        }

        public static bool IsBitSet(this byte i, BitIndex index) {
            return (i & 1 << (byte)index) != 0;
        }

        public static bool IsBitSet(this byte i, BitFlag flag) {
            return (i & (byte)flag) == (byte)flag;
        }

        public static List<byte> GetSetBitsIndexes(this byte b) {
            var indexes = new List<byte>();

            for (byte i = 0; i < 8; i++) {
                if (b.IsBitSet((BitIndex)i)) indexes.Add(i);
            }

            return indexes;
        }
    }
}
