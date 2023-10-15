using Commodore64.Cia.Enums;
using Extensions.Enums;
using Extensions.Byte;
using System.Timers;
using System;
using System.Diagnostics;

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
                        _todIsHalted = true;

                        var hb = ToBcd((byte)(_todHaltedHours & 0b00011111));
                        hb.SetBit(BitFlag.BIT_7, hb > 11); // Bit 7 is AM/PM (FALSE = AM / TRUE = PM)
                        return hb;

                    case Register.R_0x0A_TOD_MINUTES:
                        return ToBcd(_todHaltedMinutes);

                    case Register.R_0x09_TOD_SECONDS:
                        return ToBcd(_todHaltedSeconds);

                    case Register.R_0x08_TOD_TENTH_SECONDS:
                        _todIsHalted = false;
                        return ToBcd(_todHaltedTenths);

                    case Register.R_0x01_PORT_B:
                        //Debug.WriteLine("Reading PORTB");
                        return _registers[i];

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
                        _todTenths = _todHaltedTenths = FromBcd(value);
                        _todIsHalted = false;
                        if (!_todIsStarted) ToDStart();
                        break;

                    case Register.R_0x09_TOD_SECONDS:
                        _todSeconds = _todHaltedSeconds = FromBcd(value);
                        break;

                    case Register.R_0x0A_TOD_MINUTES:
                        _todMinutes = _todHaltedMinutes = FromBcd(value);
                        break;

                    case Register.R_0x0B_TOD_HOURS:
                        _todIsHalted = true;
                        _todHours = _todHaltedHours = FromBcd(value);
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


            ToDInit();
        }


        public void Clock() {

        }



        // Time of Day

        private Timer _todTimer;

        private byte _todHours = 0;
        private byte _todMinutes = 0;
        private byte _todSeconds = 0;
        private byte _todTenths = 0;

        private byte _todHaltedHours = 0;
        private byte _todHaltedMinutes = 0;
        private byte _todHaltedSeconds = 0;
        private byte _todHaltedTenths = 0;

        private bool _todIsStarted = false;
        private bool _todIsHalted = false;

        private void ToDInit() {
            _todTimer = new Timer(100) {
                AutoReset = true
            };

            _todTimer.Elapsed += _todTimer_Elapsed;

            _todHours = 1;
            _todHaltedHours = 1;
        }

        private void _todTimer_Elapsed(object sender, ElapsedEventArgs e) {
            ToDUpdate();
        }

        private void ToDStart() {
            _todIsStarted = true;
            _todTimer.Start();
        }

        private void ToDUpdate() {
            _todTenths++;

            if (_todTenths == 10) {
                _todTenths = 0;
                _todSeconds++;
            }

            if (_todSeconds == 60) {
                _todSeconds = 0;
                _todMinutes++;
            }

            if (_todMinutes == 60) {
                _todMinutes = 0;
                _todHours++;
            }

            if (_todHours == 13) {
                _todHours = 1;
            }

            if (!_todIsHalted) {
                _todHaltedTenths = _todTenths;
                _todHaltedSeconds = _todSeconds;
                _todHaltedMinutes = _todMinutes;
                _todHaltedHours = _todHours;
            }
        }


        public static byte ToBcd(byte value) {
            if (value > 159) return 0;

            var r = value % 10;
            var f = (value / 10);

            return (byte)((f * 16) + r);
        }

        public static byte FromBcd(byte value) {
            if (value > 0xF9) return 0;

            var r = value % 16;
            var f = (value / 16);

            return (byte)((f * 10) + r);
        }
    }
}
