;	title	'Z80 instruction set exerciser for Spectrum'

; zexall.src - Z80 instruction set exerciser
; Copyright (C) 1994  Frank D. Cringle
;
; 03-Nov-2002: Modified to assemble with ZMAC and MAXAM
; Copyright (C) 2002 J.G.Harston
; + labels on equates mustn't have trailing colon
; + macros don't understand <...> sequence, so parameters are passed
;   explicitly
; + ds n,c not supported, so strings are set to full explicit length
; + nonstandard 'cp a,' and 'and a,' changed to 'cp ' and 'and '
;
; 03-Nov-2002: Modified to run on Spectrum
; Copyright (C) 2002 J.G.Harston
;
; 26-Dec-2012: Added few more test vectors, set I, updated Spectrum CRCs
; Copyright (C) 2012 Patrik Rak
;
; This program is free software; you can redistribute it and/or
; modify it under the terms of the GNU General Public License
; as published by the Free Software Foundation; either version 2
; of the License, or (at your option) any later version.
;
; This program is distributed in the hope that it will be useful,
; but WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
; GNU General Public License for more details.
;
; You should have received a copy of the GNU General Public License
; along with this program; if not, write to the Free Software
; Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

;	aseg
	org	8000h

	jp	start

; machine state before test (needs to be at predictably constant address)
msbt:	ds	14
spbt:	ds	2
msbthi	equ	msbt / 0100h
msbtlo	equ	msbt & 0ffh


; For the purposes of this test program, the machine state consists of:
;	a 2 byte memory operand, followed by
;	the registers iy,ix,hl,de,bc,af,sp
; for a total of 16 bytes.

; The program tests instructions (or groups of similar instructions)
; by cycling through a sequence of machine states, executing the test
; instruction for each one and running a 32-bit crc over the resulting
; machine states.  At the end of the sequence the crc is compared to
; an expected value that was found empirically on a real Z80.

; A test case is defined by a descriptor which consists of:
;	a flag mask byte,
;	the base case,
;	the incement vector,
;	the shift vector,
;	the expected crc,
;	a short descriptive message.
;
; The flag mask byte is used to prevent undefined flag bits from
; influencing the results.  Documented flags are as per Mostek Z80
; Technical Manual.
;
; The next three parts of the descriptor are 20 byte vectors
; corresponding to a 4 byte instruction and a 16 byte machine state.
; The first part is the base case, which is the first test case of
; the sequence.  This base is then modified according to the next 2
; vectors.  Each 1 bit in the increment vector specifies a bit to be
; cycled in the form of a binary counter.  For instance, if the byte
; corresponding to the accumulator is set to 0ffh in the increment
; vector, the test will be repeated for all 256 values of the
; accumulator.  Note that 1 bits don't have to be contiguous.  The
; number of test cases 'caused' by the increment vector is equal to
; 2^(number of 1 bits).  The shift vector is similar, but specifies a
; set of bits in the test case that are to be successively inverted.
; Thus the shift vector 'causes' a number of test cases equal to the
; number of 1 bits in it.

; The total number of test cases is the product of those caused by the
; counter and shift vectors and can easily become unweildy.  Each
; individual test case can take a few milliseconds to execute, due to
; the overhead of test setup and crc calculation, so test design is a
; compromise between coverage and execution time.

; This program is designed to detect differences between
; implementations and is not ideal for diagnosing the causes of any
; discrepancies.  However, provided a reference implementation (or
; real system) is available, a failing test case can be isolated by
; hand using a binary search of the test space.


start:	; ld	hl,(6)
	ld	hl,08000h	; put stack at top of 32k
	ld	sp,hl
    ld  a,255
    ld  i,a
	ld	a,2
	call	01601h		; open stream 2 (screen)
	ld	de,msg1
	ld	c,9
	call	bdos

	ld	hl,tests	; first test case
loop:	ld	a,(hl)		; end of list ?
	inc	hl
	or	(hl)
	jp	z,done
	dec	hl
	call	stt
	jp	loop
	
done:	ld	de,msg2
	ld	c,9
	call	bdos
stop:
	jp	stop		; wait for reset

