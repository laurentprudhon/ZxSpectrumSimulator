
if 0
	org 40000

entry:	ld hl,dupa
	call PRINT

	ld hl,12345
	and a
	call PNUM
	ld a,13
	rst 16

	ld hl,0
	scf
	call PNUM
	ld a,13
	rst 16

	ld hl,34463
	scf
	call PNUM
	ld a,13
	rst 16

	xor a
	ld hl,1
	call PNUM
	ld a,13
	rst 16

	ret

dupa defb 'dupa wolowa: ',0

endif
PRINT:
	ld a,(hl)
	and a
	ret z
	rst 16
	inc hl
	jr PRINT

PRDEC:
	ld a,0
PRDEC_PX:
	ld de,10000
	call nc,_digit
	call c,_big
	ld de,1000
	call _digit
	ld de,100
	call _digit
	ld e,10
	call _digit
	ld a,'0'
	add a,l
	rst 16
	ret

_big	ld de,5536
	add hl,de
	ld de,10000
	ld a,'6'
	jr _c

_digit:
	sbc hl,de
	jr c,_p
	ld a,'0'
_l	inc a
_c	sbc hl,de
	jr nc,_l
_p	add hl,de
	and a
	ret z
	rst 16
	ld a,'0'
	ret

;end entry
