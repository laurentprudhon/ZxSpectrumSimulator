
; align interrupt
; returns exactly at 46T (0T = tstate at which INT test succeeded)

ALIGNINT:
	ld de, _align
	push de
	ld de, _try
	push de
	im 2
	halt
	rst 0

_try			; 46T+
	push de		; 57T+

	ld bc,32677	; 67T+
	call DELAY	; 32744T+
	ld bc,(FRAMET)	; 32764T+

	call DELAY	; 69884T+
	nop		; 69888T+
	pop de
	rst 0

_align			; 55T
	inc de
	halt
	rst 0
