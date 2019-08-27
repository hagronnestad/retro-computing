namespace Extensions {
    public static class ByteExtensions {
        public static byte SetBit(this byte b, BitFlag flag, bool value) {
            return value ? b |= (byte)flag : b &= (byte)~flag;
        }

        public static bool IsBitSet(this byte i, BitIndex index) {
            return (i & (1 << (byte) index)) != 0;
        }

        public static bool IsBitSet(this byte i, BitFlag flag) {
            return (i & (byte)flag) == (byte)flag;
        }
    }
}
