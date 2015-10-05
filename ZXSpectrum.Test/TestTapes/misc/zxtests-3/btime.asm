; border time
; by Jan Bobrowski

	org 49152

	call INSTINT
	call FRAME_TIME
	call GETVAR
	db "T",13
	ld hl,-(95+7)
	add hl,bc
	ld (T1),hl

	call ALIGNINT
NEXT:			; 46
	im 1		; 54

	ld bc, (T1)	; 74
	call DELAY	; 74+T1
	ld c, 0xFE	; 81+T1
	ld e, 07	; 88+T1
	ld a, 02	; 95+T1
	out (0xFE), a	; 106+T1
	out (c), e	; 118+T1

	ld hl, 0xF413	; 128+T1 XXX=62483=0xF413
	ld bc, (T1)	; 148+T1
	and a		; 152+T1
	sbc hl, bc	; 167+T1
	ld b,h
	ld c,l		; 175+T1
	call DELAY	; 175+XXX

	ld a,0		; 182+XXX
	in a,(0xFE)	; 193+XXX
	inc a		; 197+XXX
	and 1Fh		; 204+XXX
	ret nz		; 209+XXX
	im 2		; 217+XXX

	ld bc, NEXT	; 227+XXX
	push bc		; 238+XXX
	halt		; 238+XXX+4*n = 62721+4*n

	rst 0

	include delay.asm
	include instint.asm
	include frametime.asm
	include alignint.asm
	include getvar.asm

T1 dw 0
