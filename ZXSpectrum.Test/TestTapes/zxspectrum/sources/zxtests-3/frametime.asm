
FRAME_TIME:
	ld bc, _end
	push bc
	ld bc, _stage3
	push bc
	push bc
	push bc
	push bc
	ld bc, _stage2
	push bc
	ld bc, _stage1
	push bc
	ld de, 0
	ld hl, 0

	halt
	im 2
	halt
	rst 0

_stage1			; 46..49
_loop	inc hl
	jp _loop

_stage2
	add hl, hl
	add hl, hl
	add hl, hl
	add hl, hl
	ld bc, 46-16-32768
	add hl, bc
	ld b,h
	ld c,l
	halt
	rst 0

_stage3			; 46..49
	push bc
	call DELAY
	ld bc, 32768-45-31
	call DELAY
	pop bc
	inc e
	inc e
	inc e
	inc e
	inc e
	inc e
	inc e
	rst 0

_end	im 1
	ex de,hl
	add hl, bc
	ld (FRAMET),hl
	ret

FRAMET dw 0 ; frame time - 32768
