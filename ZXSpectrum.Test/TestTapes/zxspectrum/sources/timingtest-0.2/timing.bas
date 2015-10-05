   0 REM timing test
   0 REM by Patrik Rak
   0 REM based on zxtests
   0 REM by Jan Bobrowski
   0 REM license GPL

  10 LET ftime=32768+USR 40000
  20 PRINT "Frame duration: ";ftime
  30 PRINT AT 2,0;"Available tests:"
  40 PRINT
  50 LET base=14328
  60 IF ftime>70000 THEN LET base=14336

 100 LET i=0
 110 RESTORE
 120 READ a$
 130 IF a$="" THEN GO TO 200
 140 READ n
 150 IF n>=0 THEN GO TO 140
 160 PRINT i;" .. ";a$
 170 LET i=i+1
 180 GO TO 120

 200 INPUT "Choose test: ";i

 300 RESTORE 3000+10*i
 310 READ a$,code,pre,post
 320 CLS
 330 PRINT "Test: ";a$
 340 LET c=code
 350 READ n
 360 IF n<0 THEN GO TO 400
 370 POKE c,n
 380 LET c=c+1
 390 GO TO 350
 400 POKE c,201

1000 LET t=base-pre
1010 FOR l=2 TO 21
1020 PRINT AT l,0;t+pre
1030 FOR c=8 TO 30 STEP 3
1040 PRINT AT l,c;
1050 PRINT #0;AT 1,1;t+pre
1060 LET d=USR test-pre-post
1070 GO SUB 2000
1080 LET t=t+1
1090 NEXT c
1100 NEXT l
1110 GO TO 1000

2000 PRINT "   "(TO 3-LEN STR$ d);d
2010 RETURN

3000 DATA "contended NOP",32767,0,0,0,-1
3010 DATA "snow effect NOP",32768,9+4+7+9,4+9,237,87,8,62,127,237,71,0,8,237,71,-1
3020 DATA "IN #00FE IO",32768,7+7,0,62,0,219,254,-1
3030 DATA "IN #00FF IO",32768,7+7,0,62,0,219,255,-1
3040 DATA "IN #7FFE IO",32768,7+7,0,62,127,219,254,-1
3050 DATA "IN #7FFF IO",32768,7+7,0,62,127,219,255,-1
3060 DATA "IN #FFFE IO",32768,7+7,0,62,255,219,254,-1
3070 DATA "IN #FFFF IO",32768,7+7,0,62,255,219,255,-1
3080 DATA "128k page RET",49152,0,0,-1
4000 DATA ""

5000 INPUT "Choose page (0-7): ";p
5010 OUT 32765,16+p
5020 RUN

6000 INPUT "Choose page to lock (0-7): ";p
6010 OUT 32765,32+16+p
6020 RUN

9000 CLEAR 39999
9010 LOAD "" CODE
9020 RUN