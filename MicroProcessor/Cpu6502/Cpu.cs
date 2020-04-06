using Extensions.Byte;
using Extensions.Enums;
using Hardware.Memory;
using MicroProcessor.Cpu6502.Attributes;
using MicroProcessor.Cpu6502.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroProcessor.Cpu6502 {
    public class Cpu {

        public event EventHandler<OpCode> OnStep;

        public const ushort NMI_VECTOR_ADDRESS = 0xFFFA;
        public const ushort RESET_VECTOR_ADDRESS = 0xFFFC;
        public const ushort IRQ_VECTOR_ADDRESS = 0xFFFE;
        public const ushort BRK_VECTOR_ADDRESS = 0xFFFE;


        /// <summary>
        /// Keeps track of the remaining cycles to finish the current instruction.
        /// Gets decremented on every CPU cycle.
        /// </summary>
        private int _cyclesRemainingCurrentInstruction = 0;

        private TaskCompletionSource<bool> _tcsPause;
        private bool _isPaused = false;
        private bool _pausedWaiting = false;

        private bool interruptWaiting = false;
        private bool nonMaskableInterruptWaiting = false;

        public List<OpCode> OpCodes { get; private set; }
        public Dictionary<byte, OpCode> OpCodeCache { get; set; }

        /// <summary>
        /// The total number of executed instructions.
        /// </summary>
        public ulong TotalInstructions { get; private set; }

        /// <summary>
        /// The total number of executed illegal instructions.
        /// </summary>
        public ulong TotalIllegalInstructions { get; set; }

        /// <summary>
        /// The total number of times the CPU has been cycled.
        /// </summary>
        public ulong TotalCycles { get; set; }

        /// <summary>
        /// The memory available for the CPU.
        /// The CPU uses the PC to fetch its instructions from the memory.
        /// </summary>
        public IMemory<byte> Memory { get; private set; }

        /// <summary>
        /// The Program Counter.
        /// Points to the current instruction or its operands.
        /// </summary>
        public ushort PC;

        /// <summary>
        /// Stack Pointer. The SP starts at 0xFF and is decremented on push.
        /// The stack stores its values in address range 0x01FF - 0x0100.
        /// </summary>
        public byte SP;

        /// <summary>
        /// Accumulator register.
        /// </summary>
        public byte AR;

        /// <summary>
        /// X register.
        /// </summary>
        public byte XR;

        /// <summary>
        /// Y register.
        /// </summary>
        public byte YR;

        /// <summary>
        /// Status Register. This register contains a set of flags.
        /// </summary>
        public StatusRegister SR = new StatusRegister() {
            Reserved = true
        };

        public OpCode OpCode { get; set; }
        public ushort OpCodeAddress;

        /// <summary>
        /// The current Address after applying the current addressing mode.
        /// </summary>
        public ushort Address;

        /// <summary>
        /// The current Value to operate on based on the Address or the Accumulator,
        /// depending on the current addressing mode.
        /// </summary>
        public byte Value {
            get {
                return OpCode.AddressingMode == AddressingMode.Accumulator ? AR : Memory.Read(Address);
            }
            set {
                if (OpCode.AddressingMode == AddressingMode.Accumulator) {
                    AR = value;

                } else {
                    Memory.Write(Address, value);
                }
            }
        }

        /// <summary>
        /// The MOS6502 CPU.
        /// </summary>
        /// <param name="memory"></param>
        public Cpu(IMemory<byte> memory) {
            Memory = memory;

            var opCodesMethods = GetType()
                .GetMethods()
                .SelectMany(m => m.GetCustomAttributes(typeof(OpCodeDefinitionAttribute), true)
                .Select(a => new {
                    Attribute = a as OpCodeDefinitionAttribute,
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
                .Select(x => OpCode.FromOpCodeDefinitionAttribute(() => x.Method.Invoke(this, null), () => addressingMethods[x.Attribute.AddressingMode].Method.Invoke(this, null), x.Attribute))
                .ToList();

            OpCodeCache = OpCodes.ToDictionary(x => x.Code, x => x);

            Reset();
        }


        /// <summary>
        /// Pauses the CPU cycling after finishing all cycles for the current instruction.
        /// </summary>
        /// <returns></returns>
        public Task<bool> Pause() {
            if (_pausedWaiting) return Task.FromResult(false);

            _pausedWaiting = true;

            _tcsPause = new TaskCompletionSource<bool>();
            return _tcsPause.Task;
        }

        /// <summary>
        /// Resumes the CPU cycling.
        /// </summary>
        public void Resume() {
            if (_pausedWaiting) return;

            _isPaused = false;
        }

        /// <summary>
        /// Cycles the CPU and makes sure to take up as many cycles as
        /// the current instruction is supposed to.
        /// </summary>
        public void Cycle() {
            if (_isPaused) return;

            DoCycle();
        }

        private void DoCycle() {
            if (_cyclesRemainingCurrentInstruction == 0) {

                if (_pausedWaiting) {
                    _pausedWaiting = false;
                    _isPaused = true;

                    _tcsPause.SetResult(true);
                    return;
                }

                if (nonMaskableInterruptWaiting) {
                    DoNonMaskableInterrupt();

                } else if (interruptWaiting) {
                    DoInterrupt();

                } else {
                    Step();
                }

            } else {
                _cyclesRemainingCurrentInstruction--;
            }
        }

        /// <summary>
        /// Steps the CPU by executing one instruction.
        /// </summary>
        /// <param name="ignoreCycles"></param>
        public void Step(bool ignoreCycles = false) {
            
            OpCode = OpCodeCache[Memory.Read(PC)];
            OpCode.OpCodeAddress = PC;

            if (OnStep != null) {
                var o = OpCode.FromOpCodeDefinitionAttribute(null, null, OpCode);
                o.OpCodeAddress = PC;

                o.Operands.Clear();
                for (int j = 1; j < o.Length; j++) {
                    o.Operands.Add(Memory.Read(PC + j));
                }

                OnStep?.Invoke(this, o);
            }

            PC++;

            if (OpCode.AddressingMode != AddressingMode.Implied && OpCode.AddressingMode != AddressingMode.Accumulator) {
                OpCode.GetAddress();
            }
            OpCode.Run();

            TotalInstructions++;
            if (OpCode.IsIllegal) TotalIllegalInstructions++;

            // Count total cycles
            // This doesn't account for extra cycles caused by memory operations crossing pages
            TotalCycles += OpCode.Cycles;

            // Keeps track of needed cycles to complete the current instruction
            // ignoreCycles = true is used to step the CPU with a debugger
            if (!ignoreCycles) _cyclesRemainingCurrentInstruction += OpCode.Cycles;
        }

        public void Reset() {
            AR = XR = YR = 0x00;

            // SP should be 0xFD, I used to initialize it directly, but `-= 3` on a `byte` becomes 0xFD after roll over
            // I'm not sure if this is correct on multiple resets yet
            //SP -= 3;
            SP = 0xFD;

            SR.IrqDisable = true;

            PC = (ushort)((Memory.Read(RESET_VECTOR_ADDRESS + 1) << 8) | Memory.Read(RESET_VECTOR_ADDRESS));

            // Reset takes 8 cycles
            TotalCycles += 8;
        }

        /// <summary>
        /// Triggers a non maskable interrupt (NMI) before executing the next instruction.
        /// </summary>
        public void NonMaskableInterrupt() {
            nonMaskableInterruptWaiting = true;
        }

        /// <summary>
        /// Triggers an interrupt (IRQ) before executing the next instruction.
        /// </summary>
        public void Interrupt() {
            if (SR.IrqDisable) return;
            interruptWaiting = true;
        }

        public void DoNonMaskableInterrupt() {
            // From https://youtu.be/8XmxKPJDGU0?t=3362

            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0x00FF));

            PushStack((byte)((SR.Register | (byte)ProcessorStatusFlags.Reserved) & (byte)~ProcessorStatusFlags.BreakCommand));
            SR.IrqDisable = true;

            PC = (ushort)((Memory.Read(NMI_VECTOR_ADDRESS + 1) << 8) | Memory.Read(NMI_VECTOR_ADDRESS));

            // NMI takes 8 cycles
            TotalCycles += 8;
            _cyclesRemainingCurrentInstruction += 8;

            nonMaskableInterruptWaiting = false;
        }

        private void DoInterrupt() {
            // From https://youtu.be/8XmxKPJDGU0?t=3362

            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0x00FF));

            PushStack((byte)((SR.Register | (byte)ProcessorStatusFlags.Reserved) & (byte)~ProcessorStatusFlags.BreakCommand));

            SR.IrqDisable = true;

            PC = (ushort)((Memory.Read(IRQ_VECTOR_ADDRESS + 1) << 8) | Memory.Read(IRQ_VECTOR_ADDRESS));

            // IRQ takes 7 cycles
            TotalCycles += 7;
            _cyclesRemainingCurrentInstruction += 7;

            interruptWaiting = false;
        }

        /// <summary>
        /// Pushes a value to the Stack and decrements the Stack-pointer.
        /// </summary>
        /// <param name="value"></param>
        public void PushStack(byte value) {
            Memory.Write(0x0100 + SP, value);
            SP--;
        }

        /// <summary>
        /// Pops a value off the Stack and increments the Stack-pointer.
        /// </summary>
        /// <returns></returns>
        public byte PopStack() {
            SP++;
            var b = Memory.Read(0x0100 + SP);
            return b;
        }


        [AddressingMode(AddressingMode = AddressingMode.Absolute)]
        public void Absolute() {
            var lowAddressByte = Memory.Read(PC);
            PC++;
            var highAddressByte = Memory.Read(PC);
            PC++;

            Address = (ushort)((highAddressByte << 8) | lowAddressByte);
        }

        [AddressingMode(AddressingMode = AddressingMode.AbsoluteX)]
        public void AbsoluteX() {
            var lowAddressByte = Memory.Read(PC);
            PC++;
            var highAddressByte = Memory.Read(PC);
            PC++;

            Address = (ushort)((highAddressByte << 8) | lowAddressByte);
            Address += XR;
        }

        [AddressingMode(AddressingMode = AddressingMode.AbsoluteY)]
        public void AbsoluteY() {
            var lowAddressByte = Memory.Read(PC);
            PC++;
            var highAddressByte = Memory.Read(PC);
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
            var lowAddressPointer = Memory.Read(PC);
            PC++;
            var highAddressPointer = Memory.Read(PC);
            PC++;

            var addressPointer = (ushort)((highAddressPointer << 8) | lowAddressPointer);

            if (lowAddressPointer == 0xFF) {
                // This handles a hardware bug in the 6502
                Address = (ushort)(Memory.Read(addressPointer & 0xFF00) << 8 | Memory.Read(addressPointer));

            } else {
                Address = (ushort)(Memory.Read(addressPointer + 1) << 8 | Memory.Read(addressPointer));
            }
        }

        [AddressingMode(AddressingMode = AddressingMode.XIndirect)]
        public void XIndirect() {
            // The ushort type below is used intentionally to allow byte rollover check

            ushort pageZeroAddress = Memory.Read(PC);
            PC++;

            var lowAddress = Memory.Read((pageZeroAddress + XR) & 0x00FF);
            var highAddress = Memory.Read((pageZeroAddress + XR + 1) & 0x00FF);

            Address = (ushort)((highAddress << 8) | lowAddress);
        }

        [AddressingMode(AddressingMode = AddressingMode.IndirectY)]
        public void IndirectY() {
            // The ushort type below is used intentionally to allow byte rollover check

            ushort pageZeroAddress = Memory.Read(PC);
            PC++;

            var lowAddress = Memory.Read(pageZeroAddress & 0x00FF);
            var highAddress = Memory.Read((pageZeroAddress + 1) & 0x00FF);

            Address = (ushort)((highAddress << 8) | lowAddress);
            Address += YR;
        }

        [AddressingMode(AddressingMode = AddressingMode.Relative)]
        public void Relative() {
            sbyte rel_addr = (sbyte)Memory.Read(PC);
            PC++;
            Address = (ushort)(PC + rel_addr);
        }

        [AddressingMode(AddressingMode = AddressingMode.Zeropage)]
        public void Zeropage() {
            Address = Memory.Read(PC);
            PC++;
        }

        [AddressingMode(AddressingMode = AddressingMode.ZeropageX)]
        public void ZeropageX() {
            // The ushort type below is used intentionally to allow byte rollover check

            ushort a = (ushort)(Memory.Read(PC) + XR);
            Address = (a &= 0x00FF);
            PC++;
        }

        [AddressingMode(AddressingMode = AddressingMode.ZeropageY)]
        public void ZeropageY() {
            // The ushort type below is used intentionally to allow byte rollover check

            ushort a = (ushort)(Memory.Read(PC) + YR);
            Address = (a &= 0x00FF);
            PC++;
        }





        // STORAGE

        [OpCodeDefinition(Name = nameof(LDA), Code = 0xA9, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Load Accumulator")]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xA5, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Load Accumulator")]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xB5, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Load Accumulator")]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xAD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Load Accumulator")]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xBD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Load Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xB9, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Load Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xA1, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Load Accumulator")]
        [OpCodeDefinition(Name = nameof(LDA), Code = 0xB1, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Load Accumulator", AddCycleIfBoundaryCrossed = true)]
        public void LDA() {
            AR = Value;
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCodeDefinition(Name = nameof(LDX), Code = 0xA2, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Load X-register")]
        [OpCodeDefinition(Name = nameof(LDX), Code = 0xA6, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Load X-register")]
        [OpCodeDefinition(Name = nameof(LDX), Code = 0xB6, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageY, Description = "Load X-register")]
        [OpCodeDefinition(Name = nameof(LDX), Code = 0xAE, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Load X-register")]
        [OpCodeDefinition(Name = nameof(LDX), Code = 0xBE, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Load X-register", AddCycleIfBoundaryCrossed = true)]
        public void LDX() {
            XR = Value;
            SR.SetNegative(XR);
            SR.SetZero(XR);
        }

        [OpCodeDefinition(Name = nameof(LDY), Code = 0xA0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Load Y-register")]
        [OpCodeDefinition(Name = nameof(LDY), Code = 0xA4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Load Y-register")]
        [OpCodeDefinition(Name = nameof(LDY), Code = 0xB4, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Load Y-register")]
        [OpCodeDefinition(Name = nameof(LDY), Code = 0xAC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Load Y-register")]
        [OpCodeDefinition(Name = nameof(LDY), Code = 0xBC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Load Y-register", AddCycleIfBoundaryCrossed = true)]
        public void LDY() {
            YR = Value;
            SR.SetNegative(YR);
            SR.SetZero(YR);
        }

        [OpCodeDefinition(Name = nameof(STA), Code = 0x85, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Store Accumulator")]
        [OpCodeDefinition(Name = nameof(STA), Code = 0x95, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Store Accumulator")]
        [OpCodeDefinition(Name = nameof(STA), Code = 0x8D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Store Accumulator")]
        [OpCodeDefinition(Name = nameof(STA), Code = 0x9D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Store Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(STA), Code = 0x99, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Store Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(STA), Code = 0x81, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Store Accumulator")]
        [OpCodeDefinition(Name = nameof(STA), Code = 0x91, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Store Accumulator", AddCycleIfBoundaryCrossed = true)]
        public void STA() {
            Value = AR;
        }

        [OpCodeDefinition(Name = nameof(STX), Code = 0x86, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Store X-register")]
        [OpCodeDefinition(Name = nameof(STX), Code = 0x96, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageY, Description = "Store X-register")]
        [OpCodeDefinition(Name = nameof(STX), Code = 0x8E, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Store X-register")]
        public void STX() {
            Value = XR;
        }

        [OpCodeDefinition(Name = nameof(STY), Code = 0x84, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Store Y-register")]
        [OpCodeDefinition(Name = nameof(STY), Code = 0x94, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Store Y-register")]
        [OpCodeDefinition(Name = nameof(STY), Code = 0x8C, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Store Y-register")]
        public void STY() {
            Value = YR;
        }

        [OpCodeDefinition(Name = nameof(TAX), Code = 0xAA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Transfer A-register to X-register")]
        public void TAX() {
            SR.SetNegative(AR);
            SR.SetZero(AR);
            XR = AR;
        }

        [OpCodeDefinition(Name = nameof(TAY), Code = 0xA8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Transfer A-register to Y-register")]
        public void TAY() {
            SR.SetNegative(AR);
            SR.SetZero(AR);
            YR = AR;
        }

        [OpCodeDefinition(Name = nameof(TSX), Code = 0xBA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Transfer Stack-pointer to X-register")]
        public void TSX() {
            SR.SetNegative(SP);
            SR.SetZero(SP);
            XR = SP;
        }

        [OpCodeDefinition(Name = nameof(TXA), Code = 0x8A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Transfer X-register to A-register")]
        public void TXA() {
            SR.SetNegative(XR);
            SR.SetZero(XR);
            AR = XR;
        }

        [OpCodeDefinition(Name = nameof(TXS), Code = 0x9A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Transfer X-register to Stack-pointer")]
        public void TXS() {
            SP = XR;
        }

        [OpCodeDefinition(Name = nameof(TYA), Code = 0x98, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Transfer Y-register to A-register")]
        public void TYA() {
            SR.SetNegative(YR);
            SR.SetZero(YR);
            AR = YR;
        }



        // BITWISE

        [OpCodeDefinition(Name = nameof(AND), Code = 0x29, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Bitwise AND with Accumulator")]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x25, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Bitwise AND with Accumulator")]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x35, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Bitwise AND with Accumulator")]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x2D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Bitwise AND with Accumulator")]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x3D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Bitwise AND with Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x39, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Bitwise AND with Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x21, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Bitwise AND with Accumulator")]
        [OpCodeDefinition(Name = nameof(AND), Code = 0x31, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Bitwise AND with Accumulator", AddCycleIfBoundaryCrossed = true)]
        public void AND() {
            AR = (byte)(Value & AR);

            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCodeDefinition(Name = nameof(ASL), Code = 0x0A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator, Description = "Arithmetic Shift Left")]
        [OpCodeDefinition(Name = nameof(ASL), Code = 0x06, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "Arithmetic Shift Left")]
        [OpCodeDefinition(Name = nameof(ASL), Code = 0x16, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "Arithmetic Shift Left")]
        [OpCodeDefinition(Name = nameof(ASL), Code = 0x0E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Arithmetic Shift Left")]
        [OpCodeDefinition(Name = nameof(ASL), Code = 0x1E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "Arithmetic Shift Left")]
        public void ASL() {
            SR.Carry = Value.IsBitSet(BitFlag.BIT_7);
            Value <<= 1;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCodeDefinition(Name = nameof(BIT), Code = 0x24, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Bit Test")]
        [OpCodeDefinition(Name = nameof(BIT), Code = 0x2C, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Bit Test")]
        public void BIT() {
            SR.SetZero((byte)(Value & AR));
            SR.Negative = Value.IsBitSet((BitFlag)ProcessorStatusFlags.Negative);
            SR.Overflow = Value.IsBitSet((BitFlag)ProcessorStatusFlags.Overflow);
        }

        [OpCodeDefinition(Name = nameof(EOR), Code = 0x49, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Bitwise Exclusive OR")]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x45, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Bitwise Exclusive OR")]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x55, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Bitwise Exclusive OR")]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x4D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Bitwise Exclusive OR")]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x5D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Bitwise Exclusive OR", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x59, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Bitwise Exclusive OR", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x41, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Bitwise Exclusive OR")]
        [OpCodeDefinition(Name = nameof(EOR), Code = 0x51, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Bitwise Exclusive OR", AddCycleIfBoundaryCrossed = true)]
        public void EOR() {
            AR = (byte)(Value ^ AR);

            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCodeDefinition(Name = nameof(LSR), Code = 0x4A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator, Description = "Logical Shift Right")]
        [OpCodeDefinition(Name = nameof(LSR), Code = 0x46, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "Logical Shift Right")]
        [OpCodeDefinition(Name = nameof(LSR), Code = 0x56, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "Logical Shift Right")]
        [OpCodeDefinition(Name = nameof(LSR), Code = 0x4E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Logical Shift Right")]
        [OpCodeDefinition(Name = nameof(LSR), Code = 0x5E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "Logical Shift Right")]
        public void LSR() {
            SR.Carry = Value.IsBitSet(BitFlag.BIT_0);
            Value >>= 1;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCodeDefinition(Name = nameof(ORA), Code = 0x09, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Bitwise OR With Accumulator")]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x05, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Bitwise OR With Accumulator")]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x15, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Bitwise OR With Accumulator")]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x0D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Bitwise OR With Accumulator")]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x1D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Bitwise OR With Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x19, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Bitwise OR With Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x01, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Bitwise OR With Accumulator")]
        [OpCodeDefinition(Name = nameof(ORA), Code = 0x11, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Bitwise OR With Accumulator", AddCycleIfBoundaryCrossed = true)]
        public void ORA() {
            AR = (byte)(Value | AR);
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCodeDefinition(Name = nameof(ROL), Code = 0x2A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator, Description = "Rotate Left")]
        [OpCodeDefinition(Name = nameof(ROL), Code = 0x26, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "Rotate Left")]
        [OpCodeDefinition(Name = nameof(ROL), Code = 0x36, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "Rotate Left")]
        [OpCodeDefinition(Name = nameof(ROL), Code = 0x2E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Rotate Left")]
        [OpCodeDefinition(Name = nameof(ROL), Code = 0x3E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "Rotate Left")]
        public void ROL() {
            var c = SR.Carry;
            SR.Carry = Value.IsBitSet(BitFlag.BIT_7);
            Value <<= 1;
            Value = Value.SetBit(BitFlag.BIT_0, c);

            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCodeDefinition(Name = nameof(ROR), Code = 0x6A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Accumulator, Description = "Rotate Right")]
        [OpCodeDefinition(Name = nameof(ROR), Code = 0x66, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "Rotate Right")]
        [OpCodeDefinition(Name = nameof(ROR), Code = 0x76, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "Rotate Right")]
        [OpCodeDefinition(Name = nameof(ROR), Code = 0x6E, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Rotate Right")]
        [OpCodeDefinition(Name = nameof(ROR), Code = 0x7E, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "Rotate Right")]
        public void ROR() {
            var c = SR.Carry;
            SR.Carry = Value.IsBitSet(BitFlag.BIT_0);
            Value >>= 1;
            Value = Value.SetBit(BitFlag.BIT_7, c);

            SR.SetNegative(Value);
            SR.SetZero(Value);
        }


        // MATH

        [OpCodeDefinition(Name = nameof(ADC), Code = 0x69, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Add With Carry")]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x65, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Add With Carry")]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x75, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Add With Carry")]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x6D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Add With Carry")]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x7D, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Add With Carry", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x79, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Add With Carry", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x61, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Add With Carry")]
        [OpCodeDefinition(Name = nameof(ADC), Code = 0x71, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Add With Carry", AddCycleIfBoundaryCrossed = true)]
        public void ADC() {
            var temp = AR + Value + (SR.Carry ? 1 : 0);

            SR.Carry = temp > 255;
            SR.Overflow = ((~(AR ^ Value) & (AR ^ temp)) & 0b10000000) == 0b10000000;

            SR.SetZero((byte)temp);
            SR.SetNegative((byte)temp);

            AR = (byte)temp;
        }

        [OpCodeDefinition(Name = nameof(SBC), Code = 0xE9, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Subtract With Carry")]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xE5, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Subtract With Carry")]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xF5, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Subtract With Carry")]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xED, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Subtract With Carry")]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xFD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Subtract With Carry", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xF9, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Subtract With Carry", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xE1, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Subtract With Carry")]
        [OpCodeDefinition(Name = nameof(SBC), Code = 0xF1, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Subtract With Carry", AddCycleIfBoundaryCrossed = true)]
        public void SBC() {
            var v = Value ^ 0x00FF;
            var temp = AR + v + (SR.Carry ? 1 : 0);

            SR.Carry = temp > 255;
            SR.Overflow = ((~(AR ^ v) & (AR ^ temp)) & 0b10000000) == 0b10000000;

            SR.SetZero((byte)temp);
            SR.SetNegative((byte)temp);

            AR = (byte)temp;
        }


        [OpCodeDefinition(Name = nameof(DEC), Code = 0xC6, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "Decrement Memory")]
        [OpCodeDefinition(Name = nameof(DEC), Code = 0xD6, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "Decrement Memory")]
        [OpCodeDefinition(Name = nameof(DEC), Code = 0xCE, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Decrement Memory")]
        [OpCodeDefinition(Name = nameof(DEC), Code = 0xDE, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "Decrement Memory")]
        public void DEC() {
            Value--;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCodeDefinition(Name = nameof(DEX), Code = 0xCA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Decrement X-register")]
        public void DEX() {
            XR--;
            SR.SetNegative(XR);
            SR.SetZero(XR);
        }

        [OpCodeDefinition(Name = nameof(DEY), Code = 0x88, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Decrement Y-register")]
        public void DEY() {
            YR--;
            SR.SetNegative(YR);
            SR.SetZero(YR);
        }

        [OpCodeDefinition(Name = nameof(INC), Code = 0xE6, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "Increment Memory")]
        [OpCodeDefinition(Name = nameof(INC), Code = 0xF6, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "Increment Memory")]
        [OpCodeDefinition(Name = nameof(INC), Code = 0xEE, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Increment Memory")]
        [OpCodeDefinition(Name = nameof(INC), Code = 0xFE, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "Increment Memory")]
        public void INC() {
            Value++;
            SR.SetNegative(Value);
            SR.SetZero(Value);
        }

        [OpCodeDefinition(Name = nameof(INX), Code = 0xE8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Increment X-register")]
        public void INX() {
            XR++;
            SR.SetNegative(XR);
            SR.SetZero(XR);
        }

        [OpCodeDefinition(Name = nameof(INY), Code = 0xC8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Increment Y-register")]
        public void INY() {
            YR++;
            SR.SetNegative(YR);
            SR.SetZero(YR);
        }




        // REGISTERS

        [OpCodeDefinition(Name = nameof(CLC), Code = 0x18, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Clear Carry")]
        public void CLC() {
            SR.Carry = false;
        }

        [OpCodeDefinition(Name = nameof(SEC), Code = 0x38, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Set Carry")]
        public void SEC() {
            SR.Carry = true;
        }

        [OpCodeDefinition(Name = nameof(CLI), Code = 0x58, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Clear Interrupt")]
        public void CLI() {
            SR.IrqDisable = false;
        }

        [OpCodeDefinition(Name = nameof(SEI), Code = 0x78, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Set Interrupt")]
        public void SEI() {
            SR.IrqDisable = true;
        }

        [OpCodeDefinition(Name = nameof(CLV), Code = 0xB8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Clear Overflow")]
        public void CLV() {
            SR.Overflow = false;
        }

        [OpCodeDefinition(Name = nameof(CLD), Code = 0xD8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Clear Decimal")]
        public void CLD() {
            SR.DecimalMode = false;
        }

        [OpCodeDefinition(Name = nameof(SED), Code = 0xF8, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "Set Decimal")]
        public void SED() {
            SR.DecimalMode = true;
        }





        // SYSTEM

        [OpCodeDefinition(Name = nameof(BRK), Code = 0x00, Length = 1, Cycles = 7, AddressingMode = AddressingMode.Implied, Description = "Break")]
        public void BRK() {
            PushStack((byte)((PC) >> 8)); // MSB
            PushStack((byte)((PC) & 0xFF)); // LSB
            PushStack((byte)(SR.Register | (byte)ProcessorStatusFlags.BreakCommand));

            SR.IrqDisable = true;

            PC = (ushort)((Memory.Read(BRK_VECTOR_ADDRESS + 1) << 8) | Memory.Read(BRK_VECTOR_ADDRESS));
        }

        [OpCodeDefinition(Name = nameof(NOP), Code = 0xEA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        public void NOP() {

        }


        // JUMP

        [OpCodeDefinition(Name = nameof(JMP), Code = 0x4C, Length = 3, Cycles = 3, AddressingMode = AddressingMode.Absolute, Description = "Jump")]
        [OpCodeDefinition(Name = nameof(JMP), Code = 0x6C, Length = 3, Cycles = 5, AddressingMode = AddressingMode.Indirect, Description = "Jump")]
        public void JMP() {
            PC = Address;
        }

        [OpCodeDefinition(Name = nameof(JSR), Code = 0x20, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "Jump to Sub Routine")]
        public void JSR() {
            PC--;

            PushStack((byte)((PC) >> 8)); // MSB
            PushStack((byte)((PC) & 0xFF)); // LSB

            PC = Address;
        }

        [OpCodeDefinition(Name = nameof(RTI), Code = 0x40, Length = 1, Cycles = 6, AddressingMode = AddressingMode.Implied, Description = "Return from Interrupt")]
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

        [OpCodeDefinition(Name = nameof(RTS), Code = 0x60, Length = 1, Cycles = 6, AddressingMode = AddressingMode.Implied, Description = "Return from Sub Routine")]
        public void RTS() {
            byte lowByte = PopStack();
            byte highByte = PopStack();

            PC = (ushort)(lowByte | (highByte << 8));
            PC++;
        }


        // STACK

        [OpCodeDefinition(Name = nameof(PHA), Code = 0x48, Length = 1, Cycles = 3, AddressingMode = AddressingMode.Implied, Description = "Push Accumulator")]
        public void PHA() {
            PushStack(AR);
        }

        [OpCodeDefinition(Name = nameof(PLA), Code = 0x68, Length = 1, Cycles = 4, AddressingMode = AddressingMode.Implied, Description = "Pull Accumulator")]
        public void PLA() {
            AR = PopStack();
            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        [OpCodeDefinition(Name = nameof(PHP), Code = 0x08, Length = 1, Cycles = 3, AddressingMode = AddressingMode.Implied, Description = "Push Processor Status")]
        public void PHP() {
            PushStack((byte)(SR.Register | (byte)ProcessorStatusFlags.BreakCommand));
        }

        [OpCodeDefinition(Name = nameof(PLP), Code = 0x28, Length = 1, Cycles = 4, AddressingMode = AddressingMode.Implied, Description = "Pull Processor Status")]
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

        [OpCodeDefinition(Name = nameof(BCC), Code = 0x90, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Carry Clear", AddCycleIfBoundaryCrossed = true)]
        public void BCC() {
            if (!SR.Carry) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BCS), Code = 0xB0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Carry Set", AddCycleIfBoundaryCrossed = true)]
        public void BCS() {
            if (SR.Carry) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BEQ), Code = 0xF0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Equal", AddCycleIfBoundaryCrossed = true)]
        public void BEQ() {
            if (SR.Zero) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BNE), Code = 0xD0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Not Equal", AddCycleIfBoundaryCrossed = true)]
        public void BNE() {
            if (!SR.Zero) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BMI), Code = 0x30, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Minus", AddCycleIfBoundaryCrossed = true)]
        public void BMI() {
            if (SR.Negative) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BPL), Code = 0x10, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Plus", AddCycleIfBoundaryCrossed = true)]
        public void BPL() {
            if (!SR.Negative) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BVC), Code = 0x50, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Overflow Clear", AddCycleIfBoundaryCrossed = true)]
        public void BVC() {
            if (!SR.Overflow) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }

        [OpCodeDefinition(Name = nameof(BVS), Code = 0x70, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Relative, Description = "Branch on Overflow Set", AddCycleIfBoundaryCrossed = true)]
        public void BVS() {
            if (SR.Overflow) {
                PC = Address;

            } else {
                // Don't branch
                TotalCycles++;
                _cyclesRemainingCurrentInstruction++;
            }
        }



        // COMPARE

        [OpCodeDefinition(Name = nameof(CMP), Code = 0xC9, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Compare Accumulator")]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xC5, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Compare Accumulator")]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xD5, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "Compare Accumulator")]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xCD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Compare Accumulator")]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xDD, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteX, Description = "Compare Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xD9, Length = 3, Cycles = 4, AddressingMode = AddressingMode.AbsoluteY, Description = "Compare Accumulator", AddCycleIfBoundaryCrossed = true)]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xC1, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "Compare Accumulator")]
        [OpCodeDefinition(Name = nameof(CMP), Code = 0xD1, Length = 2, Cycles = 5, AddressingMode = AddressingMode.IndirectY, Description = "Compare Accumulator", AddCycleIfBoundaryCrossed = true)]
        public void CMP() {
            // The int type below is used intentionally to allow byte rollover check
            int r = AR - Value;

            SR.Carry = r >= 0;
            SR.Zero = r == 0;
            SR.Negative = ((r >> 7) & 1) == 1;
        }

        [OpCodeDefinition(Name = nameof(CPX), Code = 0xE0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Compare X-register")]
        [OpCodeDefinition(Name = nameof(CPX), Code = 0xE4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Compare X-register")]
        [OpCodeDefinition(Name = nameof(CPX), Code = 0xEC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Compare X-register")]
        public void CPX() {
            // The int type below is used intentionally to allow byte rollover check
            int r = XR - Value;

            SR.Carry = r >= 0;
            SR.Zero = r == 0;
            SR.Negative = ((r >> 7) & 1) == 1;
        }

        [OpCodeDefinition(Name = nameof(CPY), Code = 0xC0, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "Compare Y-register")]
        [OpCodeDefinition(Name = nameof(CPY), Code = 0xC4, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "Compare Y-register")]
        [OpCodeDefinition(Name = nameof(CPY), Code = 0xCC, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "Compare Y-register")]
        public void CPY() {
            // The int type below is used intentionally to allow byte rollover check
            int r = YR - Value;

            SR.Carry = r >= 0;
            SR.Zero = r == 0;
            SR.Negative = ((r >> 7) & 1) == 1;
        }



        // Illegal instructions
        // Illegal op codes reference: http://www.oxyron.de/html/opcodes02.html

        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x04, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x0C, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x14, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x1A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x1C, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x34, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x3A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x3C, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x44, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x54, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x5A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x5C, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x64, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x74, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x7A, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x7C, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x80, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x82, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0x89, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xC2, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xD4, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xDA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xDC, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xE2, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xF4, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageX, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xFA, Length = 1, Cycles = 2, AddressingMode = AddressingMode.Implied, Description = "No Operation")]
        [OpCodeDefinition(Name = nameof(_NOP), IsIllegal = true, Code = 0xFC, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteX, Description = "No Operation")]
        public void _NOP() {

        }

        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xA3, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xA7, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xAB, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "")]
        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xAF, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xB3, Length = 2, Cycles = 5, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xB7, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageY, Description = "")]
        [OpCodeDefinition(Name = nameof(_LAX), IsIllegal = true, Code = 0xBF, Length = 3, Cycles = 4, AddCycleIfBoundaryCrossed = true, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        public void _LAX() {
            LDA();
            LDX();
        }

        [OpCodeDefinition(Name = nameof(_SAX), IsIllegal = true, Code = 0x83, Length = 2, Cycles = 6, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_SAX), IsIllegal = true, Code = 0x87, Length = 2, Cycles = 3, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_SAX), IsIllegal = true, Code = 0x8F, Length = 3, Cycles = 4, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_SAX), IsIllegal = true, Code = 0x97, Length = 2, Cycles = 4, AddressingMode = AddressingMode.ZeropageY, Description = "")]
        public void _SAX() {
            Value = (byte)(AR & XR);
        }

        [OpCodeDefinition(Name = nameof(_SBC), IsIllegal = true, Code = 0xEB, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "")]
        public void _SBC() {
            SBC();
        }

        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xC3, Length = 2, Cycles = 8, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xC7, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xCF, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xD3, Length = 2, Cycles = 8, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xD7, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "")]
        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xDB, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        [OpCodeDefinition(Name = nameof(_DCP), IsIllegal = true, Code = 0xDF, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "")]
        public void _DCP() {
            DEC();
            CMP();
        }

        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xE3, Length = 2, Cycles = 8, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xE7, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xEF, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xF3, Length = 2, Cycles = 8, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xF7, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "")]
        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xFB, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        [OpCodeDefinition(Name = nameof(_ISB), IsIllegal = true, Code = 0xFF, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "")]
        public void _ISB() {
            INC();
            SBC();
        }

        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x03, Length = 2, Cycles = 8, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x07, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x0F, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x13, Length = 2, Cycles = 8, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x17, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "")]
        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x1B, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        [OpCodeDefinition(Name = nameof(_SLO), IsIllegal = true, Code = 0x1F, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "")]
        public void _SLO() {
            ASL();
            ORA();
        }

        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x23, Length = 2, Cycles = 8, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x27, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x2F, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x33, Length = 2, Cycles = 8, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x37, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "")]
        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x3B, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        [OpCodeDefinition(Name = nameof(_RLA), IsIllegal = true, Code = 0x3F, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "")]
        public void _RLA() {
            ROL();
            AND();
        }

        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x43, Length = 2, Cycles = 8, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x47, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x4F, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x53, Length = 2, Cycles = 8, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x57, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "")]
        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x5B, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        [OpCodeDefinition(Name = nameof(_SRE), IsIllegal = true, Code = 0x5F, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "")]
        public void _SRE() {
            LSR();
            EOR();
        }

        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x63, Length = 2, Cycles = 8, AddressingMode = AddressingMode.XIndirect, Description = "")]
        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x67, Length = 2, Cycles = 5, AddressingMode = AddressingMode.Zeropage, Description = "")]
        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x6F, Length = 3, Cycles = 6, AddressingMode = AddressingMode.Absolute, Description = "")]
        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x73, Length = 2, Cycles = 8, AddressingMode = AddressingMode.IndirectY, Description = "")]
        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x77, Length = 2, Cycles = 6, AddressingMode = AddressingMode.ZeropageX, Description = "")]
        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x7B, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteY, Description = "")]
        [OpCodeDefinition(Name = nameof(_RRA), IsIllegal = true, Code = 0x7F, Length = 3, Cycles = 7, AddressingMode = AddressingMode.AbsoluteX, Description = "")]
        public void _RRA() {
            ROR();
            ADC();
        }


        // This illegal op code is not covered by nestest.nes
        [OpCodeDefinition(Name = nameof(_ANC), IsIllegal = true, Code = 0x0B, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "")]
        [OpCodeDefinition(Name = nameof(_ANC), IsIllegal = true, Code = 0x2B, Length = 2, Cycles = 2, AddressingMode = AddressingMode.Immediate, Description = "")]
        public void _ANC() {
            AND();
            SR.Carry = Value.IsBitSet(BitFlag.BIT_7);
        }
    }
}
