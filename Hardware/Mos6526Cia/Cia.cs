using Extensions.Byte;
using Extensions.Enums;
using System;
using System.Diagnostics;
using System.Timers;

namespace Hardware.Mos6526Cia {

    public class Cia1 {

        private const double INTERRUPT_INTERVAL = 1000.0f / 60.0f;


        private DateTime _timeOfDay;
        private Timer _timer;

        private Stopwatch _swInterrupt;


        public event EventHandler Interrupt;
        public event EventHandler ReadDataPortA;
        public event EventHandler ReadDataPortB;



        bool EnableInterruptTimerA = false;
        bool EnableInterruptTimerB = false;
        bool EnableInterruptTodAlarm = false;

        bool LatchInterruptTimerA = false;
        bool LatchUnderflowTimerA = false;


        public byte DataPortA { get; set; }
        public byte DataPortB { get; set; }
        public byte DataDirectionA { get; set; }
        public byte DataDirectionB { get; set; }


        public byte TimeOfDayHoursBcd {
            get {
                return (byte)_timeOfDay.Hour;
            }
            set {
            }
        }

        public byte TimeOfDayMinutesBcd {
            get {
                return (byte)_timeOfDay.Minute;
            }
            set {
            }
        }

        public byte TimeOfDaySecondsBcd {
            get {
                return (byte)_timeOfDay.Second;
            }
            set {
            }
        }

        public Cia1() {
            _timeOfDay = DateTime.Now;

            _timer = new Timer(100);
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                _timeOfDay = _timeOfDay.AddMilliseconds(_timer.Interval);
            };
            _timer.Start();

            _swInterrupt = Stopwatch.StartNew();
        }



        /// <summary>
        /// Converts an int to binary coded decimal (BCD)
        /// Taken from: https://stackoverflow.com/a/7107479/1240844
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBcd(int value) {
            if (value < 0 || value > 99999999) throw new ArgumentOutOfRangeException(nameof(value));

            byte[] ret = new byte[4];

            for (int i = 0; i < ret.Length; i++) {
                ret[i] = (byte)(value % 10);
                value /= 10;
                ret[i] |= (byte)((value % 10) << 4);
                value /= 10;
            }

            return ret;
        }


        public byte this[int index] {
            get {
                switch (index) {
                    case 0:
                        ReadDataPortA?.Invoke(this, null);
                        return DataPortA;

                    case 1:
                        ReadDataPortB?.Invoke(this, null);
                        return DataPortB;

                    case 2:
                        return DataDirectionA;

                    case 3:
                        return DataDirectionB;

                    // 0xDC0D
                    case 0x0D:
                        byte b = 0;
                        if (LatchInterruptTimerA) {
                            LatchInterruptTimerA = false;
                            b = b.SetBit(BitFlag.BIT_7, true);
                        }
                        if (LatchUnderflowTimerA) {
                            LatchUnderflowTimerA = false;
                            b = b.SetBit(BitFlag.BIT_0, true);
                        }
                        return b;

                    default:
                        // TODO: Implement all registers
                        Debug.WriteLine($"[READ] CIA register not implemented: 0xDC{index:X2}");
                        //throw new IndexOutOfRangeException();
                        return 0;
                }
            }
            set {
                switch (index) {
                    case 0:
                        DataPortA = value;
                        break;

                    case 1:
                        DataPortB = value;
                        break;

                    case 2:
                        DataDirectionA = value;
                        break;

                    case 3:
                        DataDirectionB = value;
                        break;

                    // 0xDC0D
                    case 0x0D:
                        var fillBit = value.IsBitSet(BitFlag.BIT_7);

                        if (value.IsBitSet(BitFlag.BIT_0)) EnableInterruptTimerA = fillBit;
                        if (value.IsBitSet(BitFlag.BIT_1)) EnableInterruptTimerB = fillBit;
                        if (value.IsBitSet(BitFlag.BIT_2)) EnableInterruptTodAlarm = fillBit;

                        break;
                        
                    default:
                        // TODO: Implement all registers
                        Debug.WriteLine($"[WRITE] CIA register not implemented: 0xDC{index:X2} (Value: 0x{value:X2})");
                        //throw new IndexOutOfRangeException();
                        break;
                }
            }
        }

        public void Clock() {

            // Should interrupt?
            if (_swInterrupt.Elapsed.TotalMilliseconds > INTERRUPT_INTERVAL) {
                LatchUnderflowTimerA = true;

                if (EnableInterruptTimerA) {
                    LatchInterruptTimerA = true;
                    Interrupt?.Invoke(this, null);
                }
                _swInterrupt.Restart();
            }

        }
    }

}
