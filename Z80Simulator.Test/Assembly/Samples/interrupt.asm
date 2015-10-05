	JP 0100H  ;jump to main routine

	.ORG 0038H  ;interrupt routine
	IN A,(01H)  ;get the value from port 01H
	OUT (02H),A  ;echo that value to port 02H
	EI  ;enable interrupts
	RETI  ;return from interrupt

	.ORG 0100H  ;main rountine
	JR L1  ;jump over data area
L2:	.DB 0AH  ;data byte 1
	.DB 0BH  ;data byte 2
	.DB 0CH  ;data byte 3
	.DB 0DH  ;data byte 4
	.DB 0EH  ;data byte 5
L1:	LD D,05H  ;load counter register D
	LD BC,L2  ;load pointer register pair BC
L3:	LD A,(BC)  ;get the data byte
	OUT (02H),A  ;send it to port 02H
	INC BC  ;increment pointer BC
	DEC D  ;decrement counter D
	JP NZ,L3  ;loop until all data bytes are sent
	IM 1  ;set interrupt mode 1
	EI  ;enable interrupts
L4:	JP L4  ;loop forever
	.END
