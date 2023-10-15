using Memory;
using System;

namespace Hardware.Memory
{

    public class MemoryBase<TValue> : IMemory<TValue>
    {

        public event EventHandler<MemoryReadEventArgs<TValue>> OnRead;
        public event EventHandler<MemoryWriteEventArgs<TValue>> OnWrite;

        public TValue[] _memory;

        public MemoryBase(int capacity)
        {
            _memory = new TValue[capacity];
        }

        public MemoryBase(TValue[] contents)
        {
            _memory = contents;
        }


        public bool IsReadOnly { get; set; }


        public TValue this[int address]
        {
            get
            {
                return Read(address);
            }
            set
            {
                Write(address, value);
            }
        }


        public virtual TValue Read(int address)
        {
            var value = _memory[address];

            OnRead?.Invoke(this, new MemoryReadEventArgs<TValue>()
            {
                Address = address,
                Value = value
            });

            return value;
        }

        public virtual void Write(int address, TValue value)
        {
            OnWrite?.Invoke(this, new MemoryWriteEventArgs<TValue>()
            {
                Address = address,
                Value = value
            });

            if (IsReadOnly) throw new AccessViolationException("Memory area is read only.");
            _memory[address] = value;
        }
    }
}
