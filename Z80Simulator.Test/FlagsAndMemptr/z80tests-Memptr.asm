﻿; Launcher for MEMPTR  test 31 : DJNZ (taken)
MEMPTR_TEST_31_LAUNCH:	LD HL,MEMPTR_TEST_31_EXEC
	LD BC,002BH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 31 : DJNZ (taken)
MEMPTR_TEST_31_EXEC:	CALL EXEC_DJNZ_FF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 32 : DJNZ (not taken)
MEMPTR_TEST_32_LAUNCH:	LD HL,MEMPTR_TEST_32_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 32 : DJNZ (not taken)
MEMPTR_TEST_32_EXEC:	CALL EXEC_DJNZ_01
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 01 : LD A,(addr)
MEMPTR_TEST_01_LAUNCH:	LD HL,MEMPTR_TEST_01_EXEC
	LD BC,2FF8H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 01 : LD A,(addr)
MEMPTR_TEST_01_EXEC:	PUSH AF
	LD A,(2FF7H)
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 02 : LD (addr),A
MEMPTR_TEST_02_LAUNCH:	LD HL,MEMPTR_TEST_02_EXEC
	LD BC,35FCH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 02 : LD (addr),A
MEMPTR_TEST_02_EXEC:	PUSH AF
	LD A,35H
	LD (20FBH),A
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 03 : LD A,(BC)
MEMPTR_TEST_03_LAUNCH:	LD HL,MEMPTR_TEST_03_EXEC
	LD BC,1F8DH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 03 : LD A,(BC)
MEMPTR_TEST_03_EXEC:	PUSH BC
	PUSH AF
	LD BC,1F8CH
	LD A,(BC)
	POP AF
	POP BC
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 04 : LD A,(DE)
MEMPTR_TEST_04_LAUNCH:	LD HL,MEMPTR_TEST_04_EXEC
	LD BC,311FH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 04 : LD A,(DE)
MEMPTR_TEST_04_EXEC:	PUSH DE
	PUSH AF
	LD DE,311EH
	LD A,(DE)
	POP AF
	POP DE
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 05 : LD A,(HL)
MEMPTR_TEST_05_LAUNCH:	LD HL,MEMPTR_TEST_05_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 05 : LD A,(HL)
MEMPTR_TEST_05_EXEC:	PUSH HL
	PUSH AF
	LD HL,2113H
	LD A,(HL)
	POP AF
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 06 : LD (BC),A
MEMPTR_TEST_06_LAUNCH:	LD HL,MEMPTR_TEST_06_EXEC
	LD BC,179CH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 06 : LD (BC),A
MEMPTR_TEST_06_EXEC:	PUSH BC
	PUSH AF
	LD BC,0E9BH
	LD A,17H
	LD (BC),A
	POP AF
	POP BC
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 07 : LD (DE),A
MEMPTR_TEST_07_LAUNCH:	LD HL,MEMPTR_TEST_07_EXEC
	LD BC,3A08H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 07 : LD (DE),A
MEMPTR_TEST_07_EXEC:	PUSH DE
	PUSH AF
	LD DE,3307H
	LD A,FAH
	LD (DE),A
	POP AF
	POP DE
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 08 : LD (HL),A
MEMPTR_TEST_08_LAUNCH:	LD HL,MEMPTR_TEST_08_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 08 : LD (HL),A
MEMPTR_TEST_08_EXEC:	PUSH HL
	PUSH AF
	LD HL,1EFEH
	LD A,FFH
	LD (HL),A
	POP AF
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 09 : LD HL,(addr)
MEMPTR_TEST_09_LAUNCH:	LD HL,MEMPTR_TEST_09_EXEC
	LD BC,0F0AH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 09 : LD HL,(addr)
MEMPTR_TEST_09_EXEC:	PUSH HL
	LD HL,(0F09H)
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 10 : LD HL,(addr) [ED]
MEMPTR_TEST_10_LAUNCH:	LD HL,MEMPTR_TEST_10_EXEC
	LD BC,21DBH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 10 : LD HL,(addr) [ED]
