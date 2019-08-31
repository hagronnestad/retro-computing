namespace MicroProcessor {
    public enum AddressingMode {
        Accumulator,    // Accumulator	 	        OPC A	 	    operand is AC (implied single byte instruction)
        Absolute,       // absolute	 	            OPC $LLHH	 	operand is address $HHLL *
        AbsoluteX,      // absolute, X-indexed	 	OPC $LLHH,X	 	operand is address; effective address is address incremented by X with carry **
        AbsoluteY,      // absolute, Y-indexed	 	OPC $LLHH,Y	 	operand is address; effective address is address incremented by Y with carry **
        Immediate,      // immediate	 	        OPC #$BB	 	operand is byte BB
        Implied,        // implied	 	            OPC	 	        operand implied
        Indirect,       // indirect	 	            OPC ($LLHH)	 	operand is address; effective address is contents of word at address: C.w($HHLL)
        XIndirect,      // X-indexed, indirect	 	OPC ($LL,X)	 	operand is zeropage address; effective address is word in (LL + X, LL + X + 1), inc. without carry: C.w($00LL + X)
        IndirectY,      // indirect, Y-indexed	 	OPC ($LL),Y	 	operand is zeropage address; effective address is word in (LL, LL + 1) incremented by Y with carry: C.w($00LL) + Y
        Relative,       // relative	 	            OPC $BB	 	    branch target is PC + signed offset BB ***
        Zeropage,       // zeropage	 	            OPC $LL	 	    operand is zeropage address (hi-byte is zero, address = $00LL)
        ZeropageX,      // zeropage, X-indexed	 	OPC $LL,X	 	operand is zeropage address; effective address is address incremented by X without carry **
        ZeropageY       // zeropage, Y-indexed	 	OPC $LL,Y	 	operand is zeropage address; effective address is address incremented by Y without carry **
    }
}