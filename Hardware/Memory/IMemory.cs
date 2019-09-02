namespace Hardware.Memory {

    public interface IMemory<TValue> {
        bool IsReadOnly { get; set; }

        TValue Read(int address);
        void Write(int address, TValue value);

        TValue this[int i] { get; set; }
    }
}
