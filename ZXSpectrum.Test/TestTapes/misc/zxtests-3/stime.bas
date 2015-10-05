   0 REM stime
   0 REM by Jan Bobrowski
   0 REM license GPL

  10 LET t=14335
  20 PAPER 6: CLS
  30 PRINT INK 2;" "
  40 PRINT "screen test by Jan Bobrowski"
  50 PRINT "tstate of the WRITE mach. cycle"
  60 PRINT #1;"Q:inc  A:dec"
 100 IF INKEY$="q" THEN LET t=t+1
 110 IF INKEY$="a" THEN LET t=t-1
 120 PRINT AT 3,0; t
 130 RANDOMIZE USR 49152
 140 GO TO 100

9000 CLEAR 49151
9010 LOAD ""CODE
9020 GO TO 10
