START:	LD SP,STACK  ;initialize stack pointer
	LD HL,BINBYT  ;point HL index to where binary number is stored
	LD A,(HL)  ;transfer byte
	LD HL,OUTBUF  ;point HL index to output-buffer memory
	CALL BINBCD
	HALT

BINBCD:
	LD B,100  ;load 100 into register B (power of ten holding register)
	CALL BCD  ;call conversion for BCD3
	LD B,10  ;load 10 into register B
	CALL BCD  ;call conversion for BCD2
	LD (HL),A  ;store BCD1
	RET

BCD:
	LD (HL),0FFH  ;load buffer with -1
STORE:	INC (HL)  ;clear buffer first and increment for each subtraction
	SUB B  ;subtract power of ten from binary number
	JR NC,STORE  ;if number is larger than power of ten, go back and add 1 to buffer
	ADD A,B  ;if no, add power of ten to get back remainder
	INC HL  ;go to next buffer location
	RET

	.ORG 0100H
BINBYT	.DB 234  ;example binary number to be converted into a BCD number
OUTBUF  ;output-buffer memory location

STACK	.EQU 0FFFFH  ;definition of stack pointer initialization address
	.END
