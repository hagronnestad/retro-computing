namespace Hardware.Memory {

    public abstract class Memory<T> {

        private T[] _memory;

        public Memory(long size) {
            _memory = new T[size];
        }

        public T Read(long address) {
            return _memory[address];
        }

        public void Write(long address, T value) {
            _memory[address] = value;
        }

    }
}
