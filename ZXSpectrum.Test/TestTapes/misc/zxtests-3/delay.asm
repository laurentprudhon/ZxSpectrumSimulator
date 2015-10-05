; Z80 delay routine
; by Jan Bobrowski, license GPL, LGPL

DELAY:	; wait bc T (including call; bc>=141)
	; destroys: af, bc, hl

	ld hl, -141
	add hl, bc
	ld bc, -23
_loop	add hl, bc
	jr c, _loop
	ld a, l
	add a, 15
	jr nc, _g0
	cp 8
	jr c, _g1
	or 0
_g0	inc hl
_g1	rra
	jr c, _b0
	nop
_b0	rra
	jr nc, _b1
	or 0
_b1	rra
	ret nc
	ret
