using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cpu6502 {
    public class Cpu6502 {
        public List<OpCodeDefinition> OpCodes { get; private set; }
        public Dictionary<byte, OpCodeDefinition> OpCodeCache { get; set; }

        public byte[] Memory = new byte[0x100000];

        public ushort PC;

        /// <summary>
        /// Stack Pointer. The SP starts at 0xFF and is decremented on push.
        /// The stack stores its values in address range 0x01FF - 0x0100.
        /// </summary>
        public byte SP;

        public byte AR;
        public byte XR;
        public byte YR;

        public StatusRegister SR = new StatusRegister();


        public OpCodeDefinition OpCode { get; set; }
        public ushort OpCodeAddress;

        public OpCodeDefinition NextOpCode { get; set; }
        public ushort NextOpCodeAddress;

        public ushort Address;
        public byte Value {
            get {
                return OpCode.AddressingMode == AddressingMode.Accumulator ? AR : Memory[Address];
            }
            set {
                if (OpCode.AddressingMode == AddressingMode.Accumulator) {
                    AR = value;

                } else {
                    Memory[Address] = value;
                }
            }
        }


        public Cpu6502() {
            //OpCodes = GetType()
            //    .GetMethods()
            //    .SelectMany(m => m.GetCustomAttributes(typeof(OpCodeAttribute), true)
            //    .Select(a => new {
            //        Attribute = a as OpCodeAttribute,
            //        Method = m
            //    }))
            //    .Select(a => OpCodeDefinition.FromOpCodeAttribute(() => a.Method.Invoke(this, null), null, a.Attribute))

            //    .ToList();

            var opCodesMethods = GetType()
                .GetMethods()
                .SelectMany(m => m.GetCustomAttributes(typeof(OpCodeAttribute), true)
                .Select(a => new {
                    Attribute = a as OpCodeAttribute,
                    Method = m
                }));

            var addressingMethods = GetType()
                .GetMethods()
                .SelectMany(m => m.GetCustomAttributes(typeof(AddressingModeAttribute), true)
                .Select(a => new {
                    Attribute = a as AddressingModeAttribute,
                    Method = m
                }));

            OpCodes = opCodesMethods
                .Select(x => OpCodeDefinition.FromOpCodeAttribute(() => x.Method.Invoke(this, null), () => addressingMethods.FirstOrDefault(y => y.Attribute.AddressingMode == x.Attribute.AddressingMode).Method.Invoke(this, null), x.Attribute))
                .ToList();

            OpCodeCache = OpCodes.ToDictionary(x => x.Code, x => x);

            Reset();
        }

        public void LoadMemory(byte[] data, int location) {
            if (data == null || data.Length == 0) throw new ArgumentNullException();
            if ((location + data.Length) > Memory.Length) throw new OutOfMemoryException();

            Array.Copy(data, 0, Memory, location, data.Length);
        }

        public void InitPC(ushort pc) {
            PC = pc;

            NextOpCode = OpCodeCache[Memory[PC]];
            NextOpCodeAddress = PC;
        }

        public void Reset() {
            // From https://youtu.be/8XmxKPJDGU0?t=3252

            AR = XR = YR = 0x00;
            SP = 0xFD;
            SR.Register = 0x00;

            SR.Reserved = true; // From YouTube, undocumented way to detect reset/IRQ?

            var resetVectorAddress = 0xFFFC;
            PC = (ushort)((Memory[resetVectorAddress + 1] << 8) | Memory[resetVectorAddress + 0]);

            // Reset takes 8 cycles
        }

        public void Interrupt() { // IRQ
            // From https://youtu.be/8XmxKPJDGU0?t=3362

            if (SR.IrqDisable) return;

            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0x00FF));

            SR.BreakCommand = false;
            SR.Reserved = true; // From YouTube, undocumented way to detect reset/IRQ?
            SR.IrqDisable = true;

            PushStack(SR.Register);

            var irqVectorAddress = 0xFFFE;
            PC = (ushort)((Memory[irqVectorAddress + 1] << 8) | Memory[irqVectorAddress + 0]);

            // IRQ takes 7 cycles
        }

        public void NonMaskableInterrupt() { // NMI
            // From https://youtu.be/8XmxKPJDGU0?t=3362

            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0x00FF));

            SR.BreakCommand = false;
            SR.Reserved = true; // From YouTube, undocumented way to detect reset/IRQ?
            SR.IrqDisable = true;

            PushStack(SR.Register);

            var nmiVectorAddress = 0xFFFA;
            PC = (ushort)((Memory[nmiVectorAddress + 1] << 8) | Memory[nmiVectorAddress + 0]);

            // NMI takes 8 cycles
        }

        public void PushStack(byte value) {
            Memory[0x0100 + SP] = value;
            SP--;
        }

        public byte PopStack() {
            SP++;
            var b = Memory[0x0100 + SP];
            return b;
        }


        public void Step() {

            OpCode = OpCodeCache[Memory[PC]];
            OpCodeAddress = PC;
            PC++;

            Debug.WriteLine("");
            Debug.WriteLine($"OpCode: {OpCode.Code:X2}, OpCodeName: {OpCode.Name}, AddressingMode: {OpCode.AddressingMode}");
            Debug.WriteLine($"PC: {PC:X2}, AR: {AR:X2}, XR: {XR:X2}, YR: {YR:X2}, SP: {SP:X2}, ZERO: {SR.Zero}, NEGATIVE: {SR.Negative}, M 0x50:{Memory[0x50]:X2}");

            if (OpCode.AddressingMode != AddressingMode.Implied && OpCode.AddressingMode != AddressingMode.Accumulator) {
                OpCode.GetAddress();
            }
            OpCode.Run();

            Debug.WriteLine($"PC: {PC:X2}, AR: {AR:X2}, XR: {XR:X2}, YR: {YR:X2}, SP: {SP:X2}, ZERO: {SR.Zero}, NEGATIVE: {SR.Negative}, M 0x50:{Memory[0x50]:X2}");
            Debug.WriteLine("");

            NextOpCode = OpCodeCache[Memory[PC]];
            NextOpCodeAddress = PC;
        }


        [AddressingMode(AddressingMode = AddressingMode.Absolute)]
        public void Absolute() {
            var lowAddressByte = Memory[PC];
            PC++;
            var highAddressByte = Memory[PC];
            PC++;

            Address = (ushort)((highAddressByte << 8) | lowAddressByte);
        }

        [AddressingMode(AddressingMode = AddressingMode.AbsoluteX)]
        public void AbsoluteX() {
            var lowAddressByte = Memory[PC];
            PC++;
            var highAddressByte = Memory[PC];
            PC++;

            Address = (ushort)((highAddressByte << 8) | lowAddressByte);
            Address += XR;
        }

        [AddressingMode(AddressingMode = AddressingMode.AbsoluteY)]
        public void AbsoluteY() {
            var lowAddressByte = Memory[PC];
            PC++;
            var highAddressByte = Memory[PC];
            PC++;

            Address = (ushort)((highAddressByte << 8) | lowAddressByte);
            Address += YR;
        }

        [AddressingMode(AddressingMode = AddressingMode.Immediate)]
        public void Immediate() {
            Address = PC;
            PC++;
        }

        [AddressingMode(AddressingMode = AddressingMode.Indirect)]
        public void Indirect() {
            var lowAddressPointer = Memory[PC];
            PC++;
            var highAddressPointer = Memory[PC];
            PC++;

            var addressPointer = (ushort)((highAddressPointer << 8) | lowAddressPointer);

            if (lowAddressPointer == 0xFF) {
                // This handles a hardware bug in the 6502
                Address = (ushort)(Memory[addressPointer & 0xFF00] << 8 | Memory[addressPointer]);

            } else {
                Address = (ushort)(Memory[addressPointer + 1] << 8 | Memory[addressPointer]);
            }
        }

        [AddressingMode(AddressingMode = AddressingMode.XIndirect)]
        public void XIndirect() {
            var pageZeroAddress = Memory[PC];
            PC++;

            var lowAddress = Memory[pageZeroAddress + XR];
            var highAddress = Memory[pageZeroAddress + 1 + XR];

            Address = (ushort)((highAddress << 8) | lowAddress);
        }

        [AddressingMode(AddressingMode = AddressingMode.IndirectY)]
        public void IndirectY() {
            var pageZeroAddress = Memory[PC];
            PC++;

            var lowAddress = Memory[pageZeroAddress];
            var highAddress = Memory[pageZeroAddress + 1];

            Address = (ushort)((highAddress << 8) | lowAddress);
            Address += YR;
        }

        [AddressingMode(AddressingMode = AddressingMode.Relative)]
        public void Relative() {
            sbyte rel_addr = (sbyte)Memory[PC];
            PC++;
            Address = (ushort)(PC + rel_addr);
        }

        [AddressingMode(AddressingMode = AddressingMode.Zeropage)]
        public void Zeropage() {
            Address = Memory[PC];
            PC++;
        }

        [AddressingMode(AddressingMode = AddressingMode.ZeropageX)]
        public void ZeropageX() {
            Address = (ushort)(Memory[PC] + XR);
            PC++;
        }

        [AddressingMode(AddressingMode = AddressingMode.ZeropageY)]
        public void ZeropageY() {
            Address = (ushort)(Memory[PC] + YR);
            PC++;
        }





        // STORAGE

        [OpCode(Name = nameof(LDA), Code = 0xA9, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(LDA), Code = 0xA5, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(LDA), Code = 0xB5, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(LDA), Code = 0xAD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(LDA), Code = 0xBD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(LDA), Code = 0xB9, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(LDA), Code = 0xA1, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(LDA), Code = 0xB1, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void LDA() {
            SR.SetNegative(Value);
            SR.SetZero(Value);
            AR = Value;
        }

        [OpCode(Name = nameof(LDX), Code = 0xA2, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(LDX), Code = 0xA6, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(LDX), Code = 0xB6, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(LDX), Code = 0xAE, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(LDX), Code = 0xBE, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        public void LDX() {
            SR.SetNegative(Value);
            SR.SetZero(Value);
            XR = Value;
        }

        [OpCode(Name = nameof(LDY), Code = 0xA0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(LDY), Code = 0xA4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(LDY), Code = 0xB4, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(LDY), Code = 0xAC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(LDY), Code = 0xBC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        public void LDY() {
            SR.SetNegative(Value);
            SR.SetZero(Value);
            YR = Value;
        }

        [OpCode(Name = nameof(STA), Code = 0x85, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(STA), Code = 0x95, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(STA), Code = 0x8D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(STA), Code = 0x9D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(STA), Code = 0x99, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(STA), Code = 0x81, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(STA), Code = 0x91, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void STA() {
            Value = AR;
        }

        [OpCode(Name = nameof(STX), Code = 0x86, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(STX), Code = 0x96, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageY)]
        [OpCode(Name = nameof(STX), Code = 0x8E, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void STX() {
            Value = XR;
        }

        [OpCode(Name = nameof(STY), Code = 0x84, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(STY), Code = 0x94, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(STY), Code = 0x8C, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void STY() {
            Value = YR;
        }

        [OpCode(Name = nameof(TAX), Code = 0xAA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void TAX() {
            SR.SetNegative(AR);
            SR.SetZero(AR);
            XR = AR;
        }

        [OpCode(Name = nameof(TAY), Code = 0xA8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void TAY() {
            SR.SetNegative(AR);
            SR.SetZero(AR);
            YR = AR;
        }

        [OpCode(Name = nameof(TSX), Code = 0xBA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void TSX() {
            SR.SetNegative(SP);
            SR.SetZero(SP);
            XR = SP;
        }

        [OpCode(Name = nameof(TXA), Code = 0x8A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void TXA() {
            SR.SetNegative(XR);
            SR.SetZero(XR);
            AR = XR;
        }

        [OpCode(Name = nameof(TXS), Code = 0x9A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void TXS() {
            SR.SetNegative(XR);
            SR.SetZero(XR);
            SP = XR;
        }

        [OpCode(Name = nameof(TYA), Code = 0x98, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void TYA() {
            SR.SetNegative(YR);
            SR.SetZero(YR);
            AR = YR;
        }



        // BITWISE

        [OpCode(Name = nameof(AND), Code = 0x29, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(AND), Code = 0x25, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(AND), Code = 0x35, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(AND), Code = 0x2D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(AND), Code = 0x3D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(AND), Code = 0x39, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(AND), Code = 0x21, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(AND), Code = 0x31, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void AND() {
            AR = (byte)(Value & AR);

            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCode(Name = nameof(ASL), Code = 0x0A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpCode(Name = nameof(ASL), Code = 0x06, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(ASL), Code = 0x16, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(ASL), Code = 0x0E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(ASL), Code = 0x1E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX)]
        public void ASL() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#ASL
            // TODO: Needs verification!

            SR.Carry = Value.IsBitSet(BitFlag.BIT_7);
            Value <<= 1;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCode(Name = nameof(BIT), Code = 0x24, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(BIT), Code = 0x2C, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void BIT() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BIT
            // TODO: Needs verification!

            var r = (byte)(Value & AR);

            SR.Negative = Value.IsBitSet((BitFlag)ProcessorStatusFlags.Negative);
            SR.Overflow = Value.IsBitSet((BitFlag)ProcessorStatusFlags.Overflow);
            SR.SetZero(r);
        }

        [OpCode(Name = nameof(EOR), Code = 0x49, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(EOR), Code = 0x45, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(EOR), Code = 0x55, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(EOR), Code = 0x4D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(EOR), Code = 0x5D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(EOR), Code = 0x59, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(EOR), Code = 0x41, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(EOR), Code = 0x51, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void EOR() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#EOR
            // TODO: Needs verification!

            AR = (byte)(Value ^ AR);

            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCode(Name = nameof(LSR), Code = 0x4A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpCode(Name = nameof(LSR), Code = 0x46, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(LSR), Code = 0x56, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(LSR), Code = 0x4E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(LSR), Code = 0x5E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX)]
        public void LSR() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#LSR
            // TODO: Needs verification!

            SR.Carry = Value.IsBitSet(BitFlag.BIT_0);
            Value >>= 1;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCode(Name = nameof(ORA), Code = 0x09, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(ORA), Code = 0x05, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(ORA), Code = 0x15, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(ORA), Code = 0x0D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(ORA), Code = 0x1D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(ORA), Code = 0x19, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(ORA), Code = 0x01, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(ORA), Code = 0x11, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void ORA() {
            AR = (byte)(Value | AR);
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCode(Name = nameof(ROL), Code = 0x2A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpCode(Name = nameof(ROL), Code = 0x26, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(ROL), Code = 0x36, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(ROL), Code = 0x2E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(ROL), Code = 0x3E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX)]
        public void ROL() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#ROL
            // TODO: Needs verification!

            var c = SR.Carry;
            SR.Carry = Value.IsBitSet(BitFlag.BIT_7);
            Value <<= 1;
            Value = Value.SetBit(BitFlag.BIT_0, c);

            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCode(Name = nameof(ROR), Code = 0x6A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpCode(Name = nameof(ROR), Code = 0x66, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(ROR), Code = 0x76, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(ROR), Code = 0x6E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(ROR), Code = 0x7E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX)]
        public void ROR() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#ROR
            // TODO: Needs verification!

            var c = SR.Carry;
            SR.Carry = Value.IsBitSet(BitFlag.BIT_0);
            Value >>= 1;
            Value = Value.SetBit(BitFlag.BIT_7, c);

            SR.SetNegative(Value);
            SR.SetZero(Value);
        }


        // MATH

        [OpCode(Name = nameof(ADC), Code = 0x69, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(ADC), Code = 0x65, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(ADC), Code = 0x75, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(ADC), Code = 0x6D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(ADC), Code = 0x7D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(ADC), Code = 0x79, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(ADC), Code = 0x61, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(ADC), Code = 0x71, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void ADC() {
            var temp = AR + Value + (SR.Carry ? 0 : 1);

            SR.Carry = temp > 255;
            SR.Overflow = ((AR ^ temp) & ~(AR ^ Value) & 0b10000000) == 0b10000000;
            SR.SetZero((byte)temp);
            SR.SetNegative((byte)temp);
        }

        [OpCode(Name = nameof(SBC), Code = 0xE9, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(SBC), Code = 0xE5, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(SBC), Code = 0xF5, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(SBC), Code = 0xED, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(SBC), Code = 0xFD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(SBC), Code = 0xF9, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(SBC), Code = 0xE1, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(SBC), Code = 0xF1, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void SBC() {
            // https://youtu.be/8XmxKPJDGU0?t=3141

            var temp = AR + (Value ^ 0x00FF) + (SR.Carry ? 0 : 1);

            SR.Carry = temp > 255;
            SR.Overflow = ((AR ^ temp) & ~(AR ^ Value) & 0b10000000) == 0b10000000;
            SR.SetZero((byte)temp);
            SR.SetNegative((byte)temp);
        }


        [OpCode(Name = nameof(DEC), Code = 0xC6, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(DEC), Code = 0xD6, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(DEC), Code = 0xCE, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(DEC), Code = 0xDE, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX)]
        public void DEC() {
            Value--;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCode(Name = nameof(DEX), Code = 0xCA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void DEX() {
            XR--;
            SR.SetNegative(XR);
            SR.SetZero(XR);
        }

        [OpCode(Name = nameof(DEY), Code = 0x88, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void DEY() {
            YR--;
            SR.SetNegative(YR);
            SR.SetZero(YR);
        }

        [OpCode(Name = nameof(INC), Code = 0xE6, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(INC), Code = 0xF6, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(INC), Code = 0xEE, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(INC), Code = 0xFE, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX)]
        public void INC() {
            Value++;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCode(Name = nameof(INX), Code = 0xE8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void INX() {
            XR++;
            SR.SetNegative(XR);
            SR.SetZero(XR);
        }

        [OpCode(Name = nameof(INY), Code = 0xC8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void INY() {
            YR++;
            SR.SetNegative(YR);
            SR.SetZero(YR);
        }




        // REGISTERS

        [OpCode(Name = nameof(CLC), Code = 0x18, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void CLC() {
            SR.Carry = false;
        }

        [OpCode(Name = nameof(SEC), Code = 0x38, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void SEC() {
            SR.Carry = true;
        }

        [OpCode(Name = nameof(CLI), Code = 0x58, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void CLI() {
            SR.IrqDisable = false;
        }

        [OpCode(Name = nameof(SEI), Code = 0x78, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void SEI() {
            SR.IrqDisable = true;
        }

        [OpCode(Name = nameof(CLV), Code = 0xB8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void CLV() {
            SR.Overflow = false;
        }

        [OpCode(Name = nameof(CLD), Code = 0xD8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void CLD() {
            SR.DecimalMode = false;
        }

        [OpCode(Name = nameof(SED), Code = 0xF8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void SED() {
            SR.DecimalMode = true;
        }





        // SYSTEM

        [OpCode(Name = nameof(BRK), Code = 0x00, Length = 1, Cycles = 7, AddressingMode = AddressingMode.Implied)]
        public void BRK() {
            // http://www.thealmightyguru.com/Games/Hacking/Wiki/index.php?title=BRK

            SR.IrqDisable = true;

            PushStack((byte)((PC) >> 8)); // MSB
            PushStack((byte)((PC) & 0xFF)); // LSB
            PushStack(SR.Register);

            PC = BitConverter.ToUInt16(new byte[] { Memory[0xFFFE], Memory[0xFFFF] }, 0); // BRK interrupt vector
        }

        [OpCode(Name = nameof(NOP), Code = 0xEA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void NOP() {

        }


        // JUMP

        [OpCode(Name = nameof(JMP), Code = 0x4C, Length = 3, Cycles = 3, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(JMP), Code = 0x6C, Length = 3, Cycles = 5, AddressingMode = AddressingMode.Indirect)]
        public void JMP() {
            // http://6502.org/tutorials/6502opcodes.html#JMP

            PC = Address;
        }

        [OpCode(Name = nameof(JSR), Code = 0x20, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        public void JSR() {
            // http://6502.org/tutorials/6502opcodes.html#JSR

            PC--;

            PushStack((byte)((PC) >> 8)); // MSB
            PushStack((byte)((PC) & 0xFF)); // LSB

            PC = Address;
        }

        [OpCode(Name = nameof(RTI), Code = 0x40, Length = 1, Cycles = 6, AddressingMode = AddressingMode.Implied)]
        public void RTI() {
            // From https://youtu.be/8XmxKPJDGU0?t=3413

            SR.Register = PopStack();

            SR.BreakCommand = false;
            SR.Reserved = false; // From YouTube, undocumented way to detect reset/IRQ?

            byte lowByte = PopStack();
            byte highByte = PopStack();

            PC = (ushort)(lowByte | (highByte << 8));
        }

        [OpCode(Name = nameof(RTS), Code = 0x60, Length = 1, Cycles = 6, AddressingMode = AddressingMode.Implied)]
        public void RTS() {
            // http://6502.org/tutorials/6502opcodes.html#RTS

            // RTS pulls the top two bytes off the stack (low byte first) and transfers program control to that address+1. It is used, as expected, to exit a subroutine invoked via JSR which pushed the address-1.
            // Pop Stack -> SR
            // Pop Stack -> PC

            byte lowByte = PopStack();
            byte highByte = PopStack();

            PC = (ushort)(lowByte | (highByte << 8));
            PC++;
        }


        // STACK

        [OpCode(Name = nameof(PHA), Code = 0x48, Length = 1, Cycles = 3, AddressingMode = AddressingMode.Implied)]
        public void PHA() {
            // http://6502.org/tutorials/6502opcodes.html#PHA

            PushStack(AR);
        }

        [OpCode(Name = nameof(PLA), Code = 0x68, Length = 1, Cycles = 4, AddressingMode = AddressingMode.Implied)]
        public void PLA() {
            // http://6502.org/tutorials/6502opcodes.html#PLA 

            AR = PopStack();
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCode(Name = nameof(PHP), Code = 0x08, Length = 1, Cycles = 3, AddressingMode = AddressingMode.Implied)]
        public void PHP() {
            // http://6502.org/tutorials/6502opcodes.html#PHP

            PushStack(SR.Register);
        }

        [OpCode(Name = nameof(PLP), Code = 0x28, Length = 1, Cycles = 4, AddressingMode = AddressingMode.Implied)]
        public void PLP() {
            // http://6502.org/tutorials/6502opcodes.html#PLP 

            SR.Register = PopStack();
        }



        // BRANCHING

        [OpCode(Name = nameof(BCC), Code = 0x90, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BCC() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BCC

            if (!SR.Carry) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BCS), Code = 0xB0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BCS() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BCS

            if (SR.Carry) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BEQ), Code = 0xF0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BEQ() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BEQ

            if (SR.Zero) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BNE), Code = 0xD0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BNE() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BNE

            if (!SR.Zero) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BMI), Code = 0x30, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BMI() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BMI

            if (SR.Negative) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BPL), Code = 0x10, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BPL() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BPL

            if (!SR.Negative) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BVC), Code = 0x50, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BVC() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BVC

            if (!SR.Overflow) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BVS), Code = 0x70, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BVS() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BVS

            if (SR.Overflow) {
                // Branch
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }



        // COMPARE

        [OpCode(Name = nameof(CMP), Code = 0xC9, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(CMP), Code = 0xC5, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(CMP), Code = 0xD5, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(CMP), Code = 0xCD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(CMP), Code = 0xDD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(CMP), Code = 0xD9, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        [OpCode(Name = nameof(CMP), Code = 0xC1, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect)]
        [OpCode(Name = nameof(CMP), Code = 0xD1, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, AddCycleIfBoundaryCrossed = true)]
        public void CMP() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#CMP

            throw new NotImplementedException();
        }

        [OpCode(Name = nameof(CPX), Code = 0xE0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(CPX), Code = 0xE4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(CPX), Code = 0xEC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void CPX() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#CPX

            throw new NotImplementedException();
        }

        [OpCode(Name = nameof(CPY), Code = 0xC0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(CPY), Code = 0xC4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(CPY), Code = 0xCC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void CPY() {
            // https://www.masswerk.at/6502/6502_instruction_set.html#CPY

            throw new NotImplementedException();
        }
    }
}