MEMPTR_TEST_10_EXEC:	PUSH HL
	LD[EDH 6BH] HL,(21DAH)
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 11 : LD DE,(addr)
MEMPTR_TEST_11_LAUNCH:	LD HL,MEMPTR_TEST_11_EXEC
	LD BC,347BH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 11 : LD DE,(addr)
MEMPTR_TEST_11_EXEC:	PUSH DE
	LD DE,(347AH)
	POP DE
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 12 : LD BC,(addr)
MEMPTR_TEST_12_LAUNCH:	LD HL,MEMPTR_TEST_12_EXEC
	LD BC,0140H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 12 : LD BC,(addr)
MEMPTR_TEST_12_EXEC:	PUSH BC
	LD BC,(013FH)
	POP BC
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 13 : LD IX,(addr)
MEMPTR_TEST_13_LAUNCH:	LD HL,MEMPTR_TEST_13_EXEC
	LD BC,2F40H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 13 : LD IX,(addr)
MEMPTR_TEST_13_EXEC:	PUSH IX
	LD IX,(2F3FH)
	POP IX
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 14 : LD IY,(addr)
MEMPTR_TEST_14_LAUNCH:	LD HL,MEMPTR_TEST_14_EXEC
	LD BC,0001H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 14 : LD IY,(addr)
MEMPTR_TEST_14_EXEC:	PUSH IY
	LD IY,(0000H)
	POP IY
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 15 : LD SP,(addr)
MEMPTR_TEST_15_LAUNCH:	LD HL,MEMPTR_TEST_15_EXEC
	LD BC,1978H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 15 : LD SP,(addr)
MEMPTR_TEST_15_EXEC:	PUSH HL
	LD HL,0000H
	ADD HL,SP
	LD SP,(1977H)
	LD SP,HL
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 16 : LD (addr),HL
MEMPTR_TEST_16_LAUNCH:	LD HL,MEMPTR_TEST_16_EXEC
	LD BC,3042H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 16 : LD (addr),HL
MEMPTR_TEST_16_EXEC:	LD (3041H),HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 17 : LD (addr),HL [ED]
MEMPTR_TEST_17_LAUNCH:	LD HL,MEMPTR_TEST_17_EXEC
	LD BC,1A45H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 17 : LD (addr),HL [ED]
MEMPTR_TEST_17_EXEC:	LD[EDH 63H] (1A44H),HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 18 : LD (addr),DE
MEMPTR_TEST_18_LAUNCH:	LD HL,MEMPTR_TEST_18_EXEC
	LD BC,000AH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 18 : LD (addr),DE
MEMPTR_TEST_18_EXEC:	LD (0009H),DE
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 19 : LD (addr),BC
MEMPTR_TEST_19_LAUNCH:	LD HL,MEMPTR_TEST_19_EXEC
	LD BC,2E98H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 19 : LD (addr),BC
MEMPTR_TEST_19_EXEC:	LD (2E97H),BC
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 20 : LD (addr),IX
MEMPTR_TEST_20_LAUNCH:	LD HL,MEMPTR_TEST_20_EXEC
	LD BC,0D56H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 20 : LD (addr),IX
MEMPTR_TEST_20_EXEC:	LD (0D55H),IX
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 21 : LD (addr),IY
MEMPTR_TEST_21_LAUNCH:	LD HL,MEMPTR_TEST_21_EXEC
	LD BC,1912H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 21 : LD (addr),IY
MEMPTR_TEST_21_EXEC:	LD (1911H),IY
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 22 : LD (addr),SP
MEMPTR_TEST_22_LAUNCH:	LD HL,MEMPTR_TEST_22_EXEC
	LD BC,0115H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 22 : LD (addr),SP
MEMPTR_TEST_22_EXEC:	LD (0114H),SP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 23 : EX (SP),HL
MEMPTR_TEST_23_LAUNCH:	LD HL,MEMPTR_TEST_23_EXEC
	LD BC,3809H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 23 : EX (SP),HL
