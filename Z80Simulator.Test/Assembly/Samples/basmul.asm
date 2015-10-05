; Compiled with: Z80 Simulator IDE v9.70
;
; Begin
	LD IX,0FF00H
	LD SP,0FEFAH
;       The address of 'a' (integer) (global) is FEFEH (IX-2)
a:	EQU 0FEFEH
;       The address of 'b' (integer) (global) is FEFCH (IX-4)
b:	EQU 0FEFCH
;       The address of 'c' (integer) (global) is FEFAH (IX-6)
c:	EQU 0FEFAH
; Begin of program
; 1: Dim a As Integer
	LD A,00H
	LD (IX-02H),A
	LD (IX-01H),A
; 2: Dim b As Integer
	LD A,00H
	LD (IX-04H),A
	LD (IX-03H),A
; 3: Dim c As Integer
	LD A,00H
	LD (IX-06H),A
	LD (IX-05H),A
; 4: 
; 5: a = 123 'First number
	LD HL,007BH
	LD (IX-02H),L
	LD (IX-01H),H
; 6: b = 234 'Second number
	LD HL,00EAH
	LD (IX-04H),L
	LD (IX-03H),H
; 7: c = a * b
	LD L,(IX-02H)
	LD H,(IX-01H)
	LD E,(IX-04H)
	LD D,(IX-03H)
	CALL M001
	LD (IX-06H),L
	LD (IX-05H),H
; End of program
	HALT
; Integer Multiplication Routine
M001:	PUSH BC
	LD B,H
	LD C,L
	LD HL,0000H
	LD A,10H
M003:	DEC D
	INC D
	JR NZ,M002
	CP 09H
	JR C,M002
	SUB 08H
	PUSH AF
	LD D,E
	LD E,H
	LD H,L
	LD L,00H
	POP AF
	JR M003
M002:	ADD HL,HL
	RL E
	RL D
	JR NC,M004
	ADD HL,BC
M004:	DEC A
	JR NZ,M003
	POP BC
	RET
; End of listing
	.END
