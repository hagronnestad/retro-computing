using System;
using System.Timers;

namespace Hardware.Mos6526Cia {

    public class Cia {
        
        private DateTime _timeOfDay;
        private Timer _timer;

        public event EventHandler ReadDataPortA;
        public event EventHandler ReadDataPortB;

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

        public Cia() {
            _timeOfDay = DateTime.Now;

            _timer = new Timer(100);
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                _timeOfDay = _timeOfDay.AddMilliseconds(_timer.Interval);
            };
            _timer.Start();
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

                    default:
                        // TODO: Implement all registers
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
                        
                    default:
                        // TODO: Implement all registers
                        //throw new IndexOutOfRangeException();
                        break;
                }
            }
        }

        public void Clock() {
            
        }
    }

}