MEMPTR_TEST_23_EXEC:	PUSH HL
	LD HL,3809H
	EX (SP),HL
	EX (SP),HL
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 24 : EX (SP),IX
MEMPTR_TEST_24_LAUNCH:	LD HL,MEMPTR_TEST_24_EXEC
	LD BC,2114H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 24 : EX (SP),IX
MEMPTR_TEST_24_EXEC:	PUSH IX
	LD IX,2114H
	EX (SP),IX
	EX (SP),IX
	POP IX
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 25 : EX (SP),IY
MEMPTR_TEST_25_LAUNCH:	LD HL,MEMPTR_TEST_25_EXEC
	LD BC,0737H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 25 : EX (SP),IY
MEMPTR_TEST_25_EXEC:	PUSH IY
	LD IY,0737H
	EX (SP),IY
	EX (SP),IY
	POP IY
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 26 : ADD HL,BC
MEMPTR_TEST_26_LAUNCH:	LD HL,MEMPTR_TEST_26_EXEC
	LD BC,099BH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 26 : ADD HL,BC
MEMPTR_TEST_26_EXEC:	PUSH HL
	PUSH BC
	LD HL,099AH
	LD BC,1000H
	ADD HL,BC
	POP BC
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 27 : ADD IX,BC
MEMPTR_TEST_27_LAUNCH:	LD HL,MEMPTR_TEST_27_EXEC
	LD BC,2A44H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 27 : ADD IX,BC
MEMPTR_TEST_27_EXEC:	PUSH IX
	PUSH BC
	LD IX,2A43H
	LD BC,1000H
	ADD IX,BC
	POP BC
	POP IX
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 28 : ADD IY,BC
MEMPTR_TEST_28_LAUNCH:	LD HL,MEMPTR_TEST_28_EXEC
	LD BC,019DH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 28 : ADD IY,BC
MEMPTR_TEST_28_EXEC:	PUSH IY
	PUSH BC
	LD IY,019CH
	LD BC,1000H
	ADD IY,BC
	POP BC
	POP IY
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 29 : ADC HL,BC
MEMPTR_TEST_29_LAUNCH:	LD HL,MEMPTR_TEST_29_EXEC
	LD BC,1773H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 29 : ADC HL,BC
MEMPTR_TEST_29_EXEC:	PUSH HL
	PUSH BC
	LD HL,1772H
	LD BC,1000H
	OR A
	ADC HL,BC
	POP BC
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 30 : SBC HL,BC
MEMPTR_TEST_30_LAUNCH:	LD HL,MEMPTR_TEST_30_EXEC
	LD BC,2446H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 30 : SBC HL,BC
MEMPTR_TEST_30_EXEC:	PUSH HL
	PUSH BC
	LD HL,2445H
	LD BC,1000H
	OR A
	SBC HL,BC
	POP BC
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 33 : RLD
MEMPTR_TEST_33_LAUNCH:	LD HL,MEMPTR_TEST_33_EXEC
	LD BC,117FH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 33 : RLD
MEMPTR_TEST_33_EXEC:	PUSH HL
	PUSH AF
	LD HL,117EH
	LD A,FDH
	RLD
	POP AF
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 34 : RRD
MEMPTR_TEST_34_LAUNCH:	LD HL,MEMPTR_TEST_34_EXEC
	LD BC,34AEH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 34 : RRD
MEMPTR_TEST_34_EXEC:	PUSH HL
	PUSH AF
	LD HL,34ADH
	LD A,FDH
	RRD
	POP AF
	POP HL
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 35 : IN A,(port)
MEMPTR_TEST_35_LAUNCH:	LD HL,MEMPTR_TEST_35_EXEC
	LD BC,3100H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 35 : IN A,(port)
MEMPTR_TEST_35_EXEC:	PUSH AF
	LD A,30H
	IN A,(FFH)
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 36 : IN A,(C)
MEMPTR_TEST_36_LAUNCH:	LD HL,MEMPTR_TEST_36_EXEC
	LD BC,0AFFH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 36 : IN A,(C)