tests:
	dw	scfop
	dw	ccfop
	dw	scfccf
	dw	ccfscf
	dw	bita
	dw	bithl
	dw	bitx
	dw	bitz80
	dw	daaop		; can't use opcode as label
	dw	cplop
	dw	adc16
	dw	add16
	dw	add16x
	dw	add16y
	dw	alu8i
	dw	alu8r
	dw	alu8rx
	dw	alu8x
	dw	cpd1
	dw	cpi1
	dw	inca
	dw	incb
	dw	incbc
	dw	incc
	dw	incd
	dw	incde
	dw	ince
	dw	inch
	dw	inchl
	dw	incix
	dw	inciy
	dw	incl
	dw	incm
	dw	incsp
	dw	incx
	dw	incxh
	dw	incxl
	dw	incyh
	dw	incyl
	dw	ld161
	dw	ld162
	dw	ld163
	dw	ld164
	dw	ld165
	dw	ld166
	dw	ld167
	dw	ld168
	dw	ld16im
	dw	ld16ix
	dw	ld8bd
	dw	ld8im
	dw	ld8imx
	dw	ld8ix1
	dw	ld8ix2
	dw	ld8ix3
	dw	ld8ixy
	dw	ld8rr
	dw	ld8rrx
	dw	lda
	dw	ldd1
	dw	ldd2
	dw	ldi1
	dw	ldi2
	dw	negop		; jgh: can't use opcode as label
	dw	rldop		; jgh: can't use opcode as label
	dw	rot8080
	dw	rotxy
	dw	rotz80
	dw	srz80
	dw	srzx
	dw	st8ix1
	dw	st8ix2
	dw	st8ix3
	dw	stabd
	dw	0

; jgh: macro syntax changed for ZMAC and MAXAM
;	can't use opcodes as labels
;	ZMAC allows &nn as hex, so & removed from local labels
;
	macro	tstr insn1,insn2,insn3,insn4,memop,riy,rix,rhl,rde,rbc,flags,acc,rsp
.lab:	db	insn1,insn2,insn3,insn4
	dw	memop,riy,rix,rhl,rde,rbc
	db	flags
	db	acc
	dw	rsp
	if	$-.lab ne 20
;	error	'missing parameter'
	endif
	endm

	macro	tmsg msg
.lab:	db	msg
	if	$ ge .lab+31
;	error	'message too long'
	else
;	ds	.lab+30-$,'.'	; jgh: ZMAC/MAXAM don't have char parameter
	endif
	db	'$'
	endm

; jgh: ZMAC/MAXAM don't recognise <n,m> syntax for macros, so full parameters given
; jgh: each tmsg has full string, as ZMAC/MAXAM don't have ds n,c pseudo-op

; scf
scfop:	    db	%0ffh		; flag mask
	tstr	037h,0,0,0,02141h,009fah,01d60h,0a559h,08d5bh,09079h,004h,08eh,0299dh
	tstr	0,0,0,0,0,0,0,0,0,0,-1,-1,0	; (65536 cycles)
	tstr	0,0,0,0,0,0,0,0,0,0,0,0,0		; (1 cycle)
	db	0ffh,04ch,0ebh,048h			; expected crc
	db  	'scf..........................$'
	
; start test pointed to by (hl)
stt:	push	hl
	ld	a,(hl)		; get pointer to test
	inc	hl
	ld	h,(hl)
	ld	l,a
	ld	a,(hl)		; flag mask
	ld	(flgmsk+1),a
	inc	hl
	push	hl
	ld	de,20
	add	hl,de		; point to incmask
	ld	de,counter
	call	initmask
	pop	hl
	push	hl
	ld	de,20+20
	add	hl,de		; point to scanmask
	ld	de,shifter
	call	initmask
	ld	hl,shifter
	ld	(hl),1		; first bit
	pop	hl
	push	hl
	ld	de,iut		; copy initial instruction under test
	ld	bc,4
	ldir
	ld	de,msbt		; copy initial machine state
	ld	bc,16
	ldir
	ld	de,20+20+4	; skip incmask, scanmask and expcrc
	add	hl,de
	ex	de,hl
	ld	c,9
	call	bdos		; show test name
	call	initcrc		; initialise crc
