using Hardware.Memory;
using Hardware.Mos6526Cia;
using System.Diagnostics;
using System.IO;
using System;

namespace Commodore64 {

    public class C64Bus : MemoryBase<byte> {


        // The bank switching technique in the C64 also handles external ROM in the form of a
        // cartridge plugged into the expansion port and is linked to the PLA by 2 lines: GAME and EXROM.
        // These lines have been designed to allow a cartridge to map one or two 8 kByte banks of
        // ROM into the system easily by wiring the ROM's enable pin to ROMH/ROML lines of the port.
        // Extra logic on the cartridge can be used to implement more complex behaviour such as Freezers though.

        // GAME (pin 8) is pulled logically high (set to 1) through internal pull-up resistor RP4.
        // When a cartridge is attached and GAME is pulled logically low (cleared to 0),
        // the interaction with the CPU Control Lines can enable cartridge ROM to be banked in between
        // $8000-$BFFF and/or $E000-$EFFF. GAME can also have the effect of completely banking out all
        // memory between $1000-$7FFF and $A000-$CFFF (Ultimax mode).
        public bool ControlLineGame { get; set; } = true;

        // EXROM (pin 9) is pulled logically high (set to 1) through internal pull-up resistor RP4.
        // When a cartridge is attached and EXROM is pulled logically low (cleared to 0),
        // the interaction with the CPU Control Lines can enable cartridge ROM to be banked in between
        // $8000-$BFFF and/or $E000-$EFFF.
        public bool ControlLineExRom { get; set; } = true;


        private MemoryBase<byte> _romBasic;
        public MemoryBase<byte> _romCharacter;
        private MemoryBase<byte> _romKernal;

        private Cia _cia;
        private readonly VicIi _vic;

        public C64Bus(Cia cia, VicIi vic) : base(0x10000) {
            //_memory.FillWithRandomData();

            // Intialize processor addressing mode with default values
            // http://sta.c64.org/cbm64mem.html
            _memory[0] = C64MemoryValues.PROCESSOR_PORT_DIRECTION_REGISTER_DEFAULT;
            _memory[1] = C64MemoryValues.PROCESSOR_PORT_REGISTER_DEFAULT;

            _romBasic = new MemoryBase<byte>(File.ReadAllBytes("basic.rom")) { IsReadOnly = true };
            _romCharacter = new MemoryBase<byte>(File.ReadAllBytes("char.rom")) { IsReadOnly = true };
            _romKernal = new MemoryBase<byte>(File.ReadAllBytes("kernal.rom")) { IsReadOnly = true };

            _cia = cia;
            _vic = vic;
        }

        public override byte Read(int address) {
            // Handle the C64 memory map and the bank switching capabilities of the 6510
            // http://sta.c64.org/cbm64mem.html
            // https://www.c64-wiki.com/wiki/Bank_Switching

            // Processor port. Configuration for memory areas. (Bits 0-2)
            // Memory location 0x01 is hard wired for bank switching
            var processorPortMemoryConfiguration = _memory[1] & 0b00000111;

            // Always RAM (page 0-15)
            if (address >= 0x0000 && address <= 0x0FFF) {
                return base.Read(address);
            }

            // Always RAM (page 16-127)
            // Except when EXROM==1 and GAME==0
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0x1000 && address <= 0x7FFF) {

                if (ControlLineExRom && !ControlLineGame) {
                    throw new AccessViolationException();
                    // return base.Read(address); <- maybe? I wonder what a real C64 does in this case.
                }

                // RAM
                return base.Read(address);
            }

            // Always RAM (page 128-159)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0x8000 && address <= 0x9FFF) {

                // CART ROM LO
                if (ControlLineExRom && ControlLineGame) {
                    //throw new AccessViolationException();
                    //Debug.WriteLine($"Trying to read CART ROM LO at address: {address:X4}");
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b111) {
                    //throw new AccessViolationException();
                    //Debug.WriteLine($"Trying to read CART ROM LO at address: {address:X4}");
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                    //throw new AccessViolationException();
                    //Debug.WriteLine($"Trying to read CART ROM LO at address: {address:X4}");
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b111) {
                    //throw new AccessViolationException();
                    //Debug.WriteLine($"Trying to read CART ROM LO at address: {address:X4}");
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                    //throw new AccessViolationException();
                    //Debug.WriteLine($"Trying to read CART ROM LO at address: {address:X4}");
                }

                // RAM
                return base.Read(address);
            }

            // BASIC ROM, RAM or CARTRIDGE ROM (page 160-191)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0xA000 && address <= 0xBFFF) {

                if (ControlLineExRom && !ControlLineGame) {
                    throw new AccessViolationException();
                    // return base.Read(address); <- maybe? I wonder what a real C64 does in this case.
                }