MEMPTR_TEST_36_EXEC:	PUSH BC
	PUSH AF
	LD BC,0AFEH
	IN A,(C)
	POP AF
	POP BC
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 37 : OUT (port),A
MEMPTR_TEST_37_LAUNCH:	LD HL,MEMPTR_TEST_37_EXEC
	LD BC,2100H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 37 : OUT (port),A
MEMPTR_TEST_37_EXEC:	PUSH AF
	LD A,21H
	OUT (FFH),A
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 38 : OUT (C),A
MEMPTR_TEST_38_LAUNCH:	LD HL,MEMPTR_TEST_38_EXEC
	LD BC,3DFFH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 38 : OUT (C),A
MEMPTR_TEST_38_EXEC:	PUSH BC
	PUSH AF
	LD BC,3DFEH
	LD A,07H
	OUT (C),A
	POP AF
	POP BC
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 39 : LDI
MEMPTR_TEST_39_LAUNCH:	LD HL,MEMPTR_TEST_39_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 39 : LDI
MEMPTR_TEST_39_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD DE,2000H
	LD BC,3000H
	LDI
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 40 : LDD
MEMPTR_TEST_40_LAUNCH:	LD HL,MEMPTR_TEST_40_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 40 : LDD
MEMPTR_TEST_40_EXEC:	PUSH AF
	EXX
	LD HL,2000H
	LD DE,3000H
	LD BC,1000H
	LDD
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 41 : LDIR (BC=1)
MEMPTR_TEST_41_LAUNCH:	LD HL,MEMPTR_TEST_41_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 41 : LDIR (BC=1)
MEMPTR_TEST_41_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD DE,2000H
	LD BC,0001H
	CALL EXEC_LDIR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 42 : LDIR (BC>1)
MEMPTR_TEST_42_LAUNCH:	LD HL,MEMPTR_TEST_42_EXEC
	LD BC,0007H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 42 : LDIR (BC>1)
MEMPTR_TEST_42_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD DE,2000H
	LD BC,000AH
	CALL EXEC_LDIR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 43 : LDDR (BC=1)
MEMPTR_TEST_43_LAUNCH:	LD HL,MEMPTR_TEST_43_EXEC
	LD BC,0000H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 43 : LDDR (BC=1)
MEMPTR_TEST_43_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD DE,2000H
	LD BC,0001H
	CALL EXEC_LDDR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 44 : LDDR (BC>1)
MEMPTR_TEST_44_LAUNCH:	LD HL,MEMPTR_TEST_44_EXEC
	LD BC,000EH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 44 : LDDR (BC>1)
MEMPTR_TEST_44_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD DE,2000H
	LD BC,000AH
	CALL EXEC_LDDR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 45 : CPI
MEMPTR_TEST_45_LAUNCH:	LD HL,MEMPTR_TEST_45_EXEC
	LD BC,0001H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 45 : CPI
MEMPTR_TEST_45_EXEC:	PUSH AF
	EXX
	CPI
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 46 : CPD
MEMPTR_TEST_46_LAUNCH:	LD HL,MEMPTR_TEST_46_EXEC
	LD BC,3FFFH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 46 : CPD
MEMPTR_TEST_46_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	CPD
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 47 : CPIR (BC=1)
MEMPTR_TEST_47_LAUNCH:	LD HL,MEMPTR_TEST_47_EXEC
	LD BC,0001H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 47 : CPIR (BC=1)
MEMPTR_TEST_47_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD BC,0001H
	XOR A
	CALL EXEC_CPIR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 48 : CPIR (BC>1)
MEMPTR_TEST_48_LAUNCH:	LD HL,MEMPTR_TEST_48_EXEC
	LD BC,0017H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 48 : CPIR (BC>1)
MEMPTR_TEST_48_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD BC,0002H
	XOR A
	CALL EXEC_CPIR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 49 : CPDR (BC=1)
