using Commodore64.Cartridge;
using Commodore64.Cia;
using Commodore64.Enums;
using Commodore64.Sid;
using Commodore64.Sid.Enums;
using Commodore64.Vic;
using Commodore64.Vic.Enums;
using Extensions.Byte;
using Extensions.Enums;
using Hardware.Memory;
using Hardware.Mos6526Cia;
using System;
using System.IO;

namespace Commodore64
{

    public class C64Bus : MemoryBase<byte>
    {


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
        private bool _game => _cartridge == null ? true : _cartridge.ControlLineGame;

        // EXROM (pin 9) is pulled logically high (set to 1) through internal pull-up resistor RP4.
        // When a cartridge is attached and EXROM is pulled logically low (cleared to 0),
        // the interaction with the CPU Control Lines can enable cartridge ROM to be banked in between
        // $8000-$BFFF and/or $E000-$EFFF.
        private bool _exRom => _cartridge == null ? true : _cartridge.ControlLineExRom;


        private MemoryBase<byte> _romBasic;
        public MemoryBase<byte> _romCharacter;
        public MemoryBase<byte> _romKernal;

        private Cia1 _cia;
        private Cia2 _cia2;
        private readonly VicIi _vic;
        private readonly SidBase _sid;
        private ICartridge _cartridge;

        public C64Bus(Cia1 cia, Cia2 cia2, VicIi vic, SidBase sid) : base(0x10000)
        {
            //_memory.FillWithRandomData();

            // Intialize processor addressing mode with default values
            // http://sta.c64.org/cbm64mem.html
            _memory[0] = C64MemoryValues.PROCESSOR_PORT_DIRECTION_REGISTER_DEFAULT;
            _memory[1] = C64MemoryValues.PROCESSOR_PORT_REGISTER_DEFAULT;

            _romBasic = new MemoryBase<byte>(File.ReadAllBytes("basic.rom")) { IsReadOnly = true };
            _romCharacter = new MemoryBase<byte>(File.ReadAllBytes("char.rom")) { IsReadOnly = true };
            _romKernal = new MemoryBase<byte>(File.ReadAllBytes("kernal.rom")) { IsReadOnly = true };

            _cia = cia;
            _cia2 = cia2;
            _vic = vic;
            _sid = sid;
        }

        public void InsertCartridge(ICartridge cartridge)
        {
            _cartridge = cartridge;
        }