                // BASIC
                if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b111) {
                    return _romBasic.Read(address - 0xA000);
                }
                if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                    return _romBasic.Read(address - 0xA000);
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b111) {
                    return _romBasic.Read(address - 0xA000);
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                    return _romBasic.Read(address - 0xA000);
                }

                // CART ROM HI
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b111) {
                    //throw new AccessViolationException();
                    Debug.WriteLine($"Trying to read CART ROM HI at address: {address:X4}");
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b110) {
                    //throw new AccessViolationException();
                    Debug.WriteLine($"Trying to read CART ROM HI at address: {address:X4}");
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                    //throw new AccessViolationException();
                    Debug.WriteLine($"Trying to read CART ROM HI at address: {address:X4}");
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b010) {
                    //throw new AccessViolationException();
                    Debug.WriteLine($"Trying to read CART ROM HI at address: {address:X4}");
                }

                // RAM
                return base.Read(address);
            }

            // Always RAM (page 192-207)
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0xC000 && address <= 0xCFFF) {

                if (ControlLineExRom && !ControlLineGame) {
                    throw new AccessViolationException();
                    // return base.Read(address); <- maybe? I wonder what a real C64 does in this case.
                }

                return base.Read(address);
            }

            // I/O, RAM, CHAR ROM (page 208-223)
            // Some exceptions for I/O, not implemented yet
            if (address >= 0xD000 && address <= 0xDFFF) {

                // RAM
                if (processorPortMemoryConfiguration == 0b000 || processorPortMemoryConfiguration == 0b100) {
                    return base.Read(address);
                }

                // CHAR ROM
                if (processorPortMemoryConfiguration == 0b001 || processorPortMemoryConfiguration == 0b011 || processorPortMemoryConfiguration == 0b010) {
                    return _romCharacter.Read(address - 0xD000);
                }

                // I/O
                if (processorPortMemoryConfiguration == 0b101 || processorPortMemoryConfiguration == 0b111 || processorPortMemoryConfiguration == 0b110) {

                    // VIC-II (0xD000 - 0xD3FF, VIC-II register images repeated every $40, 64 bytes)
                    if (address >= 0xD000 && address <= 0xD3FF) {

                        // The VIC-II class has its own indexer which makes it easy to map
                        // addresses into the VIC-II. The `% 0x40` makes sure that the
                        // registers available in the VIC-II are mirrored all the way up to
                        // 0xD3FF.
                        return _vic[(address - 0xD000) % 0x40];

                    }


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



                    // CIA 2
                    if (address >= 0xDD00 && address <= 0xDDFF) {
                        return base.Read(address);
                    }


                    // throw new Exception($"Trying to read unimplemented I/O at address: {address:X4}");
                }


                //// RAM
                //if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b100) {
                //    return base.Read(address);
                //}
                //if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b000) {
                //    return base.Read(address);
                //}
                //if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b100) {
                //    return base.Read(address);
                //}
                //if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b000) {
                //    return base.Read(address);
                //}
                //if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b100) {
                //    return base.Read(address);
                //}
                //if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b001) {
                //    return base.Read(address);
                //}
                //if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b000) {
                //    return base.Read(address);
                //}

                //// CHAR ROM
                //if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b010) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b010) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b001) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b001) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b011) {
                //    return _romCharacter.Read(address - 0xD000);
                //}
                //if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b010) {
                //    return _romCharacter.Read(address - 0xD000);
                //}

                //// Everything else is I/O

                //Debug.WriteLine($"Trying to read unimplemented I/O at address: {address:X4}");
            }

            // KERNAL ROM, RAM, CARTRIDGE ROM (page 224-255)
            // Some exceptions for I/O and cartridge rom, not implemented yet
            if (address >= 0xE000 && address <= 0xFFFF) {

                // CART ROM HI
                if (ControlLineExRom && !ControlLineGame) {
                    throw new AccessViolationException();
                }

                // RAM
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b101) {
                    return base.Read(address);
                }
                if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b101) {
                    return base.Read(address);
                }
                if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b000) {
                    return base.Read(address);
                }
                if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b100) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b001) {
                    return base.Read(address);
                }
                if (ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b001) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b000) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b100) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b000) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && ControlLineGame && processorPortMemoryConfiguration == 0b100) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b101) {
                    return base.Read(address);
                }
                if (!ControlLineExRom && !ControlLineGame && processorPortMemoryConfiguration == 0b001) {
                    return base.Read(address);
                }

                // Everything else is KERNAL ROM
                return _romKernal.Read(address - 0xE000);
            }

            // RAM
            return base.Read(address);
        }

        public override void Write(int address, byte value) {
            var processorPortMemoryConfiguration = _memory[1] & 0b00000111;

            if (address == 0x0001) {
                base.Write(address, (byte)(value | ((byte)(value & _memory[0x0000]))));
            }

            // I/O
            if (processorPortMemoryConfiguration == 0b101 || processorPortMemoryConfiguration == 0b111 || processorPortMemoryConfiguration == 0b110) {

                // VIC-II (0xD000 - 0xD3FF, VIC-II register images repeated every $40, 64 bytes)
                if (address >= 0xD000 && address <= 0xD3FF) {

                    // The VIC-II class has its own indexer which makes it easy to map
                    // addresses into the VIC-II. The `% 0x40` makes sure that the
                    // registers available in the VIC-II are mirrored all the way up to
                    // 0xD3FF.
                    _vic[(address - 0xD000) % 0x40] = value;

                }

                // CIA 1
                if (address >= 0xDC00 && address <= 0xDCFF) {
                    // The CIA class has its own indexer which makes it easy to map
                    // addresses into the CIA. The `% 0x10` makes sure that the
                    // 16 registers available in the CIA are mirrored all the way up to
                    // 0xDCFF.
                    _cia[(address - 0xDC00) % 0x10] = value;
                    return;
                }

            }


            base.Write(address, value);
        }
    }

}