; test loop
tlp:	ld	a,(iut)
	cp	076h		; pragmatically avoid halt intructions
	jp	z,tlp2
	and	0dfh
	cp	0ddh
	jp	nz,tlp1
	ld	a,(iut+1)
	cp	076h
tlp1:	call	nz,test		; execute the test instruction
tlp2:	call	count		; increment the counter
	call	nz,shift	; shift the scan bit
	pop	hl		; pointer to test case
	jp	z,tlp3		; done if shift returned NZ
	ld	de,20+20+20
	add	hl,de		; point to expected crc
	call	cmpcrc
	ld	de,okmsg
	jp	z,tlpok
	push	hl		; save pointer to crc
	ld	hl,crcval
	ld	de,ermsg1	; jgh: swap crc= and expected= messages
	ld	c,9
	call	bdos
	call	phex8
	ld	de,ermsg2
	ld	c,9
	call	bdos
	pop	hl		; get pointer to crc back
	call	phex8
	ld	de,crlf
tlpok:	ld	c,9
	call	bdos
	pop	hl
	inc	hl
	inc	hl
	ret

tlp3:	push	hl
	ld	a,1		; initialise count and shift scanners
	ld	(cntbit),a
	ld	(shfbit),a
	ld	hl,counter
	ld	(cntbyt),hl
	ld	hl,shifter
	ld	(shfbyt),hl

	ld	b,4		; bytes in iut field
	pop	hl		; pointer to test case
	push	hl
	ld	de,iut
	call	setup		; setup iut
	ld	b,16		; bytes in machine state
	ld	de,msbt
	call	setup		; setup machine state
	jp	tlp

; set up a field of the test case
; b  = number of bytes
; hl = pointer to base case
; de = destination
setup:	call	subyte
	inc	hl
	dec	b
	jp	nz,setup
	ret

subyte:	push	bc
	push	de
	push	hl
	ld	c,(hl)		; get base byte
	ld	de,20
	add	hl,de		; point to incmask
	ld	a,(hl)
	cp	0
	jp	z,subshf
	ld	b,8		; 8 bits
subclp:	rrca
	push	af
	ld	a,0
	call	c,nxtcbit	; get next counter bit if mask bit was set
	xor	c		; flip bit if counter bit was set
	rrca
	ld	c,a
	pop	af
	dec	b
	jp	nz,subclp
	ld	b,8
subshf:	ld	de,20
	add	hl,de		; point to shift mask
	ld	a,(hl)
	cp	0
	jp	z,substr
	ld	b,8		; 8 bits
sbshf1:	rrca
	push	af
	ld	a,0
	call	c,nxtsbit	; get next shifter bit if mask bit was set
	xor	c		; flip bit if shifter bit was set
	rrca
	ld	c,a
	pop	af
	dec	b
	jp	nz,sbshf1
substr:	pop	hl
	pop	de
	ld	a,c
	ld	(de),a		; mangled byte to destination
	inc	de
	pop	bc
	ret

; get next counter bit in low bit of a
cntbit:	ds	1
cntbyt:	ds	2

nxtcbit: push	bc
	push	hl
	ld	hl,(cntbyt)
	ld	b,(hl)
	ld	hl,cntbit
	ld	a,(hl)
	ld	c,a
	rlca
	ld	(hl),a
	cp	1
	jp	nz,ncb1
	ld	hl,(cntbyt)
	inc	hl
	ld	(cntbyt),hl
ncb1:	ld	a,b
	and	c
	pop	hl
	pop	bc
	ret	z
	ld	a,1
	ret
	
; get next shifter bit in low bit of a
shfbit:	ds	1
shfbyt:	ds	2

nxtsbit: push	bc
	push	hl
	ld	hl,(shfbyt)
	ld	b,(hl)
	ld	hl,shfbit
	ld	a,(hl)
	ld	c,a
	rlca
	ld	(hl),a
	cp	1
	jp	nz,nsb1
	ld	hl,(shfbyt)
	inc	hl
	ld	(shfbyt),hl
nsb1:	ld	a,b
	and	c
	pop	hl
	pop	bc
	ret	z
	ld	a,1
	ret
	