        public override byte Read(int address)
        {
            // Handle the C64 memory map and the bank switching capabilities of the 6510
            // http://sta.c64.org/cbm64mem.html
            // https://www.c64-wiki.com/wiki/Bank_Switching


            // Processor port. Configuration for memory areas. (Bits 0-2)
            // Memory location 0x01 is hard wired for bank switching
            var bankState = (BankMode)
                ((byte)(_memory[1] & 0b00000111))
                .SetBit(BitFlag.BIT_3, _game)
                .SetBit(BitFlag.BIT_4, _exRom);


            // Always RAM (page 0-15)
            if (address >= 0x0000 && address <= 0x0FFF)
            {
                return base.Read(address);
            }


            // Always RAM (page 16-127)
            // Except when EXROM==1 and GAME==0
            // Some exceptions for cartridge rom, not implemented yet
            if (address >= 0x1000 && address <= 0x7FFF)
            {

                // UNMAPPED
                if (_exRom && !_game)
                {
                    throw new AccessViolationException();
                }

                // RAM
                return base.Read(address);
            }


            // Page 128-159
            // RAM OR CART ROM LO
            if (address >= 0x8000 && address <= 0x9FFF)
            {

                switch (bankState)
                {

                    // CART ROM LO
                    case BankMode.BANK_MODE_23:
                    case BankMode.BANK_MODE_22:
                    case BankMode.BANK_MODE_21:
                    case BankMode.BANK_MODE_20:
                    case BankMode.BANK_MODE_19:
                    case BankMode.BANK_MODE_18:
                    case BankMode.BANK_MODE_17:
                    case BankMode.BANK_MODE_16:
                    case BankMode.BANK_MODE_15:
                    case BankMode.BANK_MODE_11:
                    case BankMode.BANK_MODE_07:
                    case BankMode.BANK_MODE_03:
                        return _cartridge[address];

                    // RAM
                    default:
                        return base.Read(address);
                }

            }


            // BASIC ROM, RAM or CARTRIDGE ROM (page 160-191)
            if (address >= 0xA000 && address <= 0xBFFF)
            {

                // UNMAPPED
                if (_exRom && !_game)
                {
                    throw new AccessViolationException();
                }


                switch (bankState)
                {

                    // BASIC
                    case BankMode.BANK_MODE_31:
                    case BankMode.BANK_MODE_27:
                    case BankMode.BANK_MODE_15:
                    case BankMode.BANK_MODE_11:
                        return _romBasic.Read(address - 0xA000);

                    // CART ROM HI
                    case BankMode.BANK_MODE_07:
                    case BankMode.BANK_MODE_06:
                    case BankMode.BANK_MODE_03:
                    case BankMode.BANK_MODE_02:
                        return _cartridge[address];

                    // RAM
                    default:
                        return base.Read(address);
                }
            }


            // Always RAM (page 192-207)
            if (address >= 0xC000 && address <= 0xCFFF)
            {

                // UNMAPPED
                if (_exRom && !_game)
                {
                    throw new AccessViolationException();
                }

                // RAM
                return base.Read(address);
            }


            // I/O, RAM, CHAR ROM (page 208-223)
            if (address >= 0xD000 && address <= 0xDFFF)
            {

                switch (bankState)
                {
                    // IO
                    case BankMode.BANK_MODE_31:
                    case BankMode.BANK_MODE_30:
                    case BankMode.BANK_MODE_14:
                    case BankMode.BANK_MODE_29:
                    case BankMode.BANK_MODE_13:
                    case BankMode.BANK_MODE_23:
                    case BankMode.BANK_MODE_22:
                    case BankMode.BANK_MODE_21:
                    case BankMode.BANK_MODE_20:
                    case BankMode.BANK_MODE_19:
                    case BankMode.BANK_MODE_18:
                    case BankMode.BANK_MODE_17:
                    case BankMode.BANK_MODE_16:
                    case BankMode.BANK_MODE_15:
                    case BankMode.BANK_MODE_07:
                    case BankMode.BANK_MODE_06:
                    case BankMode.BANK_MODE_05:

                        // VIC-II (0xD000 - 0xD3FF, VIC-II register images repeated every $40, 64 bytes)
                        if (address >= 0xD000 && address <= 0xD3FF)
                        {

                            // The VIC-II class has its own indexer which makes it easy to map
                            // addresses into the VIC-II. The `% 0x40` makes sure that the
                            // registers available in the VIC-II are mirrored all the way up to
                            // 0xD3FF.
                            return _vic[(Register)((address - 0xD000) % 0x40)];

                        }

                        // SID (0xD400 - 0xD7FF, VIC-II register images repeated every $20, 32 bytes)
                        if (address >= 0xD400 && address <= 0xD7FF)
                        {

                            // The SID class has its own indexer which makes it easy to map
                            // addresses into the VIC-II. The `% 0x20` makes sure that the
                            // registers available in the VIC-II are mirrored all the way up to
                            // 0xD7FF.
                            return _sid[(SidRegister)((address - 0xD400) % 0x20)];
                        }

                        // CIA 1
                        if (address >= 0xDC00 && address <= 0xDCFF)
                        {

                            switch (address)
                            {
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
                        if (address >= 0xDD00 && address <= 0xDDFF)
                        {
                            return _cia2[(Cia.Enums.Register)((address - 0xDD00) % 0x10)];
                            //return base.Read(address);
                        }

                        //throw new AccessViolationException();
                        //Debug.WriteLine($"Trying to access I/O at address: 0x{address:X2}");
                        break;

                    // CHAR ROM
                    case BankMode.BANK_MODE_27:
                    case BankMode.BANK_MODE_26:
                    case BankMode.BANK_MODE_10:
                    case BankMode.BANK_MODE_25:
                    case BankMode.BANK_MODE_09:
                    case BankMode.BANK_MODE_11:
                    case BankMode.BANK_MODE_03:
                    case BankMode.BANK_MODE_02:
                        return _romCharacter.Read(address - 0xD000);

                    // RAM
                    default:
                        return base.Read(address);
                }


            }

            // KERNAL ROM, RAM, CARTRIDGE ROM (page 224-255)
            // Some exceptions for I/O and cartridge rom, not implemented yet
            if (address >= 0xE000 && address <= 0xFFFF)
            {

                switch (bankState)
                {

                    // KERNAL ROM
                    case BankMode.BANK_MODE_31:
                    case BankMode.BANK_MODE_30:
                    case BankMode.BANK_MODE_14:
                    case BankMode.BANK_MODE_27:
                    case BankMode.BANK_MODE_26:
                    case BankMode.BANK_MODE_10:
                    case BankMode.BANK_MODE_15:
                    case BankMode.BANK_MODE_11:
                    case BankMode.BANK_MODE_07:
                    case BankMode.BANK_MODE_06:
                    case BankMode.BANK_MODE_03:
                    case BankMode.BANK_MODE_02:
                        return _romKernal.Read(address - 0xE000);

                    // CART ROM HI
                    case BankMode.BANK_MODE_23:
                    case BankMode.BANK_MODE_22:
                    case BankMode.BANK_MODE_21:
                    case BankMode.BANK_MODE_20:
                    case BankMode.BANK_MODE_19:
                    case BankMode.BANK_MODE_18:
                    case BankMode.BANK_MODE_17:
                    case BankMode.BANK_MODE_16:
                        return _cartridge[address];

                    // RAM
                    default:
                        return base.Read(address);
                }

            }

            // RAM
            return base.Read(address);
        }

        public override void Write(int address, byte value)
        {
            var processorPortMemoryConfiguration = _memory[1] & 0b00000111;

            if (address == 0x0001)
            {
                base.Write(address, (byte)(value | ((byte)(value & _memory[0x0000]))));
                return;
            }

            // I/O
            if (processorPortMemoryConfiguration == 0b101 || processorPortMemoryConfiguration == 0b111 || processorPortMemoryConfiguration == 0b110)
            {

                // VIC-II (0xD000 - 0xD3FF, VIC-II register images repeated every $40, 64 bytes)
                if (address >= 0xD000 && address <= 0xD3FF)
                {

                    // The VIC-II class has its own indexer which makes it easy to map
                    // addresses into the VIC-II. The `% 0x40` makes sure that the
                    // registers available in the VIC-II are mirrored all the way up to
                    // 0xD3FF.
                    _vic[(Register)((address - 0xD000) % 0x40)] = value;
                }

                // SID (0xD400 - 0xD7FF, VIC-II register images repeated every $20, 32 bytes)
                if (address >= 0xD400 && address <= 0xD7FF)
                {

                    // The SID class has its own indexer which makes it easy to map
                    // addresses into the VIC-II. The `% 0x20` makes sure that the
                    // registers available in the VIC-II are mirrored all the way up to
                    // 0xD7FF.
                    _sid[(SidRegister)((address - 0xD400) % 0x20)] = value;
                }

                // CIA 1
                if (address >= 0xDC00 && address <= 0xDCFF)
                {
                    // The CIA class has its own indexer which makes it easy to map
                    // addresses into the CIA. The `% 0x10` makes sure that the
                    // 16 registers available in the CIA are mirrored all the way up to
                    // 0xDCFF.
                    _cia[(address - 0xDC00) % 0x10] = value;
                    return;
                }

                // CIA 2
                if (address >= 0xDD00 && address <= 0xDDFF)
                {
                    //base.Write(address, value);
                    //Debug.WriteLine($"Unimplemented CIA2.Write (using base.Write) Address: 0x{address:X4}, Value: 0x{value:X2}");
                    _cia2[(Cia.Enums.Register)((address - 0xDD00) % 0x10)] = value;
                    return;
                }

            }

            base.Write(address, value);
        }
    }

}