namespace Memory
{

    public class MemoryReadEventArgs<TValue>
    {
        public int Address { get; set; }
        public TValue Value { get; set; }
    }

}
