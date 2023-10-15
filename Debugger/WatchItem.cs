namespace Debugger
{

    public class WatchItem
    {
        private int _value;

        public int Address { get; set; }

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ValueHex = $"{_value:X2}";
                ValueDecimal = $"{_value}";
            }
        }

        public string ValueHex { get; private set; }
        public string ValueDecimal { get; private set; }

        public string Description { get; set; }
    }

}
