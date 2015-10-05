   0 REM ULA test 3
   0 REM (adapted to 128)
   0 REM by Jan Bobrowski
   0 REM license GPL

  10 LET iport=65535
     LET cport=iport-1
     PRINT AT 1,0;INK 1;"        ULA test 3 by JB"
     LET ft=32768+USR 49152
     PRINT #0;AT 1,8;ft
     FOR a=0 TO 31
     POKE 16384+a,a
     POKE 22528+a,64+a
     NEXT a
     LET start=14329
     IF ft>70000 THEN LET start=14355
     LET t$="":LET v=iport
     GOSUB 2000
     PRINT #0;AT 1,8;"iport:";t$;
     LET t$="":LET v=cport
     GOSUB 2000
     PRINT #0;" cport:";t$

 100 LET t=start
     FOR l=3 TO 21
     PRINT AT l,1;t
     FOR c=8 TO 30 STEP 3
     PRINT AT l,c;
     PRINT #0;AT 1,1;t
     LET v=USR readp
     LET i=USR contp
     LET t$=""
     GO SUB 2100
     PRINT INVERSE i;t$
     LET t=t+1
     NEXT c
     NEXT l
     GO TO 100

2000 LET u=INT(v/4096)
     GO SUB 2200
     LET v=v-4096*u
     LET u=INT(v/256)
     GO SUB 2200
     LET v=v-256*u
2100 LET u=INT(v/16)
     GO SUB 2200
     LET u=v-16*u
2200 LET t$=t$+"0123456789ABCDEF"(u+1)
     RETURN

9000 CLEAR 49151
     LOAD ""CODE
     RUN
