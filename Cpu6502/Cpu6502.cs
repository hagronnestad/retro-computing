using System;

namespace Cpu6502 {
    public class Cpu6502 {

        public byte[] Memory { get; set; } = new byte[0x10000];

        public ushort PC { get; set; }

        public byte SP { get; set; }

        public byte AR { get; set; }
        public byte XR { get; set; }
        public byte YR { get; set; }

        public StatusRegister SR { get; set; }




        // STORAGE

        public void LDA(byte operand) {
            SR.SetNegative(operand);
            SR.SetZero(operand);
            AR = operand;
        }
        public void LDX(byte operand) {
            SR.SetNegative(operand);
            SR.SetZero(operand);
            XR = operand;
        }
        public void LDY(byte operand) {
            SR.SetNegative(operand);
            SR.SetZero(operand);
            YR = operand;
        }
        public void STA(out byte operand) {
            operand = AR;
        }
        public void STX(out byte operand) {
            operand = XR;
        }
        public void STY(out byte operand) {
            operand = YR;
        }
        public void TAX() {
            SR.SetNegative(AR);
            SR.SetZero(AR);
            XR = AR;
        }
        public void TAY() {
            SR.SetNegative(AR);
            SR.SetZero(AR);
            YR = AR;
        }
        public void TSX() {
            SR.SetNegative(SP);
            SR.SetZero(SP);
            XR = SP;
        }
        public void TXA() {
            SR.SetNegative(XR);
            SR.SetZero(XR);
            AR = XR;
        }
        public void TXS() {
            SR.SetNegative(XR);
            SR.SetZero(XR);
            SP = XR;
        }
        public void TYA() {
            SR.SetNegative(YR);
            SR.SetZero(YR);
            AR = YR;
        }



        // BITWISE

        public void AND(byte operand) {
            var r = operand & AR;
            AR = (byte)r;

            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        public void ASL(byte operand) { }

        public void BIT(byte operand) {
            // https://www.masswerk.at/6502/6502_instruction_set.html#BIT
            // TODO: Needs verification!

            var r = AR & operand;

            SR.Negative = (operand & (byte)ProcessorStatusFlags.Negative) == (byte)ProcessorStatusFlags.Negative;
            SR.Overflow = (operand & (byte)ProcessorStatusFlags.Overflow) == (byte)ProcessorStatusFlags.Overflow;

            SR.SetZero((byte)r);
        }

        public void EOR(byte operand) {
            // https://www.masswerk.at/6502/6502_instruction_set.html#EOR
            // TODO: Needs verification!

            var r = operand ^ AR;
            AR = (byte)r;

            SR.SetNegative(AR);
            SR.SetZero(AR);
        }

        public void LSR(byte operand) {
        }

        public void ORA(byte operand) {
            var r = operand | AR;
            SR.SetNegative(operand);
            SR.SetZero(operand);
            AR = (byte)r;
        }

        public void ROL(byte operand) { }
        public void ROR(byte operand) { }





        // REGISTERS

        public void CLC() {
            SR.Carry = false;
        }

        public void SEC() {
            SR.Carry = true;
        }

        public void CLI() {
            SR.IrqDisable = false;
        }

        public void SEI() {
            SR.IrqDisable = true;
        }

        public void CLV() {
            SR.Overflow = false;
        }

        public void CLD() {
            SR.DecimalMode = false;
        }

        public void SED() {
            SR.DecimalMode = true;
        }





        // SYSTEM

        public void BRK(byte operand) {
            // http://www.thealmightyguru.com/Games/Hacking/Wiki/index.php?title=BRK

            PC += 2;
            // Push PC -> Stack
            // Push SR -> Stack
            SR.IrqDisable = true;
            PC = BitConverter.ToUInt16(Memory, 0xFFFE); // lsb first maybe because little-endian? 🤔

            throw new NotImplementedException();
        }

        public void NOP() {

        }

    }
}
