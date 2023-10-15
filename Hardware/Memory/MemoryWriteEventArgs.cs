namespace Memory
{

    public class MemoryWriteEventArgs<TValue>
    {
        public int Address { get; set; }
        public TValue Value { get; set; }
    }

}
