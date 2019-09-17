using Hardware.Memory;
using Hardware.Mos6526Cia;
using System.Diagnostics;
using System.IO;
using Extensions.Byte;

namespace Commodore64 {

    public class C64Memory : MemoryBase<byte> {

        private MemoryBase<byte> _romBasic;
        public MemoryBase<byte> _romCharacter;
        private MemoryBase<byte> _romKernal;

        private Cia _cia;

        public C64Memory(Cia cia) : base(0x10000) {
            _memory.FillWithRandomData();

            _romBasic = new MemoryBase<byte>(File.ReadAllBytes("basic.rom")) { IsReadOnly = true };
            _romCharacter = new MemoryBase<byte>(File.ReadAllBytes("char.rom")) { IsReadOnly = true };
            _romKernal = new MemoryBase<byte>(File.ReadAllBytes("kernal.rom")) { IsReadOnly = true };

            _cia = cia;

            // Intialize processor addressing mode with default values
            // http://sta.c64.org/cbm64mem.html
            _memory[0] = C64MemoryValues.PROCESSOR_PORT_DIRECTION_REGISTER_DEFAULT;
            _memory[1] = C64MemoryValues.PROCESSOR_PORT_REGISTER_DEFAULT;
        }

        public override byte Read(int address) {
            // Handle the C64 memory map and the bank switching capabilities of the 6510
            // http://sta.c64.org/cbm64mem.html
            // https://www.c64-wiki.com/wiki/Bank_Switching

            // Processor addressing mode
            // Memory location 0x01 is hard wired for bank switching
            var am = _memory[1] & 0b00000111;

            // Always RAM (page 0-15)
            if (address >= 0x0000 && address <= 0x0FFF) {
                return base.Read(address);
            }

            // Always RAM (page 16-127)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0x1000 && address <= 0x7FFF) {
                return base.Read(address);
            }

            // Always RAM (page 128-159)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0x8000 && address <= 0x9FFF) {
                return base.Read(address);
            }

            // BASIC ROM, RAM or CARTRIDGE ROM (page 160-191)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0xA000 && address <= 0xBFFF) {
                switch (am) {
                    case 0b111:
                    case 0b011:
                        return _romBasic.Read(address - 0xA000);
                }

                return base.Read(address);
            }

            // Always RAM (page 192-207)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0xC000 && address <= 0xCFFF) {
                return base.Read(address);
            }

            // I/O, RAM, CHAR ROM (page 208-223)
            // Some exceptions for I/O, not implemented yet
            if (address >= 0xD000 && address <= 0xDFFF) {

                // CIA 1
                if (address >= 0xDC00 && address <= 0xDCFF) {

                    switch (address) {
                        case 0xDC09:
                            return _cia.TimeOfDaySecondsBcd;
                        case 0xDC0A:
                            return _cia.TimeOfDayMinutesBcd;
                        case 0xDC0B:
                            return _cia.TimeOfDayHoursBcd;

                        default:
                            // The CIA class has its own indexer which makes it easy to map
                            // addresses into the CIA. The `% 0x10` makes sure that the
                            // 16 registers available in the CIA are mirrored all the way up to
                            // 0xDCFF.
                            return _cia[(address - 0xDC00) % 0x10];
                    }

                }


                switch (am) {
                    case 0b011:
                    case 0b010:
                    case 0b001:
                        return _romCharacter.Read(address - 0xC000);
                }

                return base.Read(address);
            }

            // KERNAL ROM, RAM, CARTRIDGE ROM (page 224-255)
            // Some exceptions for I/O and cartridge rom, not implemented yet
            if (address >= 0xE000 && address <= 0xFFFF) {
                switch (am) {
                    case 0b111:
                    case 0b110:
                    case 0b011:
                    case 0b010:
                        return _romKernal.Read(address - 0xE000);
                }

                return base.Read(address);
            }

            return base.Read(address);
        }

        public override void Write(int address, byte value) {
            if (address == 0x0801) Debug.WriteLine($"Value: 0x{value:X2} written to Address: 0x{address:X4}");
            if (address == 0x0801) Debug.WriteLine($"Value: 0x{value:X2} written to Address: 0x{address:X4}");

            if (address == 0x0281) Debug.WriteLine($"Value: 0x{value:X2} written to Address: 0x{address:X4}");
            if (address == 0x0282) Debug.WriteLine($"Value: 0x{value:X2} written to Address: 0x{address:X4}");


            // CIA 1
            if (address >= 0xDC00 && address <= 0xDCFF) {
                // The CIA class has its own indexer which makes it easy to map
                // addresses into the CIA. The `% 0x10` makes sure that the
                // 16 registers available in the CIA are mirrored all the way up to
                // 0xDCFF.
                _cia[(address - 0xDC00) % 0x10] = value;
                return;
            }

            base.Write(address, value);
        }
    }

}