MEMPTR_TEST_49_LAUNCH:	LD HL,MEMPTR_TEST_49_EXEC
	LD BC,3FFFH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 49 : CPDR (BC=1)
MEMPTR_TEST_49_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD BC,0001H
	XOR A
	CALL EXEC_CPDR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 50 : CPDR (BC>1)
MEMPTR_TEST_50_LAUNCH:	LD HL,MEMPTR_TEST_50_EXEC
	LD BC,001FH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 50 : CPDR (BC>1)
MEMPTR_TEST_50_EXEC:	PUSH AF
	EXX
	LD HL,1000H
	LD BC,0002H
	XOR A
	CALL EXEC_CPDR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 51 : INI
MEMPTR_TEST_51_LAUNCH:	LD HL,MEMPTR_TEST_51_EXEC
	LD BC,3FFFH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 51 : INI
MEMPTR_TEST_51_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,3FFEH
	INI
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 52 : IND
MEMPTR_TEST_52_LAUNCH:	LD HL,MEMPTR_TEST_52_EXEC
	LD BC,3FFDH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 52 : IND
MEMPTR_TEST_52_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,3FFEH
	IND
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 53 : INIR
MEMPTR_TEST_53_LAUNCH:	LD HL,MEMPTR_TEST_53_EXEC
	LD BC,01FFH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 53 : INIR
MEMPTR_TEST_53_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,04FEH
	INIR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 54 : INDR
MEMPTR_TEST_54_LAUNCH:	LD HL,MEMPTR_TEST_54_EXEC
	LD BC,01FDH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 54 : INDR
MEMPTR_TEST_54_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,08FEH
	INDR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 55 : OUTI
MEMPTR_TEST_55_LAUNCH:	LD HL,MEMPTR_TEST_55_EXEC
	LD BC,3F00H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 55 : OUTI
MEMPTR_TEST_55_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,3FFFH
	OUTI
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 56 : OUTD
MEMPTR_TEST_56_LAUNCH:	LD HL,MEMPTR_TEST_56_EXEC
	LD BC,01FEH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 56 : OUTD
MEMPTR_TEST_56_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,02FFH
	OUTD
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 57 : OTIR
MEMPTR_TEST_57_LAUNCH:	LD HL,MEMPTR_TEST_57_EXEC
	LD BC,0100H
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 57 : OTIR
MEMPTR_TEST_57_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,04FFH
	OTIR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
; Launcher for MEMPTR  test 58 : OTDR
MEMPTR_TEST_58_LAUNCH:	LD HL,MEMPTR_TEST_58_EXEC
	LD BC,00FEH
	JP MEMPTR_TEST_COMMON_EXEC
; Code executed for MEMPTR  test 58 : OTDR
MEMPTR_TEST_58_EXEC:	PUSH AF
	EXX
	LD HL,0000H
	LD BC,08FFH
	OTDR
	EXX
	POP AF
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
DEFB 00H
MEMPTR_TEST_COMMON_EXEC:	PUSH BC
	CALL MEMPTR_CMN_EXEC_START
	POP BC
	OR A
	SBC HL,BC ; Compare Flags checksum with expected value
	JP NZ,TEST_SCREEN_TEST_FAILED
	JP TEST_SCREEN_TEST_PASSED
MEMPTR_CMN_EXEC_START:	DI
	LD DE,MEMPTR_CMN_EXEC_LOOP
	LD BC,0014H
	LDIR
	LD IX,AREA_MEMPTR_TEST_INSERT
	LD BC,2000H
	LD HL,MEMPTR_CMN_EXEC_CHECKSUM
	PUSH HL
	LD A,(VAR_MEMPTR_CHECKSUM)
; --- NOPs below will be replaced by MEMPTR_TEST_nn_EXEC (instruction LDIR at address 9435 above) ---
MEMPTR_CMN_EXEC_LOOP:	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
	NOP
; --- End of replaced code ---
	LD HL,VAR_MEMPTR_CHECKSUM