; clear memory at hl, bc bytes
clrmem:	push	af
	push	bc
	push	de
	push	hl
	ld	(hl),0
	ld	d,h
	ld	e,l
	inc	de
	dec	bc
	ldir
	pop	hl
	pop	de
	pop	bc
	pop	af
	ret

; initialise counter or shifter
; de = pointer to work area for counter or shifter
; hl = pointer to mask
initmask:
	push	de
	ex	de,hl
	ld	bc,20+20
	call	clrmem		; clear work area
	ex	de,hl
	ld	b,20		; byte counter
	ld	c,1		; first bit
	ld	d,0		; bit counter
imlp:	ld	e,(hl)
imlp1:	ld	a,e
	and	c
	jp	z,imlp2
	inc	d
imlp2:	ld	a,c
	rlca
	ld	c,a
	cp	1
	jp	nz,imlp1
	inc	hl
	dec	b
	jp	nz,imlp
; got number of 1-bits in mask in reg d
	ld	a,d
	and	0f8h
	rrca
	rrca
	rrca			; divide by 8 (get byte offset)
	ld	l,a
	ld	h,0
	ld	a,d
	and	7		; bit offset
	inc	a
	ld	b,a
	ld	a,080h
imlp3:	rlca
	dec	b
	jp	nz,imlp3
	pop	de
	add	hl,de
	ld	de,20
	add	hl,de
	ld	(hl),a
	ret

; multi-byte counter
count:	push	bc
	push	de
	push	hl
	ld	hl,counter	; 20 byte counter starts here
	ld	de,20		; somewhere in here is the stop bit
	ex	de,hl
	add	hl,de
	ex	de,hl
cntlp:	inc	(hl)
	ld	a,(hl)
	cp	0
	jp	z,cntlp1	; overflow to next byte
	ld	b,a
	ld	a,(de)
	and	b		; test for terminal value
	jp	z,cntend
	ld	(hl),0		; reset to zero
cntend:	pop	bc
	pop	de
	pop	hl
	ret

cntlp1:	inc	hl
	inc	de
	jp	cntlp
	

; multi-byte shifter
shift:	push	bc
	push	de
	push	hl
	ld	hl,shifter	; 20 byte shift register starts here
	ld	de,20		; somewhere in here is the stop bit
	ex	de,hl
	add	hl,de
	ex	de,hl
shflp:	ld	a,(hl)
	or	a
	jp	z,shflp1
	ld	b,a
	ld	a,(de)
	and	b
	jp	nz,shlpe
	ld	a,b
	rlca
	cp	1
	jp	nz,shflp2
	ld	(hl),0
	inc	hl
	inc	de
shflp2:	ld	(hl),a
	xor	a		; set Z
shlpe:	pop	hl
	pop	de
	pop	bc
	ret
shflp1:	inc	hl
	inc	de
	jp	shflp

counter: ds	2*20
shifter: ds	2*20

; test harness
test:	push	af
	push	bc
	push	de
	push	hl
      if	0
	ld	de,crlf
	ld	c,9
	call	bdos
	ld	hl,iut
	ld	b,4
	call	hexstr
	ld	e,' '
	ld	c,2
	call	bdos
	ld	b,16
	ld	hl,msbt
	call	hexstr
      endif	
	push	iy		; jgh: Spectrum INTs change (IY+x)
	di			; disable interrupts
	ld	(spsav),sp	; save stack pointer
	ld	sp,msbt+2	; point to test-case machine state
	pop	iy		; and load all regs
	pop	ix
	pop	hl
	pop	de
	pop	bc
	pop	af
	ld	sp,(spbt)
iut:	ds	4		; max 4 byte instruction under test
	ld	(spat),sp	; save stack pointer
	ld	sp,spat
	push	af		; save other registers
	push	bc
	push	de
	push	hl
	push	ix
	push	iy
	ld	sp,(spsav)	; restore stack pointer
	pop	iy		; jgh: Restore Spectrum's IY
	ei			; enable interrupts
	ld	hl,(msbt)	; copy memory operand
	ld	(msat),hl
	ld	hl,flgsat	; flags after test
	ld	a,(hl)
flgmsk:	and	0d7h		; mask-out irrelevant bits (self-modified code!)
	ld	(hl),a
	ld	b,16		; total of 16 bytes of state
	ld	de,msat
	ld	hl,crcval
