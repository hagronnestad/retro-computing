using Memory;
using System;

namespace Hardware.Memory
{

    public interface IMemory<TValue>
    {
        event EventHandler<MemoryReadEventArgs<TValue>> OnRead;
        event EventHandler<MemoryWriteEventArgs<TValue>> OnWrite;

        bool IsReadOnly { get; }

        TValue Read(int address);
        void Write(int address, TValue value);

        TValue this[int i] { get; set; }
    }
}
