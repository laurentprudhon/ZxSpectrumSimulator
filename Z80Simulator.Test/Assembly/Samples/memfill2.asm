L2:	IN A,(01H)  ;get value on port 01H to be used for memory fill
	LD D,0FFH  ;initial value in counter register D
	LD BC,0FF00H  ;initial value in pointer register pair BC
L1:	LD (BC),A  ;load value in A to the memory location addressed by BC
	INC BC  ;increment pointer BC
	DEC D  ;decrement counter D
	JP NZ,L1  ;loop until value in D is zero
	LD (BC),A  ;fill the last memory location FFFFH
	JP L2  ;repeat routine
	.END