tcrc:	ld	a,(de)
	inc	de
	call	updcrc		; accumulate crc of this test case
	dec	b
	jp	nz,tcrc
      if	0
	ld	e,' '
	ld	c,2
	call	bdos
	ld	hl,crcval
	call	phex8
	ld	de,crlf
	ld	c,9
	call	bdos
	ld	hl,msat
	ld	b,16
	call	hexstr
	ld	de,crlf
	ld	c,9
	call	bdos
      endif
	pop	hl
	pop	de
	pop	bc
	pop	af
	ret

; machine state after test
msat:	ds	14	; memop,iy,ix,hl,de,bc,af
spat:	ds	2	; stack pointer after test
; ZMAC/MAXAM doesn't like ':' after label with EQUs
flgsat	equ	spat-2	; flags

spsav:	ds	2	; saved stack pointer

; display hex string (pointer in hl, byte count in b)
hexstr:	ld	a,(hl)
	call	phex2
	inc	hl
	dec	b
	jp	nz,hexstr
	ret

; display hex
; display the big-endian 32-bit value pointed to by hl
phex8:	push	af
	push	bc
	push	hl
	ld	b,4
ph8lp:	ld	a,(hl)
	call	phex2
	inc	hl
	dec	b
	jp	nz,ph8lp
	pop	hl
	pop	bc
	pop	af
	ret

; display byte in a
phex2:	push	af
	rrca
	rrca
	rrca
	rrca
	call	phex1
	pop	af
; fall through	

; display low nibble in a
phex1:	push	af
	push	bc
	push	de
	push	hl
	and	0fh
	cp	10
	jp	c,ph11
	add	a,'a'-'9'-1
ph11:	add	a,'0'
	ld	e,a
	ld	c,2
	call	bdos
	pop	hl
	pop	de
	pop	bc
	pop	af
	ret

bdos:	push	af
	push	bc
	push	de
	push	hl
; Accept call 2 (print char) and call 9 (print string)
; Ignore all others
	ld	b,a		; save char (destroys B)
	ld	a,c
	cp	2
	jr	z,prchar
	cp	9
	jr	z,prstring
bdosdone:
	pop	hl
	pop	de
	pop	bc
	pop	af
	ret

; Pass bdos calls to Spectrum system
prchar:
	ld	a,b		; get char back
	cp	10		; ignore LF as CR auto-LFs
	jr	z,bdosdone
	rst	010h
	jr	bdosdone

prstring:
	ld	a,(de)
	cp	'$'
	jr	z,bdosdone
	cp	10
	call	nz,010h
	inc	de
	jr	prstring


msg1:	db	'Z80all2 instruction exerciser',10,13,10,13,'$'
msg2:	db	'Tests complete$'
okmsg:	db	'OK',10,13,'$'
ermsg1:	db	'   CRC:$'	; was ERROR:
ermsg2:	db	' expected:$'
crlf:	db	10,13,'$'

; compare crc
; hl points to value to compare to crcval
cmpcrc:	push	bc
	push	de
	push	hl
	ld	de,crcval
	ld	b,4
cclp:	ld	a,(de)
	cp	(hl)
	jp	nz,cce
	inc	hl
	inc	de
	dec	b
	jp	nz,cclp
cce:	pop	hl
	pop	de
	pop	bc
	ret

; 32-bit crc routine
; entry: a contains next byte, hl points to crc
; exit:  crc updated
updcrc:	push	af
	push	bc
	push	de
	push	hl
	push	hl
	ld	de,3
	add	hl,de	; point to low byte of old crc
	xor	(hl)	; xor with new byte
	ld	l,a
	ld	h,0
	add	hl,hl	; use result as index into table of 4 byte entries
	add	hl,hl
	ex	de,hl
	ld	hl,crctab
	add	hl,de	; point to selected entry in crctab
	ex	de,hl
	pop	hl
	ld	bc,4	; c = byte count, b = accumulator
crclp:	ld	a,(de)
	xor	b
	ld	b,(hl)
	ld	(hl),a
	inc	de
	inc	hl
	dec	c
	jp	nz,crclp
      if	0
	ld	hl,crcval
	call	phex8
	ld	de,crlf
	ld	c,9
	call	bdos
      endif
	pop	hl
	pop	de
	pop	bc
	pop	af
	ret

