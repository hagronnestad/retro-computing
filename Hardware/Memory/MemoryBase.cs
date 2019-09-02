using System;

namespace Hardware.Memory {

    public class MemoryBase<TValue> : IMemory<TValue> {

        protected TValue[] _memory;

        public MemoryBase(int capacity) {
            _memory = new TValue[capacity];
        }

        public MemoryBase(TValue[] contents) {
            _memory = contents;
        }


        public bool IsReadOnly { get; set; }


        public TValue this[int address] {
            get {
                return Read(address);
            }
            set {
                Write(address, value);
            }
        }


        public virtual TValue Read(int address) {
            return _memory[address];
        }

        public virtual void Write(int address, TValue value) {
            if (IsReadOnly) throw new AccessViolationException("Memory area is read only.");
            _memory[address] = value;
        }
    }
}