AREA_MEMPTR_TEST_INSERT:	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	BIT 0,(HL)
	PUSH AF
	DEC SP
	POP AF
	INC SP
	RLA
	RLA
	RLA
	RR E
	CPD
	INC HL
	LD (HL),E
	DEC HL
	RET PO ; ==> jump to MEMPTR_CMN_EXEC_CHECKSUM
	JP (IX) ; ==> loop back to MEMPTR_CMN_EXEC_LOOP
MEMPTR_CMN_EXEC_CHECKSUM:	CALL MEMPTR_CHECKSUM
	EI
	LD A,(VAR_SAVE_A)
	OR A
	JR NZ,MEMPTR_CMN_EXEC_RET
	LD A,20H
MEMPTR_CMN_EXEC_RET:	RET
MEMPTR_CHECKSUM:	LD HL,VAR_MEMPTR_CHECKSUM
	LD DE,0000H
	LD A,(HL)
	PUSH AF
	BIT 0,A
	JR NZ,MEMPTR_CHECKSUM_CASE2
MEMPTR_CHECKSUM_LOOP1:	LD A,08H
MEMPTR_CHECKSUM_LOOP2:	RRC (HL)
	JR C,MEMPTR_CHECKSUM_CASE1
	INC DE
	DEC A
	JR NZ,MEMPTR_CHECKSUM_LOOP2
	DEC HL
	JR MEMPTR_CHECKSUM_LOOP1
MEMPTR_CHECKSUM_CASE1:	DEC DE
	JR MEMPTR_CHECKSUM_CASE4
MEMPTR_CHECKSUM_CASE2:	LD A,08H
MEMPTR_CHECKSUM_LOOP3:	RRC (HL)
	JR NC,MEMPTR_CHECKSUM_CASE3
	INC DE
	DEC A
	JR NZ,MEMPTR_CHECKSUM_LOOP3
	DEC HL
	JR MEMPTR_CHECKSUM_CASE2
MEMPTR_CHECKSUM_CASE3:	DEC DE
MEMPTR_CHECKSUM_CASE4:	POP AF
	BIT 0,A
	RES 5,D
	JR Z,MEMPTR_CHECKSUM_CASE5
	SET 5,D
MEMPTR_CHECKSUM_CASE5:	EX DE,HL
	RET
EXEC_LDIR:	LD A,(VAR_MEMPTR_CHECKSUM)
	LDIR
	POP HL
	JP (HL)
EXEC_LDDR:	LD A,(VAR_MEMPTR_CHECKSUM)
	LDDR
	POP HL
	JP (HL)
EXEC_CPIR:	PUSH AF
	LD A,(VAR_MEMPTR_CHECKSUM)
	CPIR
	POP AF
	POP HL
	JP (HL)
EXEC_CPDR:	PUSH AF
	LD A,(VAR_MEMPTR_CHECKSUM)
	POP AF
	CPDR
	POP HL
	JP (HL)
EXEC_DJNZ_FF:	PUSH BC
	LD A,(VAR_MEMPTR_CHECKSUM)
	LD B,FFH
	DJNZ EXEC_DJNZ_FF_LOOP
EXEC_DJNZ_FF_LOOP:	POP BC
	POP HL
	JP (HL)
EXEC_DJNZ_01:	PUSH BC
	LD A,(VAR_MEMPTR_CHECKSUM)
	LD B,01H
	DJNZ EXEC_DJNZ_01_LOOP
EXEC_DJNZ_01_LOOP:	POP BC
	POP HL
	JP (HL)
VAR_SAVE_A:	DEFB 00H ; Used to restore accumulator in MEMPTR_TEST_COMMON_EXEC
VAR_MEMPTR_CHECKSUM:	DEFB 00H ; Originally address FFFFH
	DEFB 00H
TEST_SCREEN_TEST_FAILED: LD A,FFH
JR ENDNOP
TEST_SCREEN_TEST_PASSED: LD A,0
ENDNOP: NOP