	LD A,0FFH  ;initial value in register A
	LD BC,0FF00H  ;initial value in register pair BC
L1:	LD (BC),A  ;load value in A to the memory location addressed by BC
	INC BC  ;increment BC
	DEC A  ;decrement A
	JP NZ,L1  ;loop until value in A is zero
	LD (BC),A  ;load value 00H to memory location FFFFH
	HALT  ;halt cpu
	.END
