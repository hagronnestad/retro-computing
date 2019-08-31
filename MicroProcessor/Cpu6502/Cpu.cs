using Extensions.Byte;
using Extensions.Enums;
using MicroProcessor.Cpu6502.Attributes;
using MicroProcessor.Cpu6502.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroProcessor.Cpu6502 {
    public class Cpu {
        public List<OpCodeDefinition> OpCodes { get; private set; }
        public Dictionary<byte, OpCodeDefinition> OpCodeCache { get; set; }

        public int TotalInstructions = 0;

        public byte[] Memory = new byte[0x10000];

        public ushort PC;

        /// <summary>
        /// Stack Pointer. The SP starts at 0xFF and is decremented on push.
        /// The stack stores its values in address range 0x01FF - 0x0100.
        /// </summary>
        public byte SP;

        public byte AR;
        public byte XR;
        public byte YR;

        public StatusRegister SR = new StatusRegister() {
            Reserved = true
        };


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


        public Cpu() {

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
                })).ToDictionary(x => x.Attribute.AddressingMode, x => x);

            OpCodes = opCodesMethods
                .Select(x => OpCodeDefinition.FromOpCodeAttribute(() => x.Method.Invoke(this, null), () => addressingMethods[x.Attribute.AddressingMode].Method.Invoke(this, null), x.Attribute))
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

        public void Step() {
            TotalInstructions += 1;

            OpCode = OpCodeCache[Memory[PC]];
            OpCodeAddress = PC;
            PC++;

            //var opCodeBytes = string.Join(" ", Memory.Skip(OpCodeAddress).Take(OpCode.Length).Select(x => $"{x:X2}")).PadRight(8);
            //var logLine = $"{OpCodeAddress:X4}  {opCodeBytes}  {OpCode.Name}  A:{AR:X2} X:{XR:X2} Y:{YR:X2} P:{SR.Register:X2} SP:{SP:X2}\n";
            //File.AppendAllText("log.txt", logLine);

            if (OpCode.AddressingMode != AddressingMode.Implied && OpCode.AddressingMode != AddressingMode.Accumulator) {
                OpCode.GetAddress();
            }
            OpCode.Run();

            NextOpCode = OpCodeCache[Memory[PC]];
            NextOpCodeAddress = PC;
        }

        public void Reset() {
            AR = XR = YR = 0x00;

            // SP should be 0xFD, I used to initialize it directly, but `-= 3` on a `byte` becomes 0xFD after roll over
            // I'm not sure if this is correct on multiple resets yet
            SP -= 3;
            //SP = 0xFD;

            SR.IrqDisable = true;

            var resetVectorAddress = 0xFFFC;
            PC = (ushort)((Memory[resetVectorAddress + 1] << 8) | Memory[resetVectorAddress + 0]);

            // Reset takes 8 cycles
        }

        public void Interrupt() { // IRQ
            // From https://youtu.be/8XmxKPJDGU0?t=3362

            if (SR.IrqDisable) return;

            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0x00FF));

            PushStack((byte)((SR.Register | (byte)ProcessorStatusFlags.Reserved) & (byte)~ProcessorStatusFlags.BreakCommand));
            SR.IrqDisable = true;

            var irqVectorAddress = 0xFFFE;
            PC = (ushort)((Memory[irqVectorAddress + 1] << 8) | Memory[irqVectorAddress + 0]);

            // IRQ takes 7 cycles
        }

        public void NonMaskableInterrupt() { // NMI
            // From https://youtu.be/8XmxKPJDGU0?t=3362

            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0x00FF));

            PushStack((byte)((SR.Register | (byte)ProcessorStatusFlags.Reserved) & (byte)~ProcessorStatusFlags.BreakCommand));
            SR.IrqDisable = true;

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
            // The ushort type below is used intentionally to allow byte rollover check

            ushort pageZeroAddress = Memory[PC];
            PC++;

            var lowAddress = Memory[(pageZeroAddress + XR) & 0x00FF];
            var highAddress = Memory[(pageZeroAddress + XR + 1) & 0x00FF];

            Address = (ushort)((highAddress << 8) | lowAddress);
        }

        [AddressingMode(AddressingMode = AddressingMode.IndirectY)]
        public void IndirectY() {
            // The ushort type below is used intentionally to allow byte rollover check

            ushort pageZeroAddress = Memory[PC];
            PC++;

            var lowAddress = Memory[pageZeroAddress & 0x00FF];
            var highAddress = Memory[(pageZeroAddress + 1) & 0x00FF];

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
            // The ushort type below is used intentionally to allow byte rollover check

            ushort a = (ushort)(Memory[PC] + XR);
            Address = (a &= 0x00FF);
            PC++;
        }

        [AddressingMode(AddressingMode = AddressingMode.ZeropageY)]
        public void ZeropageY() {
            // The ushort type below is used intentionally to allow byte rollover check

            ushort a = (ushort)(Memory[PC] + YR);
            Address = (a &= 0x00FF);
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
            AR = Value;
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCode(Name = nameof(LDX), Code = 0xA2, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(LDX), Code = 0xA6, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(LDX), Code = 0xB6, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageY)]
        [OpCode(Name = nameof(LDX), Code = 0xAE, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(LDX), Code = 0xBE, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, AddCycleIfBoundaryCrossed = true)]
        public void LDX() {
            XR = Value;
            SR.SetNegative(XR);
            SR.SetZero(XR);
        }

        [OpCode(Name = nameof(LDY), Code = 0xA0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(LDY), Code = 0xA4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(LDY), Code = 0xB4, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX)]
        [OpCode(Name = nameof(LDY), Code = 0xAC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(LDY), Code = 0xBC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, AddCycleIfBoundaryCrossed = true)]
        public void LDY() {
            YR = Value;
            SR.SetNegative(YR);
            SR.SetZero(YR);
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
            SR.Carry = Value.IsBitSet(BitFlag.BIT_7);
            Value <<= 1;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCode(Name = nameof(BIT), Code = 0x24, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(BIT), Code = 0x2C, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void BIT() {
            SR.SetZero((byte)(Value & AR));
            SR.Negative = Value.IsBitSet((BitFlag)ProcessorStatusFlags.Negative);
            SR.Overflow = Value.IsBitSet((BitFlag)ProcessorStatusFlags.Overflow);
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
            var temp = AR + Value + (SR.Carry ? 1 : 0);

            SR.Carry = temp > 255;
            SR.Overflow = ((~(AR ^ Value) & (AR ^ temp)) & 0b10000000) == 0b10000000;

            SR.SetZero((byte)temp);
            SR.SetNegative((byte)temp);

            AR = (byte)temp;
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
            var v = Value ^ 0x00FF;
            var temp = AR + v + (SR.Carry ? 1 : 0);

            SR.Carry = temp > 255;
            SR.Overflow = ((~(AR ^ v) & (AR ^ temp)) & 0b10000000) == 0b10000000;

            SR.SetZero((byte)temp);
            SR.SetNegative((byte)temp);

            AR = (byte)temp;
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
            PushStack((byte)((PC) >> 8)); // MSB
            PushStack((byte)((PC) & 0xFF)); // LSB
            PushStack((byte)(SR.Register | (byte)ProcessorStatusFlags.BreakCommand));

            SR.IrqDisable = true;

            PC = BitConverter.ToUInt16(new byte[] { Memory[0xFFFE], Memory[0xFFFF] }, 0); // BRK interrupt vector
        }

        [OpCode(Name = nameof(NOP), Code = 0xEA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied)]
        public void NOP() {

        }


        // JUMP

        [OpCode(Name = nameof(JMP), Code = 0x4C, Length = 3, Cycles = 3, AddressingMode = AddressingMode.Absolute)]
        [OpCode(Name = nameof(JMP), Code = 0x6C, Length = 3, Cycles = 5, AddressingMode = AddressingMode.Indirect)]
        public void JMP() {
            PC = Address;
        }

        [OpCode(Name = nameof(JSR), Code = 0x20, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute)]
        public void JSR() {
            PC--;

            PushStack((byte)((PC) >> 8)); // MSB
            PushStack((byte)((PC) & 0xFF)); // LSB

            PC = Address;
        }

        [OpCode(Name = nameof(RTI), Code = 0x40, Length = 1, Cycles = 6, AddressingMode = AddressingMode.Implied)]
        public void RTI() {
            var poppedSR = new StatusRegister() { Register = PopStack() };

            SR.Carry = poppedSR.Carry;
            SR.Zero = poppedSR.Zero;
            SR.IrqDisable = poppedSR.IrqDisable;
            SR.DecimalMode = poppedSR.DecimalMode;
            // This instruction ignores DecimalMode and Break
            SR.Overflow = poppedSR.Overflow;
            SR.Negative = poppedSR.Negative;

            byte lowByte = PopStack();
            byte highByte = PopStack();

            PC = (ushort)(lowByte | (highByte << 8));
        }

        [OpCode(Name = nameof(RTS), Code = 0x60, Length = 1, Cycles = 6, AddressingMode = AddressingMode.Implied)]
        public void RTS() {
            byte lowByte = PopStack();
            byte highByte = PopStack();

            PC = (ushort)(lowByte | (highByte << 8));
            PC++;
        }


        // STACK

        [OpCode(Name = nameof(PHA), Code = 0x48, Length = 1, Cycles = 3, AddressingMode = AddressingMode.Implied)]
        public void PHA() {
            PushStack(AR);
        }

        [OpCode(Name = nameof(PLA), Code = 0x68, Length = 1, Cycles = 4, AddressingMode = AddressingMode.Implied)]
        public void PLA() {
            AR = PopStack();
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCode(Name = nameof(PHP), Code = 0x08, Length = 1, Cycles = 3, AddressingMode = AddressingMode.Implied)]
        public void PHP() {
            PushStack((byte)(SR.Register | (byte)ProcessorStatusFlags.BreakCommand));
        }

        [OpCode(Name = nameof(PLP), Code = 0x28, Length = 1, Cycles = 4, AddressingMode = AddressingMode.Implied)]
        public void PLP() {
            var poppedSR = new StatusRegister() { Register = PopStack() };

            SR.Carry = poppedSR.Carry;
            SR.Zero = poppedSR.Zero;
            SR.IrqDisable = poppedSR.IrqDisable;
            SR.DecimalMode = poppedSR.DecimalMode;
            // This instruction ignores DecimalMode and Break
            SR.Overflow = poppedSR.Overflow;
            SR.Negative = poppedSR.Negative;
        }



        // BRANCHING

        [OpCode(Name = nameof(BCC), Code = 0x90, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BCC() {
            if (!SR.Carry) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BCS), Code = 0xB0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BCS() {
            if (SR.Carry) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BEQ), Code = 0xF0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BEQ() {
            if (SR.Zero) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BNE), Code = 0xD0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BNE() {
            if (!SR.Zero) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BMI), Code = 0x30, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BMI() {
            if (SR.Negative) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BPL), Code = 0x10, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BPL() {
            if (!SR.Negative) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BVC), Code = 0x50, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BVC() {
            if (!SR.Overflow) {
                PC = Address;

            } else {
                // Don't branch
                // Increment cycle count
            }
        }

        [OpCode(Name = nameof(BVS), Code = 0x70, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, AddCycleIfBoundaryCrossed = true)]
        public void BVS() {
            if (SR.Overflow) {
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
            // The int type below is used intentionally to allow byte rollover check
            int r = AR - Value;

            SR.Carry = r >= 0;
            SR.Zero = r == 0;
            SR.Negative = ((r >> 7) & 1) == 1;
        }

        [OpCode(Name = nameof(CPX), Code = 0xE0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(CPX), Code = 0xE4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(CPX), Code = 0xEC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void CPX() {
            // The int type below is used intentionally to allow byte rollover check
            int r = XR - Value;

            SR.Carry = r >= 0;
            SR.Zero = r == 0;
            SR.Negative = ((r >> 7) & 1) == 1;
        }

        [OpCode(Name = nameof(CPY), Code = 0xC0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate)]
        [OpCode(Name = nameof(CPY), Code = 0xC4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage)]
        [OpCode(Name = nameof(CPY), Code = 0xCC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute)]
        public void CPY() {
            // The int type below is used intentionally to allow byte rollover check
            int r = YR - Value;

            SR.Carry = r >= 0;
            SR.Zero = r == 0;
            SR.Negative = ((r >> 7) & 1) == 1;
        }
    }
}