initcrc:push	af
	push	bc
	push	hl
	ld	hl,crcval
	ld	a,0ffh
	ld	b,4
icrclp:	ld	(hl),a
	inc	hl
	dec	b
	jp	nz,icrclp
	pop	hl
	pop	bc
	pop	af
	ret

crcval	ds	4

crctab:	db	000h,000h,000h,000h
	db	077h,007h,030h,096h
	db	0eeh,00eh,061h,02ch
	db	099h,009h,051h,0bah
	db	007h,06dh,0c4h,019h
	db	070h,06ah,0f4h,08fh
	db	0e9h,063h,0a5h,035h
	db	09eh,064h,095h,0a3h
	db	00eh,0dbh,088h,032h
	db	079h,0dch,0b8h,0a4h
	db	0e0h,0d5h,0e9h,01eh
	db	097h,0d2h,0d9h,088h
	db	009h,0b6h,04ch,02bh
	db	07eh,0b1h,07ch,0bdh
	db	0e7h,0b8h,02dh,007h
	db	090h,0bfh,01dh,091h
	db	01dh,0b7h,010h,064h
	db	06ah,0b0h,020h,0f2h
	db	0f3h,0b9h,071h,048h
	db	084h,0beh,041h,0deh
	db	01ah,0dah,0d4h,07dh
	db	06dh,0ddh,0e4h,0ebh
	db	0f4h,0d4h,0b5h,051h
	db	083h,0d3h,085h,0c7h
	db	013h,06ch,098h,056h
	db	064h,06bh,0a8h,0c0h
	db	0fdh,062h,0f9h,07ah
	db	08ah,065h,0c9h,0ech
	db	014h,001h,05ch,04fh
	db	063h,006h,06ch,0d9h
	db	0fah,00fh,03dh,063h
	db	08dh,008h,00dh,0f5h
	db	03bh,06eh,020h,0c8h
	db	04ch,069h,010h,05eh
	db	0d5h,060h,041h,0e4h
	db	0a2h,067h,071h,072h
	db	03ch,003h,0e4h,0d1h
	db	04bh,004h,0d4h,047h
	db	0d2h,00dh,085h,0fdh
	db	0a5h,00ah,0b5h,06bh
	db	035h,0b5h,0a8h,0fah
	db	042h,0b2h,098h,06ch
	db	0dbh,0bbh,0c9h,0d6h
	db	0ach,0bch,0f9h,040h
	db	032h,0d8h,06ch,0e3h
	db	045h,0dfh,05ch,075h
	db	0dch,0d6h,00dh,0cfh
	db	0abh,0d1h,03dh,059h
	db	026h,0d9h,030h,0ach
	db	051h,0deh,000h,03ah
	db	0c8h,0d7h,051h,080h
	db	0bfh,0d0h,061h,016h
	db	021h,0b4h,0f4h,0b5h
	db	056h,0b3h,0c4h,023h
	db	0cfh,0bah,095h,099h
	db	0b8h,0bdh,0a5h,00fh
	db	028h,002h,0b8h,09eh
	db	05fh,005h,088h,008h
	db	0c6h,00ch,0d9h,0b2h
	db	0b1h,00bh,0e9h,024h
	db	02fh,06fh,07ch,087h
	db	058h,068h,04ch,011h
	db	0c1h,061h,01dh,0abh
	db	0b6h,066h,02dh,03dh
	db	076h,0dch,041h,090h
	db	001h,0dbh,071h,006h
	db	098h,0d2h,020h,0bch
	db	0efh,0d5h,010h,02ah
	db	071h,0b1h,085h,089h
	db	006h,0b6h,0b5h,01fh
	db	09fh,0bfh,0e4h,0a5h
	db	0e8h,0b8h,0d4h,033h
	db	078h,007h,0c9h,0a2h
	db	00fh,000h,0f9h,034h
	db	096h,009h,0a8h,08eh
	db	0e1h,00eh,098h,018h
	db	07fh,06ah,00dh,0bbh
	db	008h,06dh,03dh,02dh
	db	091h,064h,06ch,097h
	db	0e6h,063h,05ch,001h
	db	06bh,06bh,051h,0f4h
	db	01ch,06ch,061h,062h
	db	085h,065h,030h,0d8h
	db	0f2h,062h,000h,04eh
	db	06ch,006h,095h,0edh
	db	01bh,001h,0a5h,07bh
	db	082h,008h,0f4h,0c1h
	db	0f5h,00fh,0c4h,057h
	db	065h,0b0h,0d9h,0c6h
	db	012h,0b7h,0e9h,050h
	db	08bh,0beh,0b8h,0eah
	db	0fch,0b9h,088h,07ch
	db	062h,0ddh,01dh,0dfh
	db	015h,0dah,02dh,049h
	db	08ch,0d3h,07ch,0f3h
	db	0fbh,0d4h,04ch,065h
	db	04dh,0b2h,061h,058h
	db	03ah,0b5h,051h,0ceh
	db	0a3h,0bch,000h,074h
	db	0d4h,0bbh,030h,0e2h
	db	04ah,0dfh,0a5h,041h
	db	03dh,0d8h,095h,0d7h
	db	0a4h,0d1h,0c4h,06dh
	db	0d3h,0d6h,0f4h,0fbh
	db	043h,069h,0e9h,06ah
	db	034h,06eh,0d9h,0fch
	db	0adh,067h,088h,046h
	db	0dah,060h,0b8h,0d0h
	db	044h,004h,02dh,073h
	db	033h,003h,01dh,0e5h
	db	0aah,00ah,04ch,05fh
	db	0ddh,00dh,07ch,0c9h
	db	050h,005h,071h,03ch
	db	027h,002h,041h,0aah
	db	0beh,00bh,010h,010h
	db	0c9h,00ch,020h,086h
	db	057h,068h,0b5h,025h
	db	020h,06fh,085h,0b3h
	db	0b9h,066h,0d4h,009h
	db	0ceh,061h,0e4h,09fh
	db	05eh,0deh,0f9h,00eh
	db	029h,0d9h,0c9h,098h
	db	0b0h,0d0h,098h,022h
	db	0c7h,0d7h,0a8h,0b4h
	db	059h,0b3h,03dh,017h
	db	02eh,0b4h,00dh,081h
	db	0b7h,0bdh,05ch,03bh
	db	0c0h,0bah,06ch,0adh
	db	0edh,0b8h,083h,020h
	db	09ah,0bfh,0b3h,0b6h
	db	003h,0b6h,0e2h,00ch
	db	074h,0b1h,0d2h,09ah
	db	0eah,0d5h,047h,039h
	db	09dh,0d2h,077h,0afh
	db	004h,0dbh,026h,015h
	db	073h,0dch,016h,083h
	db	0e3h,063h,00bh,012h
	db	094h,064h,03bh,084h
	db	00dh,06dh,06ah,03eh
	db	07ah,06ah,05ah,0a8h
	db	0e4h,00eh,0cfh,00bh
	db	093h,009h,0ffh,09dh
	db	00ah,000h,0aeh,027h
	db	07dh,007h,09eh,0b1h
	db	0f0h,00fh,093h,044h
	db	087h,008h,0a3h,0d2h
	db	01eh,001h,0f2h,068h
	db	069h,006h,0c2h,0feh
	db	0f7h,062h,057h,05dh
	db	080h,065h,067h,0cbh
	db	019h,06ch,036h,071h
	db	06eh,06bh,006h,0e7h
	db	0feh,0d4h,01bh,076h
	db	089h,0d3h,02bh,0e0h
	db	010h,0dah,07ah,05ah
	db	067h,0ddh,04ah,0cch
	db	0f9h,0b9h,0dfh,06fh
	db	08eh,0beh,0efh,0f9h
	db	017h,0b7h,0beh,043h
	db	060h,0b0h,08eh,0d5h
	db	0d6h,0d6h,0a3h,0e8h
	db	0a1h,0d1h,093h,07eh
	db	038h,0d8h,0c2h,0c4h
	db	04fh,0dfh,0f2h,052h
	db	0d1h,0bbh,067h,0f1h
	db	0a6h,0bch,057h,067h
	db	03fh,0b5h,006h,0ddh
	db	048h,0b2h,036h,04bh
	db	0d8h,00dh,02bh,0dah
	db	0afh,00ah,01bh,04ch
	db	036h,003h,04ah,0f6h
	db	041h,004h,07ah,060h
	db	0dfh,060h,0efh,0c3h
	db	0a8h,067h,0dfh,055h
	db	031h,06eh,08eh,0efh
	db	046h,069h,0beh,079h
	db	0cbh,061h,0b3h,08ch
	db	0bch,066h,083h,01ah
	db	025h,06fh,0d2h,0a0h
	db	052h,068h,0e2h,036h
	db	0cch,00ch,077h,095h
	db	0bbh,00bh,047h,003h
	db	022h,002h,016h,0b9h
	db	055h,005h,026h,02fh
	db	0c5h,0bah,03bh,0beh
	db	0b2h,0bdh,00bh,028h
	db	02bh,0b4h,05ah,092h
	db	05ch,0b3h,06ah,004h
	db	0c2h,0d7h,0ffh,0a7h
	db	0b5h,0d0h,0cfh,031h
	db	02ch,0d9h,09eh,08bh
	db	05bh,0deh,0aeh,01dh
	db	09bh,064h,0c2h,0b0h
	db	0ech,063h,0f2h,026h
	db	075h,06ah,0a3h,09ch
	db	002h,06dh,093h,00ah
	db	09ch,009h,006h,0a9h
	db	0ebh,00eh,036h,03fh
	db	072h,007h,067h,085h
	db	005h,000h,057h,013h
	db	095h,0bfh,04ah,082h
	db	0e2h,0b8h,07ah,014h
	db	07bh,0b1h,02bh,0aeh
	db	00ch,0b6h,01bh,038h
	db	092h,0d2h,08eh,09bh
	db	0e5h,0d5h,0beh,00dh
	db	07ch,0dch,0efh,0b7h
	db	00bh,0dbh,0dfh,021h
	db	086h,0d3h,0d2h,0d4h
	db	0f1h,0d4h,0e2h,042h
	db	068h,0ddh,0b3h,0f8h
	db	01fh,0dah,083h,06eh
	db	081h,0beh,016h,0cdh
	db	0f6h,0b9h,026h,05bh
	db	06fh,0b0h,077h,0e1h
	db	018h,0b7h,047h,077h
	db	088h,008h,05ah,0e6h
	db	0ffh,00fh,06ah,070h
	db	066h,006h,03bh,0cah
	db	011h,001h,00bh,05ch
	db	08fh,065h,09eh,0ffh
	db	0f8h,062h,0aeh,069h
	db	061h,06bh,0ffh,0d3h
	db	016h,06ch,0cfh,045h
	db	0a0h,00ah,0e2h,078h
	db	0d7h,00dh,0d2h,0eeh
	db	04eh,004h,083h,054h
	db	039h,003h,0b3h,0c2h
	db	0a7h,067h,026h,061h
	db	0d0h,060h,016h,0f7h
	db	049h,069h,047h,04dh
	db	03eh,06eh,077h,0dbh
	db	0aeh,0d1h,06ah,04ah
	db	0d9h,0d6h,05ah,0dch
	db	040h,0dfh,00bh,066h
	db	037h,0d8h,03bh,0f0h
	db	0a9h,0bch,0aeh,053h
	db	0deh,0bbh,09eh,0c5h
	db	047h,0b2h,0cfh,07fh
	db	030h,0b5h,0ffh,0e9h
	db	0bdh,0bdh,0f2h,01ch
	db	0cah,0bah,0c2h,08ah
	db	053h,0b3h,093h,030h
	db	024h,0b4h,0a3h,0a6h
	db	0bah,0d0h,036h,005h
	db	0cdh,0d7h,006h,093h
	db	054h,0deh,057h,029h
	db	023h,0d9h,067h,0bfh
	db	0b3h,066h,07ah,02eh
	db	0c4h,061h,04ah,0b8h
	db	05dh,068h,01bh,002h
	db	02ah,06fh,02bh,094h
	db	0b4h,00bh,0beh,037h
	db	0c3h,00ch,08eh,0a1h
	db	05ah,005h,0dfh,01bh
	db	02dh,002h,0efh,08dh

