using Commodore64.Cia.Enums;
using Extensions.Byte;
using Extensions.Enums;
using System;
using System.Diagnostics;
using System.Timers;

namespace Commodore64.Cia {

    // Some information and test programs here:
    // https://sourceforge.net/p/vice-emu/code/HEAD/tree/testprogs/CIA/tod/

    /// <summary>
    /// This should be a generic CIA chip class that could be shared between the C64s two CIA chips
    /// or at least be a base class for the common functionality and extended class with
    /// correctly named registers for convenience.
    /// TODO: ^ that and merge with Cia class in Mos6526Cia project
    /// </summary>

    public class Cia2 {

        public byte[] _registers = new byte[0x10];

        public byte this[Register index] {
            get { // Read
                var i = (int)index;

                switch (index) {

                    case Register.R_0x0B_TOD_HOURS:
                        _timeOfDayIsHalted = true;

                        var hour = (byte)_timeOfDayValue.Hour;
                        hour.SetBit(BitFlag.BIT_7, hour > 11); // Bit 7 is AM/PM (FALSE = AM / TRUE = PM)
                        return hour;

                    case Register.R_0x0A_TOD_MINUTES:
                        return (byte)_timeOfDayValue.Minute;

                    case Register.R_0x09_TOD_SECONDS:
                        return (byte)_timeOfDayValue.Second;

                    case Register.R_0x08_TOD_TENTH_SECONDS:
                        _timeOfDayIsHalted = false;

                        return (byte)(_timeOfDayValue.Millisecond / 100);

                    default:
                        return _registers[i];
                }
            }
            set { // Write
                var i = (int)index;

                switch (index) {
                    case Register.R_0x00_PORT_A:
                        // Apply data direction register
                        // Only bits which are set in the data direction register can be set in the port register

                        var ddrA = _registers[(byte)Register.R_0x02_PORT_A_DATA_DIRECTION];

                        // Should have the same effect as the below code if I'm not mistaken
                        _registers[i] = (byte)((value & ddrA) | (_registers[i] & ~ddrA));

                        //if (ddrA.IsBitSet(BitFlag.BIT_0)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_0, value.IsBitSet(BitFlag.BIT_0));
                        //if (ddrA.IsBitSet(BitFlag.BIT_1)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_1, value.IsBitSet(BitFlag.BIT_1));
                        //if (ddrA.IsBitSet(BitFlag.BIT_2)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_2, value.IsBitSet(BitFlag.BIT_2));
                        //if (ddrA.IsBitSet(BitFlag.BIT_3)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_3, value.IsBitSet(BitFlag.BIT_3));
                        //if (ddrA.IsBitSet(BitFlag.BIT_4)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_4, value.IsBitSet(BitFlag.BIT_4));
                        //if (ddrA.IsBitSet(BitFlag.BIT_5)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_5, value.IsBitSet(BitFlag.BIT_5));
                        //if (ddrA.IsBitSet(BitFlag.BIT_6)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_6, value.IsBitSet(BitFlag.BIT_6));
                        //if (ddrA.IsBitSet(BitFlag.BIT_7)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_7, value.IsBitSet(BitFlag.BIT_7));
                        break;

                    case Register.R_0x01_PORT_B:
                        // Apply data direction register
                        // Only bits which are set in the data direction register can be set in the port register

                        var ddrB = _registers[(byte)Register.R_0x03_PORT_B_DATA_DIRECTION];

                        // Should have the same effect as the below code if I'm not mistaken
                        _registers[i] = (byte)((value & ddrB) | (_registers[i] & ~ddrB));

                        //if (ddrB.IsBitSet(BitFlag.BIT_0)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_0, value.IsBitSet(BitFlag.BIT_0));
                        //if (ddrB.IsBitSet(BitFlag.BIT_1)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_1, value.IsBitSet(BitFlag.BIT_1));
                        //if (ddrB.IsBitSet(BitFlag.BIT_2)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_2, value.IsBitSet(BitFlag.BIT_2));
                        //if (ddrB.IsBitSet(BitFlag.BIT_3)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_3, value.IsBitSet(BitFlag.BIT_3));
                        //if (ddrB.IsBitSet(BitFlag.BIT_4)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_4, value.IsBitSet(BitFlag.BIT_4));
                        //if (ddrB.IsBitSet(BitFlag.BIT_5)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_5, value.IsBitSet(BitFlag.BIT_5));
                        //if (ddrB.IsBitSet(BitFlag.BIT_6)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_6, value.IsBitSet(BitFlag.BIT_6));
                        //if (ddrB.IsBitSet(BitFlag.BIT_7)) _registers[i] = _registers[i].SetBit(BitFlag.BIT_7, value.IsBitSet(BitFlag.BIT_7));
                        break;

                    case Register.R_0x08_TOD_TENTH_SECONDS:
                        _timeOfDayStartValue = _timeOfDayValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            _registers[(int)Register.R_0x0B_TOD_HOURS & 0b00011111],
                            _registers[(int)Register.R_0x0A_TOD_MINUTES],
                            _registers[(int)Register.R_0x09_TOD_SECONDS],
                            value);
                        if (!_timeOfDayIsStarted) StartTimeOfDay();
                        _registers[i] = value;
                        break;

                    default:
                        _registers[i] = value;
                        break;
                }
            }
        }


        public Cia2() {
            // This sets the default VIC bank at startup
            // The value is getting set on startup by the KERNAL, but for some reason the
            // R_0x02_PORT_A_DATA_DIRECTION register isn't set up to allow the write at that moment
            // I don't know if either of the registers should have a default value or if they're
            // controlled by hardware, anyway, this works for now...
            _registers[(int)Register.R_0x00_PORT_A] = 0b00000011;


            InitTimeOfDay();
        }


        public void Clock() {

        }



        // Time of Day

        private Stopwatch _timeOfDayStopwatch;
        private Timer _timeOfDayTimer;

        private DateTime _timeOfDayStartValue;
        private DateTime _timeOfDayValue;
        private DateTime _timeOfDayAlarm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

        private bool _timeOfDayIsStarted = false;
        private bool _timeOfDayIsHalted = false;

        private void InitTimeOfDay() {
            _timeOfDayStopwatch = new Stopwatch();
            _timeOfDayStartValue = _timeOfDayValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0, 0);

            // This timer drives the ToD at 60Hz to match the hardware resolution
            _timeOfDayTimer = new Timer(1000.0f / 60.0f);
            _timeOfDayTimer.Elapsed += _timeOfDayTimer_Elapsed;
            _timeOfDayTimer.AutoReset = true;
        }

        private void StartTimeOfDay() {
            _timeOfDayTimer.Start();

            // This stopwatch keeps accurate time elapsed from the ToD reference
            // We can't just add the elapsed time for each _timeOfDayTimer.Elapsed event
            // because that's not accurate enough and drifts very fast
            _timeOfDayStopwatch.Start();

            _timeOfDayIsStarted = true;
        }

        private void _timeOfDayTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if (!_timeOfDayIsHalted) _timeOfDayValue = _timeOfDayStartValue.AddMilliseconds(_timeOfDayStopwatch.ElapsedMilliseconds);

            //Debug.WriteLine(_timeOfDayValue.ToLongTimeString());
        }
    }
}
