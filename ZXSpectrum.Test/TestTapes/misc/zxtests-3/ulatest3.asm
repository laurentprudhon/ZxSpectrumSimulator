; ulatest3 by JB '07
; adapted to 128 '11

	org 49152

INIT	call INSTINT
	call FRAME_TIME
	call GETVAR
	db "IPORT",13
	ld hl, iPORTl
	ld (hl), c
	ld hl, iPORTh
	ld (hl), b
	call GETVAR
	db "CPORT",13
	ld hl, cPORTl
	ld (hl), c
	ld hl, cPORTh
	ld (hl), b
	ld bc, READP
	ld hl, READP_name
	call SETVAR
	ld bc, CONTP
	ld hl, CONTP_name
	call SETVAR
	ld bc,(FRAMET)
	ret

READP_name db "READP",13
CONTP_name db "CONTP",13

TVAR dw 0

			;  0T - start of interrupt signal
			; nnT - absolute time after instruction

READP:	; read port
	call GETVAR
	db "T",13
	ld (TVAR), bc
	call ALIGNINT	; 46T
	im 1		; 54T
	ld bc, (TVAR)	; 74T
	ld hl, -120	; 84T
	add hl, bc	; 95T
	ld b,h
	ld c,l		; 103T
	call DELAY	; TVAR-17T
iPORTh equ $+1
	ld a, 0FFh	; TVAR-10T
iPORTl equ $+1
	in a, (0FFh)	;+11-1
	ld b,0
	ld c,a
	ret

CONTP:	; contention test
	call GETVAR
	db "T",13
	ld (TVAR), bc
	ld bc, _test
	call _pjump
	im 1
	ld b,0
	ret

_pjump	push bc
	jp ALIGNINT

_test:			; 46T
	ld bc, (TVAR)	; 66T
	ld hl, -112	; 76T
	add hl, bc	; 87T
	ld b,h
	ld c,l		; 95T
	call DELAY	; TVAR-17T

cPORTh equ $+1
	ld a, 0FFh	; TVAR-10T
cPORTl equ $+1
	in a, (0FEh)	; TVAR+1T+ (in at 7)

;	ld bc, (TVAR)	; TVAR+21T
;	ld hl, 4267	; TVAR+31T
;	and a		; TVAR+35T
;	sbc hl, bc	; TVAR+50T
;	ld b,h
;	ld c,l		; TVAR+58T

	ld bc,(TVAR)	; TVAR+21
	ld hl,(FRAMET)	; TVAR+37
	and a		; TVAR+41
	sbc hl,bc	; TVAR+56
	ld bc,32656	; TVAR+66
	add hl,bc	; TVAR+77
	ld b,h
	ld c,l		; TVAR+85

	call DELAY	; 69861T+
	ld c, 1		; 69868T+
	nop
	nop
	nop
	nop
	nop		; 69888T+
	dec c
	halt

	rst 0

	include delay.asm
	include instint.asm
	include frametime.asm
	include alignint.asm
	include getvar.asm
	include setvar.asm
