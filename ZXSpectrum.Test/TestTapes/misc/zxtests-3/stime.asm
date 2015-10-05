; screen time
; by Jan Bobrowski

	org 49152

	call INSTINT
	call FRAME_TIME
	call GETVAR
	db "T",13
	ld hl,-106
	add hl,bc
	ld (T1),hl
	call TEST
	xor a
	ld (de),a
	ret

TEST:
	call ALIGNINT
NEXT:			; 46
	im 1		; 55

	ld de,16384	; 65
	xor a		; 68
	ld (de),a	; 75
	ld bc, (T1)	; 95
	call DELAY	; 95+T1
	ld a,0xFF	; 102+T1
	ld (de),a	; 109+T1 write starts at 106+T1
	ld hl, 0xF41F	; 119+T1 XXX=62495=0xF41F
	ld bc, (T1)	; 139+T1
	and a		; 143+T1
	sbc hl, bc	; 158+T1
	ld b,h
	ld c,l		; 166+T1
	call DELAY	; 166+XXX

	xor a		; 170+XXX
	in a,(0xFE)	; 181+XXX
	inc a		; 185+XXX
	and 1Fh		; 192+XXX
	ret nz		; 197+XXX
	im 2		; 205+XXX

	ld bc, NEXT	; 215+XXX
	push bc		; 226+XXX
	halt		; 226+XXX+4*n = 62721+4*n

	rst 0

	include delay.asm
	include instint.asm
	include frametime.asm
	include alignint.asm
	include getvar.asm

T1 dw 0
