
	org 49152

entry:
	ld a,2
	call SYS_CHAN

	call INSTINT
	call FRAME_TIME

	call p_ftime
	call p_eipx
	call p_itime

	ret

p_ftime
	ld hl,t_ftime
	call PRINT
	ld de,32768
	ld hl,(FRAMET)
	add hl,de
	ld de,frame_ok
	ld c,2
	jr num_ok_nl

p_eipx
	ld hl,t_eipx
	call PRINT
	call EI_PREFIX
	cp 1
	ld hl,t_no
	jr c,str_ok_nl
	ld hl,t_yes
	jr z,str_ok_nl
	stc
	ld hl,t_fail
	jr str_ok_nl

p_itime
	ld hl,t_itime
	call PRINT
	call INT_TIME
	ld hl,t_fail
	jr c,str_ok_nl
	ex de,hl
	ld de,int_ok
	ld c,1
	jr num_ok_nl

oneof	and a
_l	ex de,hl
	ld c,(hl)
	inc hl
	ld b,(hl)
	inc hl
	ex de,hl
	dec a
	sbc hl,bc
	add hl,bc
	ret z ; nc
	cp 1
	jr nc,_l
	ret ; nz

str_ok_nl
	call color
	call PRINT
	jr inknl

num_ok_nl
	push af
	call oneof
	call color
	pop af
	call PRDEC

inknl	call SYS_TEMPS
	ld a,23
	rst 16
	ld a,32
	rst 16
	ld a,13
	rst 16
	ret

color:
	ret nc
	ld c,2
	ld a,16
	rst 16
	ld a,c
	rst 16
	ret


EI_PREFIX:
	ld hl,_int
	push hl
	call ALIGNINT
	di			; 4
	ld bc,32768-46-42+1	; 14
	call DELAY		; n+14
	ld bc,(FRAMET)		; n+34
	call DELAY		; n+ft+34
	xor a			; n+ft+38
	ei			; n+ft+42
	inc a
	di
	inc a
	ret

_int	im 1
	ei
	ret


INT_TIME:

_start equ 48
_instw equ 12

	ld hl,_int
	push hl
	call ALIGNINT

	ld bc,32768-46-20-24+_start-_instw*4
			; 56
	ld de,_start	; 66
_loop
	call DELAY
	ld bc,(FRAMET)
	call DELAY	; n+fr+20
	nop		; n+fr+24

	rept _instw-1
	defb 0xDD
	endm
	nop		; n+fr+iw*4+24

	ld bc, 32768 - _instw*4 - 60 - 1
	dec de		; n+fr+iw*4+40
	ld a,d		; n+fr+iw*4+44
	or e		; n+fr+iw*4+48
	jr nz,_loop	; n+fr+iw*4+60
	ret

_int
	im 1
	ld a,e
	xor _start
	cp 1
	ret

frame_ok defw 69888, 70908
int_ok defw 32

t_ftime defb 'Frame time: ',0
t_eipx defb 'EI is prefix: ',0
t_itime defb 'INT time: ',0
t_yes defb 'yes',0
t_no defb 'no',0
t_fail defb 'test failed',0

include print.asm
include delay.asm
include instint.asm
include frametime.asm
include alignint.asm

SYS_CLS equ 0x0D6B
SYS_TEMPS equ 0x0D4D
SYS_CHAN equ 0x1601
SYS_BREAK equ 0x1F54

end entry
