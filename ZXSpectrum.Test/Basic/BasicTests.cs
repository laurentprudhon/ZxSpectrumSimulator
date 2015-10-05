using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z80Simulator.Assembly;
using Z80Simulator.Instructions;
using ZXSpectrum.Basic;
using ZXSpectrum.TapeFiles;
using ZXSpectrum.Test.Utils;

namespace ZXSpectrum.Test.Basic
{
    public class BasicTests
    {
        public static void CheckSpectrumNumbers()
        {
            // Examples :
            // Number   Five byte form
            // 0.375    7F 40 00 00 00
            // PI       82 49 0F DA A2
            // –1.5E20  C4 82 1A B0 D4
            // 1000     00 00 E8 03 00
            // –1000    00 FF 18 FC 00

            byte[] fivebytes1 = new byte[] { 0x7F, 0x40, 0x00, 0x00, 0x00 };
            byte[] fivebytes2 = new byte[] { 0x82, 0x49, 0x0F, 0xDA, 0xA2 };
            byte[] fivebytes3 = new byte[] { 0xC4, 0x82, 0x1A, 0xB0, 0xD4 };
            byte[] fivebytes4 = new byte[] { 0x00, 0x00, 0xE8, 0x03, 0x00 };
            byte[] fivebytes5 = new byte[] { 0x00, 0xFF, 0x18, 0xFC, 0x00 };

            SpectrumNumber numberFromBytes1 = new SpectrumNumber(fivebytes1);
            SpectrumNumber numberFromBytes2 = new SpectrumNumber(fivebytes2);
            SpectrumNumber numberFromBytes3 = new SpectrumNumber(fivebytes3);
            SpectrumNumber numberFromBytes4 = new SpectrumNumber(fivebytes4);
            SpectrumNumber numberFromBytes5 = new SpectrumNumber(fivebytes5);

            SpectrumNumber bytesFromNumber1 = new SpectrumNumber(0.375);
            SpectrumNumber bytesFromNumber2 = new SpectrumNumber(3.14159265346825);
            SpectrumNumber bytesFromNumber3 = new SpectrumNumber(-1.5E20);
            SpectrumNumber bytesFromNumber4 = new SpectrumNumber(1000);
            SpectrumNumber bytesFromNumber5 = new SpectrumNumber(-1000);

            SpectrumNumber bytesFromText1 = new SpectrumNumber("0.375");
            SpectrumNumber bytesFromText2 = new SpectrumNumber("3.14159265346825");
            SpectrumNumber bytesFromText3 = new SpectrumNumber("-1.5E20");
            SpectrumNumber bytesFromText4 = new SpectrumNumber("1000");
            SpectrumNumber bytesFromText5 = new SpectrumNumber("-1000");
        }

        public static void CheckBasicText()
        {
            string fileName = "TreasureIsland";
            TapFile tapFile = TapFileReader.ReadTapFile(PlatformSpecific.GetStreamForProjectFile("TapeFiles/SampleTapFiles/" + fileName + ".tap"), fileName);

            ProgramHeader firstHeader = (ProgramHeader)tapFile.DataBlocks[0];
            int programLength = firstHeader.ProgramLength;
            int variablesLength = firstHeader.FollowingDataLength - firstHeader.ProgramLength;

            TapDataBlock programAndVarsBlock = tapFile.DataBlocks[1];
            MemoryStream binaryMemoryStream = new MemoryStream(programAndVarsBlock.TapeData);
            // Read flag
            binaryMemoryStream.ReadByte();

            BasicProgram program = BasicReader.ReadMemoryFormat(fileName, binaryMemoryStream, programLength, variablesLength);

            /*
10 REM Treasure island
100 REM parrot
110 POKE USR "p"+0|0,BIN 00100000|32
120 POKE USR "p"+1|1,BIN 00110110|54
130 POKE USR "p"+2|2,BIN 00111110|62
140 POKE USR "p"+3|3,BIN 00011100|28
150 POKE USR "p"+4|4,BIN 00011110|30
160 POKE USR "p"+5|5,BIN 00100111|39
170 POKE USR "p"+6|6,BIN 01000000|64
180 POKE USR "p"+7|7,BIN 00000000|0
200 REM native
210 POKE USR "n"+0|0,BIN 00000001|1
220 POKE USR "n"+1|1,BIN 00011001|25
230 POKE USR "n"+2|2,BIN 11011001|217
240 POKE USR "n"+3|3,BIN 11111111|255
250 POKE USR "n"+4|4,BIN 11011001|217
260 POKE USR "n"+5|5,BIN 00011001|25
270 POKE USR "n"+6|6,BIN 00100101|37
280 POKE USR "n"+7|7,BIN 00100101|37
300 REM ship sail
310 POKE USR "j"+0|0,BIN 11111111|255
320 POKE USR "j"+1|1,BIN 01111110|126
330 POKE USR "j"+2|2,BIN 10100101|165
340 POKE USR "j"+3|3,BIN 11000011|195
350 POKE USR "j"+4|4,BIN 11000011|195
360 POKE USR "j"+5|5,BIN 10100101|165
370 POKE USR "j"+6|6,BIN 01111110|126
380 POKE USR "j"+7|7,BIN 11111111|255
400 REM ship
410 POKE USR "s"+0|0,BIN 00010000|16
420 POKE USR "s"+1|1,BIN 11111111|255
430 POKE USR "s"+2|2,BIN 11111111|255
440 POKE USR "s"+3|3,BIN 01111110|126
450 POKE USR "s"+4|4,BIN 01111110|126
460 POKE USR "s"+5|5,BIN 01111110|126
470 POKE USR "s"+6|6,BIN 00111100|60
480 POKE USR "s"+7|7,BIN 00111100|60
500 REM quicksand
510 POKE USR "q"+0|0,BIN 00001100|12
520 POKE USR "q"+1|1,BIN 00011100|28
530 POKE USR "q"+2|2,BIN 00111110|62
540 POKE USR "q"+3|3,BIN 01111111|127
550 POKE USR "q"+4|4,BIN 11111111|255
560 POKE USR "q"+5|5,BIN 11111110|254
570 POKE USR "q"+6|6,BIN 01111100|124
580 POKE USR "q"+7|7,BIN 00111000|56
600 REM man
610 POKE USR "m"+0|0,BIN 00011000|24
620 POKE USR "m"+1|1,BIN 00011000|24
630 POKE USR "m"+2|2,BIN 01111110|126
640 POKE USR "m"+3|3,BIN 00011000|24
650 POKE USR "m"+4|4,BIN 00111100|60
660 POKE USR "m"+5|5,BIN 01100110|102
670 POKE USR "m"+6|6,BIN 01100110|102
680 POKE USR "m"+7|7,BIN 00000000|0
810 LET f=0|0
820 LET xs=1|1: LET ys=1|1
830 LET mes=0|0
840 DIM v(10|10)
850 DIM u(10|10)
860 DIM l(20|20)
870 DIM r(20|20)
880 DIM x(20|20)
1050 GO SUB 5000|5000
1060 LET xm=x(t+1|1)
1070 LET ym=t+1|1
1100 GO SUB 3000|3000
1110 PAUSE 50|50+RND *20|20
1150 GO SUB 7000|7000
1200 GO SUB 3570|3570
1210 GO SUB 3500|3500
1270 IF xm=xtAND ym=ytTHEN  GO TO 9000|9000
1280 LET f=f+1|1
1290 IF INT (f/5|5)=f/5|5THEN  GO SUB 6000|6000
1300 IF x(ym)=xmAND INT (f/5|5)=f/5|5THEN  GO TO 1150|1150
1310 IF x(ym)=xmTHEN  GO TO 1210|1210
1330 REM off path
1340 BEEP .3|0.299999999930151,-6|6: GO SUB 6000|6000
1350 FOR q=1|1TO 10|10
1360 REM test for quicksands
1370 IF v(q)=xmAND u(q)=ymTHEN  GO SUB 4000|4000
1380 NEXT q
1400 IF RND <= .4|0.399999999906868THEN  GO TO 2000|2000
1410 REM natives logic
1420 GO SUB 7000|7000
1430 PRINT AT 19|19,1|1; PAPER 8|8; INK 0|0;"HOSTILE NATIVES AHEAD"
1440 FOR n=1|1TO 3|3
1445 LET r=INT (RND *3|3)
1448 IF ym+r>= bTHEN  LET r=0|0
1450 PRINT AT ym+r,xm; PAPER 8|8; INK 0|0;"[UDG-N]"
1460 NEXT n
1470 LET ym=ym-3|3
1480 IF ym<= t+1|1THEN  LET ym=t+1|1
1490 LET xm=x(ym)
1500 PRINT AT ym,xm; PAPER 8|8; INK 0|0;"[UDG-M]"
1550 LET mes=1|1
1560 GO TO 1210|1210
2000 REM parrot logic
2010 GO SUB 7000|7000
2020 GO SUB 3570|3570
2050 PRINT AT 19|19,1|1; PAPER 8|8; INK 0|0;"FOLLOW LONG JOHN SILVERS PARROT"
2100 LET yj=ym+1|1
2110 IF yj>pTHEN  LET yj=p
2120 LET xj=x(yj)
2130 PRINT AT yj,xj; PAPER 8|8; INK 2|2;"[UDG-P]"
2140 LET mes=1|1
2150 GO TO 1210|1210
3000 REM print island
3110 PAPER 7|7: INK 0|0
3120 CLS 
3140 PRINT AT t,l(t);"[BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12]"(1|1TO r(t)-l(t)+1|1)
3160 FOR y=tTO b
3170 PRINT AT y,l(y);"[BG-9]";AT y,r(y);"[BG-6]"
3180 NEXT y
3260 PRINT AT b+1|1,l(b);"[BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12][BG-12]"(1|1TO r(b)-l(b)+1|1)
3300 REM print path
3350 FOR y=t+1|1TO p
3360 PRINT AT y,x(y);"#"
3370 NEXT y
3400 PRINT AT 20|20,1|1;"X MARKS THE SPOT"
3410 PRINT AT p,x(p);"X"
3420 PRINT AT ym,xm;"[UDG-M]"
3440 RETURN 
3460 GO TO 3510|3510
3500 REM move man
3510 BEEP .1|0.0999999999767169,6|6
3515 LET a$=INKEY$ 
3520 IF a$=""THEN  GO TO 3515|3515
3525 PRINT AT ym,xm; PAPER 8|8; INK 0|0;" "
3530 IF a$="h"THEN  GO SUB 8000|8000: RETURN 
3535 IF a$="5"THEN  LET xm=xm-1|1: GO TO 3560|3560
3540 IF a$="8"THEN  LET xm=xm+1|1: GO TO 3560|3560
3550 IF a$<> "6"THEN  GO TO 3510|3510
3560 LET ym=ym+1|1
3570 PRINT AT ym,xm; PAPER 8|8; INK 0|0;"[UDG-M]"
3580 IF mes=0|0THEN  RETURN 
3585 FOR i=0|0TO 31|31
3590 PRINT AT 19|19,i; PAPER 8|8;" "
3595 NEXT i
3598 LET mes=0|0: RETURN 
3600 REM print quicksands
3620 FOR q=1|1TO 10|10
3650 PRINT AT u(q),v(q); INK 4|4; PAPER 8|8;"[UDG-Q]"
3660 NEXT q
3700 PRINT AT 17|17,3|3; INK 0|0; PAPER 8|8;"[UDG-P]";AT 16|16,3|3;"[UDG-N]"
3800 LET xs=xs+1|1
3810 LET ys=ys+1|1
3900 RETURN 
4000 REM quicksand
4010 GO SUB 7000|7000
4020 PRINT AT ym,xm; INK 6|6; PAPER 8|8;"[UDG-Q]"
4100 PRINT AT ym+1|1,xm+1|1; PAPER 8|8; INK 0|0;"AARGH"
4110 FOR i=5|5TO -5|5STEP -1|1
4120 BEEP .1|0.0999999999767169,i
4150 NEXT i
4160 PRINT AT 19|19,1|1; INK 0|0; PAPER 8|8;"IN THE QUICKSAND"
4170 IF RND >0.5|0.499999999883585THEN  PRINT  INK 0|0; PAPER 8|8;"A friendly native pulled you out": PAUSE 100|100: RETURN 
4180 GO TO 9990|9990
5000 REM calculate island
5120 LET l=INT (RND *3|3)+10|10
5130 LET t=INT (RND *3|3)+2|2
5140 LET w=INT (RND *3|3)+10|10
5150 LET b=INT (RND *2|2)+17|17
5230 FOR y=tTO b
5240 LET l(y)=l
5250 LET r(y)=l+w
5340 LET l=l-(SGN (10|10-y)*INT (RND *2|2))
5350 LET w=w+(SGN (10|10-y)*INT (RND *2|2))
5360 IF l(y)>30|30THEN  LET l(y)=30|30
5380 NEXT y
5500 REM path logic
5510 LET x(t+1|1)=l(t+1|1)+INT (RND *4|4)
5520 LET k=t+2|2
5550 FOR p=kTO b-1|1-INT (RND *3|3)
5610 LET x(p)=x(p-1|1)+INT (RND *3|3)-1|1
5620 IF x(p)>= r(p)THEN  LET x(p)=r(p)-1|1
5630 IF x(p)<= l(p)THEN  LET x(p)=l(p)+1|1
5640 NEXT p
5645 LET p=p-1|1
5650 REM treasure
5660 LET xt=x(p)
5670 LET yt=p
5700 REM quicksand logic
5710 FOR q=1|1TO 10|10
5720 LET d=INT (RND *(p-t-2|2))+t+1|1
5750 LET u(q)=d
5760 LET v(q)=x(d)+SGN (RND *.5|0.499999999883585)
5770 IF v(q)<= l(d)THEN  LET v(q)=v(q)+3|3
5780 IF v(q)>= r(d)THEN  LET v(q)=v(q)-3|3
5790 NEXT q
5800 RETURN 
6000 REM priate ship
6010 INK 5|5: PAPER 5|5
6020 CLS 
6030 LET r=INT (RND *3|3)
6100 LET xs=xs+r
6200 LET ys=ys+r
6210 PRINT AT 18|18,18|18; INK 4|4;"[UDG-Q]"
6220 PRINT AT ys,xs; INK 0|0;"[UDG-J]"
6230 PRINT AT ys+1|1,xs; INK 0|0;"[UDG-S]"
6240 PAUSE 50|50
6250 IF ys<18|18THEN  RETURN 
6280 PRINT AT 19|19,1|1; PAPER 8|8; INK 0|0;"THE PIRATES HAVE LANDED"
6290 PRINT  PAPER 8|8; INK 0|0;"YOU AE CAPTURED"
6300 GO TO 9990|9990
7000 REM reprint island
7100 PAPER 5|5: INK 0|0
7110 CLS 
7225 PRINT AT t-1|1,l(t); PAPER 6|6;"                                "(1|1TO r(t)-l(t)+1|1)
7230 FOR y=tTO b
7250 PRINT AT y,l(y)-1|1; PAPER 6|6;" "; PAPER 4|4;"                                "(1|1TO r(y)-l(y)+1|1); PAPER 6|6;" "
7360 NEXT y
7410 PRINT AT y,l(b); PAPER 6|6;"                                "(1|1TO r(b)-l(b)+1|1)
7500 RETURN 
8000 GO SUB 3000|3000
8010 GO SUB 3600|3600
8020 PAUSE 100|100+INT RND *100|100
8030 GO SUB 6000|6000
8040 GO SUB 7000|7000
8050 GO SUB 3570|3570
8060 RETURN 
9000 REM treasure found
9100 FOR c=0|0TO 6|6
9140 BRIGHT 0|0
9150 PAPER c
9160 CLS 
9180 PAUSE 5|5
9200 BRIGHT 1|1
9239 CLS 
9240 BEEP .1|0.0999999999767169,c*2|2
9260 PAUSE 5|5
9270 NEXT c
9300 PRINT AT 10|10,7|7;"YOU FOUND THE GOLD"
9310 BEEP 1|1,14|14
9350 BRIGHT 0|0
9360 PAUSE 20|20
9990 INPUT "Another game (y/n)?";a$
9991 IF a$=""THEN  GO TO 9990|9990
9995 IF a$(1|1)="y"THEN  RUN 
9999 PAPER 7|7: INK 0|0: CLS 

--- PROGRAM VARIABLES ---

#Number# f = 0
#Number# xs = 1
#Number# ys = 1
#Number# mes = 0
#Number# l = 10
#Number# t = 2
#Number# w = 11
#Number# b = 18
#ForLoopControl# y = value:8, limit:17, step:1, loopingline:3350, statementinline:2
#Number# k = 4
#ForLoopControl# p = value:17, limit:17, step:1, loopingline:5550, statementinline:2
#Number# xt = 14
#Number# yt = 17
#ForLoopControl# q = value:11, limit:10, step:1, loopingline:5710, statementinline:2
#Number# d = 12
#Number# xm = 11
#Number# ym = 3
#Number# r = 2
#Number# yj = 15
#Number# xj = 10
#ForLoopControl# i = value:32, limit:31, step:1, loopingline:3585, statementinline:2
#ForLoopControl# n = value:4, limit:3, step:1, loopingline:1440, statementinline:2
#String# a$ = 8
#ForLoopControl# c = value:5, limit:6, step:1, loopingline:9100, statementinline:2
#NumberArray# v = [ 13 14 14 13 14 14 15 14 15 14 ]
#NumberArray# u = [ 6 14 12 5 11 14 15 14 15 12 ]
#NumberArray# l = [ 0 10 10 10 9 8 7 6 5 4 4 4 5 6 6 7 8 9 0 0 ]
#NumberArray# r = [ 0 21 21 21 20 20 20 20 20 19 19 19 19 19 19 20 21 21 0 0 ]
#NumberArray# x = [ 0 0 11 12 12 12 12 12 12 12 13 13 13 13 14 13 14 0 0 0 ]
*/
        }

        public static void DecompileZexall2()
        {
            string fileName = "zexall2";
            TapFile tapFile = TapFileReader.ReadTapFile(PlatformSpecific.GetStreamForProjectFile("TestTapes/" + fileName + ".tap"), fileName);

            ProgramHeader loaderHeader = (ProgramHeader)tapFile.DataBlocks[0];
            int programLength = loaderHeader.ProgramLength;
            int variablesLength = loaderHeader.FollowingDataLength - loaderHeader.ProgramLength;

            TapDataBlock loaderBlock = tapFile.DataBlocks[1];
            MemoryStream binaryMemoryStream = new MemoryStream(loaderBlock.TapeData);
            /* Read flag */
            binaryMemoryStream.ReadByte();
            BasicProgram basicLoader = BasicReader.ReadMemoryFormat(fileName, binaryMemoryStream, programLength, variablesLength);

            ByteArrayHeader codeHeader = (ByteArrayHeader)tapFile.DataBlocks[2];
            int dataLength = codeHeader.DataLength;

            TapDataBlock codeBlock = tapFile.DataBlocks[3];
            binaryMemoryStream = new MemoryStream(codeBlock.TapeData);
            /* Read flag */
            binaryMemoryStream.ReadByte();

            int baseAddress = codeHeader.StartAddress;
            int startAddress = codeHeader.StartAddress;

            SpectrumMemoryMap spectrumMemory = new SpectrumMemoryMap();

            // Load machine code in memory to generate entry points BEFORE dissassembly
            int b = -1;
            int currentAddress = baseAddress;
            long streamPositionBefore = binaryMemoryStream.Position;
            while ((b = binaryMemoryStream.ReadByte()) >= 0)
            {
                spectrumMemory.MemoryCells[currentAddress] = (byte)b;
                spectrumMemory.MemoryCellDescriptions[currentAddress] = null; // Reset previous cell descriptions
                currentAddress++;
            }
            binaryMemoryStream.Position = streamPositionBefore;

            List<CallTarget> entryPoints = new List<CallTarget>();
            entryPoints.Add(new CallTarget(new CallSource(CallSourceType.ExternalEntryPoint, 0, null), startAddress));
            
            Program program = Disassembler.GenerateProgram(fileName, binaryMemoryStream, baseAddress, entryPoints.ToArray(), spectrumMemory);

            program.Lines.Insert(5, Assembler.ParseProgramLine("PRINT_A EQU 0010H"));
            program.Lines.Insert(5, Assembler.ParseProgramLine("CHAN_OPEN EQU 1601H"));
            program.Lines.Insert(5, Assembler.ParseProgramLine(""));
            program.RenumberLinesAfterLineNumber(0);

            program.PrefixLabelToProgramLine(0x8000, "MAIN");
            program.RenameSymbolInProgram("L8013", "START");
            program.PrefixLabelToProgramLine(0x9F69, "MSG1");
            program.RenameSymbolInProgram("L9F41", "BDOS");
            program.PrefixLabelToProgramLine(0x8043, "TESTS");
            program.RenameSymbolInProgram("L802B", "LOOP");
            program.RenameSymbolInProgram("L8038", "DONE");
            program.RenameSymbolInProgram("L9C4F", "STT");
            program.RenameSymbolInProgram("L8040", "STOP");
            program.PrefixLabelToProgramLine(0x9F8B, "MSG2");
            program.PrefixLabelToProgramLine(0x9ED7, "FLGMSK");
            program.PrefixLabelToProgramLine(0x9E49, "COUNTER");
            program.RenameSymbolInProgram("L9DB8", "INITMASK");
            program.PrefixLabelToProgramLine(0x9E71, "SHIFTER");
            program.PrefixLabelToProgramLine(0x9EB3, "IUT");
            program.PrefixLabelToProgramLine(0x8003, "MSBT");
            program.RenameSymbolInProgram("L9FF4", "INITCRC");
            program.RenameSymbolInProgram("L9C94", "TLP");
            program.RenameSymbolInProgram("L9CA8", "TLP1");
            program.RenameSymbolInProgram("L9CAB", "TLP2");
            program.RenameSymbolInProgram("L9E99", "TEST");
            program.RenameSymbolInProgram("L9DF8", "COUNT");
            program.RenameSymbolInProgram("L9E1C", "SHIFT");
            program.RenameSymbolInProgram("L9CE9", "TLP3");
            program.RenameSymbolInProgram("L9FB5", "CMPCRC");
            program.PrefixLabelToProgramLine(0x9F9A, "OKMSG");
            program.RenameSymbolInProgram("L9CE0", "TLPOK");
            program.PrefixLabelToProgramLine(0x9F9F, "ERMSG1");
            program.RenameSymbolInProgram("L9F0C", "PHEX8");
            program.PrefixLabelToProgramLine(0x9FA7, "ERMSG2");
            program.PrefixLabelToProgramLine(0x9FB2, "CRLF");
            program.PrefixLabelToProgramLine(0x9D5F, "CNBIT");
            program.PrefixLabelToProgramLine(0x9D83, "SHFBIT");
            program.PrefixLabelToProgramLine(0x9D60, "CNTBYT");
            program.PrefixLabelToProgramLine(0x9D84, "SHBYT");
            program.RenameSymbolInProgram("L9D13", "SETUP");
            program.RenameSymbolInProgram("L9D1C", "SUBYTE");
            program.RenameSymbolInProgram("L9D3D", "SUBSHF");
            program.RenameSymbolInProgram("L9D2C", "SUBCLP");
            program.RenameSymbolInProgram("L9D62", "NXTCBIT");
            program.RenameSymbolInProgram("L9D58", "SUBSTR");
            program.RenameSymbolInProgram("L9D49", "SBSHF1");
            program.RenameSymbolInProgram("L9D86", "NXTSBIT");
            program.RenameSymbolInProgram("L9D7B", "NCB1");
            program.RenameSymbolInProgram("L9D9F", "NSB1");
            program.RenameSymbolInProgram("L9DA7", "CLRMEM");
            program.RenameSymbolInProgram("L9DC7", "IMLP");
            program.RenameSymbolInProgram("L9DC8", "IMLP1");
            program.RenameSymbolInProgram("L9DCE", "IMLP2");
            program.RenameSymbolInProgram("L9DEB", "IMLP3");
            program.RenameSymbolInProgram("L9E04", "CNTLP");
            program.RenameSymbolInProgram("L9E13", "CNTEND");
            program.RenameSymbolInProgram("L9E17", "CNTLP1");
            program.RenameSymbolInProgram("L9E28", "SHFLP");
            program.RenameSymbolInProgram("L9E3E", "SHFLP2");
            program.RenameSymbolInProgram("L9E40", "SHFLPE");
            program.RenameSymbolInProgram("L9E44", "SHFLP1");
            program.PrefixLabelToProgramLine(0x9F00, "SPSAV");
            program.PrefixLabelToProgramLine(0x8011, "SPBT");
            program.PrefixLabelToProgramLine(0x9EFE, "SPAT");
            program.PrefixLabelToProgramLine(0x9EF0, "MSAT");
            program.PrefixLabelToProgramLine(0x9EFC, "FLGSAT");
            program.PrefixLabelToProgramLine(0xA008, "CRCVAL");
            program.RenameSymbolInProgram("L9EE2", "TCRC");
            program.RenameSymbolInProgram("L9FCC", "UPDCRC");
            program.PrefixLabelToProgramLine(0x9F04, "HEXSTR");
            program.RenameSymbolInProgram("L9F11", "PH8LP");
            program.RenameSymbolInProgram("L9F1E", "PHEX2");
            program.RenameSymbolInProgram("L9F27", "PHEX1");
            program.RenameSymbolInProgram("L9F34", "PH11");
            program.RenameSymbolInProgram("L9F54", "PRCHAR");
            program.RenameSymbolInProgram("L9F5C", "PRSTRING");
            program.RenameSymbolInProgram("L9F4F", "BDOSDONE");
            program.RenameSymbolInProgram("L9FBD", "CCLP");
            program.RenameSymbolInProgram("L9FC8", "CCE");
            program.PrefixLabelToProgramLine(0xA00C, "CRCTAB");
            program.RenameSymbolInProgram("L9FE5", "CRCLP");
            program.RenameSymbolInProgram("L9FFE", "ICRCLP");

            string[] testNames = new string[] { "SCFOP","CCFOP","SCFCCF","CCFSCF","BITA","BITHL","BITX","BITZ80","DAAOP","CPLOP","ADC16","ADD16","ADD16X",
            "ADD16Y","ALU8I","ALU8R","ALU8RX","ALU8X","CPD1","CPI1","INCA","INCB","INCBC","INCC","INCD","INCDE","INCE","INCH","INCHL","INCIX","INCIY",
            "INCL","INCM","INCSP","INCX","INCXH","INCXL","INCYH","INCYL","LD161","LD162","LD163","LD164","LD165","LD166","LD167","LD168","LD16IM","LD16IX",
            "LD8BD","LD8IM","LD8IMX","LD8IX1","LD8IX2","LD8IX3","LD8IXY","LD8RR","LD8RRX","LDA","LDD1","LDD2","LDI1","LDI2","NEGOP","RLDOP","ROT8080",
            "ROTXY","ROTZ80","SRZ80","SRZX","ST8IX1","ST8IX2","ST8IX3","STABD" };

            int testTableAddress = 0x8043;
            foreach(string testName in testNames)
            {
                int testAddress = spectrumMemory.MemoryCells[testTableAddress++] + 256 * spectrumMemory.MemoryCells[testTableAddress++];
                program.PrefixLabelToProgramLine(testAddress, testName);
            }

            MemoryMap testMemoryMap = new MemoryMap(65536);
            program.Variables.Clear();
            Assembler.CompileProgram(program, 0x8000, testMemoryMap);
            for (int programAddress = program.BaseAddress; programAddress <= program.MaxAddress; programAddress++)
            {
                if (testMemoryMap.MemoryCells[programAddress] != spectrumMemory.MemoryCells[programAddress])
                {
                    throw new Exception("Error in decompiled program at address " + programAddress + " : source byte = " + spectrumMemory.MemoryCells[programAddress] + ", decompiled program line + " + ((ProgramLine)spectrumMemory.MemoryCellDescriptions[programAddress].Description).Text + " produced byte = " + testMemoryMap.MemoryCells[programAddress]);
                }
            }

            string htmlProgram = AsmHtmlFormatter.BeautifyAsmCode(fileName, program);
        }


        public static void DecompileZ80tests()
        {
            string fileName = "z80tests";
            TapFile tapFile = TapFileReader.ReadTapFile(PlatformSpecific.GetStreamForProjectFile("TestTapes/" + fileName + ".tap"), fileName);
            
            ProgramHeader loaderHeader = (ProgramHeader)tapFile.DataBlocks[0];
            int programLength = loaderHeader.ProgramLength;
            int variablesLength = loaderHeader.FollowingDataLength - loaderHeader.ProgramLength;

            TapDataBlock loaderBlock = tapFile.DataBlocks[1];
            MemoryStream binaryMemoryStream = new MemoryStream(loaderBlock.TapeData);
            /* Read flag */ binaryMemoryStream.ReadByte();
            BasicProgram basicLoader = BasicReader.ReadMemoryFormat(fileName, binaryMemoryStream, programLength, variablesLength);

            ByteArrayHeader codeHeader = (ByteArrayHeader)tapFile.DataBlocks[2];
            int dataLength = codeHeader.DataLength;

            TapDataBlock codeBlock = tapFile.DataBlocks[3];
            binaryMemoryStream = new MemoryStream(codeBlock.TapeData);
            /* Read flag */ binaryMemoryStream.ReadByte();

            int baseAddress = codeHeader.StartAddress;
            int startAddress = codeHeader.StartAddress;

            SpectrumMemoryMap spectrumMemory = new SpectrumMemoryMap();

            // Load machine code in memory to generate entry points BEFORE dissassembly
            int b = -1;
            int currentAddress = baseAddress;
            long streamPositionBefore = binaryMemoryStream.Position;
            while ((b = binaryMemoryStream.ReadByte()) >= 0)
            {
                spectrumMemory.MemoryCells[currentAddress] = (byte)b;
                spectrumMemory.MemoryCellDescriptions[currentAddress] = null; // Reset previous cell descriptions
                currentAddress++;
            }
            binaryMemoryStream.Position = streamPositionBefore;

            List<CallTarget> entryPoints = new List<CallTarget>();
            entryPoints.Add(new CallTarget(new CallSource(CallSourceType.ExternalEntryPoint, 0, null), startAddress));
            entryPoints.Add(new CallTarget(new CallSource(CallSourceType.CallInstruction, 0, null), 0x80B8));
            entryPoints.Add(new CallTarget(new CallSource(CallSourceType.CallInstruction, 0, null), 0x80C0));
            entryPoints.Add(new CallTarget(new CallSource(CallSourceType.JumpInstruction, 0x94CE, null), 0x94D1));
            ReadEntryPointsInTestsParametersTable(0x822B, spectrumMemory, entryPoints);
            ReadEntryPointsInTestsParametersTable(0x8407, spectrumMemory, entryPoints);    
                        
            Program program = Disassembler.GenerateProgram(fileName, binaryMemoryStream, baseAddress, entryPoints.ToArray(), spectrumMemory);

            foreach (ProgramLine line in program.Lines)
            {
                if (line.Type == ProgramLineType.OpCodeInstruction)
                {
                    TryReplaceAddressWithSystemVariables(spectrumMemory, program, line, 0);
                    TryReplaceAddressWithSystemVariables(spectrumMemory, program, line, 1);
                }
            }

            program.Lines.Insert(5, Assembler.ParseProgramLine("VAR_MEMPTR_CHECKSUM EQU FFFFH"));
            program.Lines.Insert(5, Assembler.ParseProgramLine("LAST_K EQU 5C08H"));
            program.Lines.Insert(5, Assembler.ParseProgramLine("CHAN_OPEN EQU 1601H"));
            program.Lines.Insert(5, Assembler.ParseProgramLine("CLS EQU 0D6BH"));            
            program.Lines.Insert(5, Assembler.ParseProgramLine("START EQU 0000H"));
            program.Lines.Insert(5, Assembler.ParseProgramLine(""));
            program.RenumberLinesAfterLineNumber(0);

            program.PrefixLabelToProgramLine(0x8000, "MAIN"); 
            program.RenameSymbolInProgram("L8003", "EXEC_LDIR"); 
            program.RenameSymbolInProgram("L800A", "EXEC_LDDR"); 
            program.RenameSymbolInProgram("L8011", "EXEC_CPIR"); 
            program.RenameSymbolInProgram("L801A", "EXEC_CPDR"); 
            program.RenameSymbolInProgram("L8023", "EXEC_DJNZ_FF"); 
            program.RenameSymbolInProgram("L802B", "EXEC_DJNZ_FF_LOOP");
            program.RenameSymbolInProgram("L802E", "EXEC_DJNZ_01"); 
            program.RenameSymbolInProgram("L8036", "EXEC_DJNZ_01_LOOP"); 
            program.RenameSymbolInProgram("L8039", "MENU_SCREEN");
            program.RenameSymbolInProgram("L8042", "MENU_SCREEN_INPUT");
            program.RenameSymbolInProgram("L80DA", "INIT_SCREEN");
            program.AppendCommentToProgramLine(0x803C, "Menu screen string start address");
            program.InsertCommentAboveProgramLine(0x8114, "--- Menu screen string ---");
            CharDecoder decoder = new CharDecoder();
            program.CommentStringChars(0x8114, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
            program.AppendCommentToProgramLine(0x8045, "If Key 1 was pressed");
            program.RenameSymbolInProgram("L8057", "FLAGS_TESTS");
            program.AppendCommentToProgramLine(0x8049, "If Key 2 was pressed");
            program.RenameSymbolInProgram("L805F", "MEMPTR_TESTS");
            program.AppendCommentToProgramLine(0x804D, "If Key 3 was pressed");
            program.InsertCommentAboveProgramLine(0x8057, "=> exit to BASIC");
            program.AppendCommentToProgramLine(0x80DA, "Clear the display");
            program.AppendCommentToProgramLine(0x80DF, "Open a channel : user stream 02");
            program.InsertCommentAboveProgramLine(0x80E2, "=> return to MENU_SCREEN");
            program.RenameSymbolInProgram("L80E2", "WAIT_KEYPRESS");
            program.RenameSymbolInProgram("L80E6", "WAIT_KEYPRESS_LOOP");
            program.RenameSymbolInProgram("L80ED", "DISPLAY_STRING");
            program.RenameSymbolInProgram("L80D2", "SETIY_DISPLAY_STRING"); 
            program.RenameSymbolInProgram("L80F5", "DISPLAY_TWO_HEXBYTES");
            program.RenameSymbolInProgram("L80FE", "DISPLAY_TWO_HEXCHARS");                
            program.RenameSymbolInProgram("L810B", "DISPLAY_ONE_HEXCHAR");
            program.InsertCommentAboveProgramLine(0x80ED, "Display all chars starting from address DE upwards, until char FF");
            program.RenameSymbolInProgram("L8072", "TEST_SCREEN");
            program.RenameSymbolInProgram("L807D", "TEST_SCREEN_LAUNCH_TEST");
            program.RenameSymbolInProgram("L80AF", "TEST_SCREEN_NEXT_TEST");
            program.PrefixLabelToProgramLine(0x80B8, "TEST_SCREEN_TEST_PASSED", true);
            Program.ReplaceAddressWithSymbolInProgramLine(program.GetLineFromAddress(0x80A6), 1, false, "TEST_SCREEN_TEST_PASSED");
            program.PrefixLabelToProgramLine(0x80C0, "TEST_SCREEN_TEST_FAILED", true);
            Program.ReplaceAddressWithSymbolInProgramLine(program.GetLineFromAddress(0x80AA), 1, false, "TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x805A, "Flags test screen string start address");
            program.InsertCommentAboveProgramLine(0x81CD, "--- Flags test screen string ---");
            program.CommentStringChars(0x81CD, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
            program.AppendCommentToProgramLine(0x8057, "Flags tests parameter table start address");
            program.InsertCommentAboveProgramLine(0x822B, "--- Flags tests parameter table ---");
            program.AppendCommentToProgramLine(0x8062, "MEMPTR test screen string start address");
            program.InsertCommentAboveProgramLine(0x81EA, "--- MEMPTR test screen string ---");
            program.CommentStringChars(0x81EA, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
            program.AppendCommentToProgramLine(0x805F, "MEMPTR tests parameter table start address");
            program.InsertCommentAboveProgramLine(0x8407, "--- MEMPTR tests parameter table ---");
            program.RenameSymbolInProgram("L8067", "END_OF_TESTS");
            program.AppendCommentToProgramLine(0x8067, "End of tests string start address");
            program.InsertCommentAboveProgramLine(0x81AB, "--- End of tests string ---");
            program.CommentStringChars(0x81AB, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
            program.AppendCommentToProgramLine(0x8226, "Space reserved to save HL in routine TEST_SCREEN");
            program.AppendCommentToProgramLine(0x8227, "Space reserved to save HL in routine TEST_SCREEN");
            program.AppendCommentToProgramLine(0x8228, "Space reserved to save SP in routine TEST_SCREEN");
            program.AppendCommentToProgramLine(0x8229, "Space reserved to save SP in routine TEST_SCREEN");
            program.InsertCommentAboveProgramLine(0x8085, "Here HL points to an entry in the tests parameters table");
            program.InsertCommentAboveProgramLine(0x8088, "Test params structure 0 - 1 : Pointer to an executable machine code function : the test launcher. 00H|00H marks the end of the parameters table.");
            program.InsertCommentAboveProgramLine(0x8088, "Test params structure 2 -> FFH : starting with the third byte, the parameter table entry contains a string describing the test");
            program.AppendCommentToProgramLine(0x8091, "Display tested opcode name");
            program.AppendCommentToProgramLine(0x8094, "TAB");
            program.AppendCommentToProgramLine(0x8097, "BRIGHT ...");
            program.AppendCommentToProgramLine(0x809A, "... 0");
            program.AppendCommentToProgramLine(0x809C, ":");
            program.AppendCommentToProgramLine(0x809F, "space");
            program.InsertCommentAboveProgramLine(0x80AF, "=> jump to test launcher, start address found at the beginning the current parameter table entry");
            program.RenameSymbolInProgram("L892D", "FLAGS_TEST_COMMON_EXEC");
            program.RenameSymbolInProgram("L9423", "MEMPTR_TEST_COMMON_EXEC");
            program.RenameSymbolInProgram("L89B9", "FLAGS_TEST_REPEAT_EXEC");
            program.RenameSymbolInProgram("L8A0B", "FLAGS_TEST_DDCB_EXEC");
            program.RenameSymbolInProgram("L8A6D", "FLAGS_TEST_FDCB_EXEC");
            program.RenameSymbolInProgram("L8B03", "FLAGS_TEST_CB_EXEC");
            program.CommentStringChars(0x8208, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
            program.CommentStringChars(0x820F, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
            program.AppendCommentToProgramLine(0x8083, "Set white border around the screen");
            program.AppendCommentToProgramLine(0x808C, "Here : HL points to the address of the test laucher routine, DE points to the string describing the test");
            program.AppendCommentToProgramLine(0x808F, "If HL = 00 : end of the tests table, else :");
            program.AppendCommentToProgramLine(0x80A2, "After the end of the string, save the address of the next entry in tests table for next iteration");
            program.InsertCommentAboveProgramLine(0x8208, "--- Passed test string ---");
            program.InsertCommentAboveProgramLine(0x820F, "--- Failed test string ---");
            program.InsertCommentAboveProgramLine(0x892D, "Generic execution environment for flags test");
            program.RenameSymbolInProgram("L9520", "ADD_FLAGS_TO_CHECKSUM");
            program.AppendCommentToProgramLine(0x954A, "Flags Checksum value");
            program.AppendCommentToProgramLine(0x892F, "Replaces instruction CALL START below with CALL (test EXEC address)");
            program.InsertCommentAboveProgramLine(0x894E, "==> call to one of the FLAGS_TEST_nn_EXEC routine");
            program.AppendCommentToProgramLine(0x895C, "Display Flags checksum value");
            program.AppendCommentToProgramLine(0x8964, "Compare Flags checksum with expected value");
            program.AppendCommentToProgramLine(0x8966, "==> return to TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x8968, "==> return to TEST_SCREEN_TEST_PASSED");
            program.RenameSymbolInProgram("L8949", "FLAGS_TEST_CMN_EXEC_AF_VAL");
            program.AppendCommentToProgramLine(0x89BA, "Replaces instruction LDI below with one of the repeated instructions from the test table");
            program.InsertCommentAboveProgramLine(0x89D1, "==> executes one of the repeated instructions from the test table");
            program.RenameSymbolInProgram("L89C3", "FLAGS_TEST_REPEAT_EXEC_EXT_LOOP");
            program.RenameSymbolInProgram("L89CD", "FLAGS_TEST_REPEAT_EXEC_INN_LOOP");
            program.AppendCommentToProgramLine(0x89E1, "Display Flags checksum value");
            program.AppendCommentToProgramLine(0x89E9, "Compare Flags checksum with expected value");
            program.AppendCommentToProgramLine(0x89EB, "==> return to TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x89ED, "==> return to TEST_SCREEN_TEST_PASSED");
            program.RenameSymbolInProgram("L8A1C", "FLAGS_TEST_DDCB_EXEC_LOOP");
            program.AppendCommentToProgramLine(0x8A1F, "<= third byte of opcode replaced with all values between 00 and FF by the line above");
            program.AppendCommentToProgramLine(0x8A45, "Display Flags checksum value");
            program.AppendCommentToProgramLine(0x8A4D, "Compare Flags checksum with expected value");
            program.AppendCommentToProgramLine(0x8A4F, "==> return to TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x8A51, "==> return to TEST_SCREEN_TEST_PASSED");
            program.RenameSymbolInProgram("L8A81", "FLAGS_TEST_FDCB_EXEC_LOOP");
            program.AppendCommentToProgramLine(0x8A84, "<= third byte of opcode replaced with all values between 00 and FF by the line above");
            program.AppendCommentToProgramLine(0x8AAA, "Display Flags checksum value");
            program.AppendCommentToProgramLine(0x8AB2, "Compare Flags checksum with expected value");
            program.AppendCommentToProgramLine(0x8AB4, "==> return to TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x8AB6, "==> return to TEST_SCREEN_TEST_PASSED");
            program.RenameSymbolInProgram("L8B14", "FLAGS_TEST_CB_EXEC_LOOP");
            program.RenameSymbolInProgram("L8B2A", "FLAGS_TEST_CB_EXEC_CASE1");
            program.RenameSymbolInProgram("L8B2C", "FLAGS_TEST_CB_EXEC_CASE2");
            program.RenameSymbolInProgram("L8B3A", "FLAGS_TEST_CB_EXEC_CASE3");
            program.AppendCommentToProgramLine(0x8B24, "<= third byte of opcode replaced with all values between 00 and FF by the line above at 8B17");
            program.AppendCommentToProgramLine(0x8B2A, "<= third byte of opcode replaced with all values between 00 and FF by the line above at 8B14");
            program.AppendCommentToProgramLine(0x8B51, "Display Flags checksum value");
            program.AppendCommentToProgramLine(0x8B59, "Compare Flags checksum with expected value");
            program.AppendCommentToProgramLine(0x8B5B, "==> return to TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x8B5D, "==> return to TEST_SCREEN_TEST_PASSED");
            program.RenameSymbolInProgram("L942E", "MEMPTR_CMN_EXEC_START");
            program.AppendCommentToProgramLine(0x94CF, "==> loop back to MEMPTR_CMN_EXEC_LOOP");
            program.AppendCommentToProgramLine(0x94CE, "==> jump to MEMPTR_CMN_EXEC_CHECKSUM");
            program.PrefixLabelToProgramLine(0x94D1, "MEMPTR_CMN_EXEC_CHECKSUM", true);
            program.RenameSymbolInProgram("L94E1", "MEMPTR_CMN_EXEC_RET");
            program.AppendCommentToProgramLine(0x9429, "Compare Flags checksum with expected value");
            program.AppendCommentToProgramLine(0x942B, "==> return to TEST_SCREEN_TEST_FAILED");
            program.AppendCommentToProgramLine(0x942D, "==> return to TEST_SCREEN_TEST_PASSED");
            program.InsertCommentAboveProgramLine(0x9445, "--- NOPs below will be replaced by MEMPTR_TEST_nn_EXEC (instruction LDIR at address 9435 above) ---");
            program.InsertCommentAboveProgramLine(0x9459, "--- End of replaced code ---");
            program.AppendCommentToProgramLine(0x94DB, "Display Flags checksum value");
            program.RenameSymbolInProgram("L94E2", "MEMPTR_CHECKSUM");
            program.RenameSymbolInProgram("L94EE", "MEMPTR_CHECKSUM_LOOP1");
            program.RenameSymbolInProgram("L94F0", "MEMPTR_CHECKSUM_LOOP2");
            program.RenameSymbolInProgram("L94FB", "MEMPTR_CHECKSUM_CASE1");
            program.RenameSymbolInProgram("L94FE", "MEMPTR_CHECKSUM_CASE2");
            program.RenameSymbolInProgram("L9500", "MEMPTR_CHECKSUM_LOOP3");
            program.RenameSymbolInProgram("L950B", "MEMPTR_CHECKSUM_CASE3");
            program.RenameSymbolInProgram("L950C", "MEMPTR_CHECKSUM_CASE4");
            program.RenameSymbolInProgram("L9515", "MEMPTR_CHECKSUM_CASE5");
            program.RenameSymbolInProgram("L9517", "RESET_FLAGS_CHECKSUM");
            program.AppendCommentToProgramLine(0x8B5E, "Used to save and restore accumulator in FLAGS_TEST_CB_EXEC");
            program.AppendCommentToProgramLine(0x822A, "Used to restore accumulator in MEMPTR_TEST_COMMON_EXEC");
            program.AppendCommentToProgramLine(0x954B, "Not used ?");
            program.AppendCommentToProgramLine(0x954C, "Not used ?");
            program.PrefixLabelToProgramLine(0x954A, "VAR_FLAGS_CHECKSUM", true);
            program.ReplaceAddressWithSymbol(0xFFFF, "VAR_MEMPTR_CHECKSUM", true);
            program.PrefixLabelToProgramLine(0x8226, "VAR_SAVE_HL", true);
            program.PrefixLabelToProgramLine(0x8228, "VAR_SAVE_SP", true);
            program.PrefixLabelToProgramLine(0x822A, "VAR_SAVE_A", true);
            program.PrefixLabelToProgramLine(0x8114, "STRING_MENU_SCREEN", true);
            program.PrefixLabelToProgramLine(0x81AB, "STRING_END_OF_TESTS", true);
            program.PrefixLabelToProgramLine(0x81CD, "STRING_FLAGS_TESTS_SCREEN", true);
            program.PrefixLabelToProgramLine(0x81EA, "STRING_MEMPTR_TESTS_SCREEN", true);
            program.PrefixLabelToProgramLine(0x8208, "STRING_PASSED_TEST", true);
            program.PrefixLabelToProgramLine(0x820F, "STRING_FAILED_TEST", true);
            program.PrefixLabelToProgramLine(0x822B, "TABLE_FLAGS_TESTS", true);
            program.PrefixLabelToProgramLine(0x8407, "TABLE_MEMPTR_TESTS", true);
            program.PrefixLabelToProgramLine(0x8B5E, "VAR_SAVE_A2", true);
            program.PrefixLabelToProgramLine(0x9445, "MEMPTR_CMN_EXEC_LOOP", true);
            program.PrefixLabelToProgramLine(0x945C, "AREA_MEMPTR_TEST_INSERT", true);

            AddCommentsInTestsParametersTable(0x822B, spectrumMemory, decoder, program);
            AddCommentsInTestsParametersTable(0x8407, spectrumMemory, decoder, program);    

            MemoryMap testMemoryMap = new MemoryMap(65536);
            program.Variables.Clear();
            Assembler.CompileProgram(program, 0x8000, testMemoryMap);
            for (int programAddress = program.BaseAddress; programAddress <= program.MaxAddress; programAddress++)
            {
                if (testMemoryMap.MemoryCells[programAddress] != spectrumMemory.MemoryCells[programAddress])
                {
                    throw new Exception("Error in decompiled program at address " + programAddress + " : source byte = " + spectrumMemory.MemoryCells[programAddress] + ", decompiled program line + " + ((ProgramLine)spectrumMemory.MemoryCellDescriptions[programAddress].Description).Text + " produced byte = " + testMemoryMap.MemoryCells[programAddress]);
                }
            }

            string htmlProgram  = AsmHtmlFormatter.BeautifyAsmCode(fileName, program);
        }

        private static void AddCommentsInTestsParametersTable(int address, SpectrumMemoryMap memory, CharDecoder decoder, Program program)
        {
            bool isFlagTests = address == 0x822B;
            string testType = isFlagTests ? "FLAGS" : "MEMPTR";
            int testCounter = 0;
            int paramAddress = address;
            int targetAddress = 0;
            while ((targetAddress = memory.MemoryCells[address++] + memory.MemoryCells[address++] * 256) > 0)
            {
                program.DefineWordData(address - 2);
                program.CommentStringChars(address, decoder.DecodeChar, Program.StringTerminationType.SpecialChar, 0, 0xFF);
                StringBuilder testName = new StringBuilder();
                for (; memory.MemoryCells[address] != 0xFF; address++)
                {
                    testName.Append(SpectrumCharSet.GetSpectrumChar(memory.MemoryCells[address]).Text);
                }

                testCounter++;
                string testDescription = testType + "  test " + testCounter.ToString("D2") + " : " + testName.ToString();
                string testLabel = testType + "_TEST_" + testCounter.ToString("D2");
                program.InsertCommentAboveProgramLine(paramAddress, "Params for " + testDescription);
                program.InsertCommentAboveProgramLine(targetAddress, "Launcher for " + testDescription);
                program.PrefixLabelToProgramLine(targetAddress, testLabel + "_LAUNCH", true);
                if ((isFlagTests && (testCounter <= 29)) || !isFlagTests)
                {
                    program.InsertCommentAboveProgramLine(targetAddress + 9, "Code executed for " + testDescription);
                    program.PrefixLabelToProgramLine(targetAddress + 9, testLabel + "_EXEC", true);
                }
                address++;
                paramAddress = address;
            }
        }

        private static void ReadEntryPointsInTestsParametersTable(int address, SpectrumMemoryMap memory, List<CallTarget> entryPoints)
        {
            bool isFlagTests = address == 0x822B;
            int targetAddress = 0;
            int counter = 0;
            while ((targetAddress = memory.MemoryCells[address++] + memory.MemoryCells[address++] * 256) > 0)
            {
                entryPoints.Add(new CallTarget(new CallSource(CallSourceType.CallInstruction, 0x80AE, null), targetAddress));
                if (isFlagTests && (counter < 29))
                {
                    entryPoints.Add(new CallTarget(new CallSource(CallSourceType.CallInstruction, 0x894B, null), targetAddress + 9));
                    counter++;
                }
                else if(!isFlagTests)
                {
                    entryPoints.Add(new CallTarget(new CallSource(CallSourceType.CodeRelocation, 0x9445, null) { CodeRelocationBytesCount = 20 }, targetAddress + 9));
                    counter++;
                }
                
                for (; memory.MemoryCells[address] != 0xFF; address++)
                { }
                address++;
            }
        }

        private static void TryReplaceAddressWithSystemVariables(SpectrumMemoryMap spectrumMemory, Program program, ProgramLine line, int paramIndex)
        {
            if ((paramIndex == 0 && line.InstructionType.Param1Type == AddressingMode.Extended) ||
                (paramIndex == 1 && line.InstructionType.Param2Type == AddressingMode.Extended))
            {
                NumberOperand operand = line.OpCodeParameters[paramIndex].NumberExpression as NumberOperand;
                if (operand != null)
                {
                    int address = operand.GetValue(null, line);
                    if (address >= 23552 && address <= 23733)
                    {
                        SpectrumSystemVariable systemVariable = spectrumMemory.MemoryCellDescriptions[address].Description as SpectrumSystemVariable;
                        if (systemVariable != null)
                        {
                            if (!program.Variables.ContainsKey(systemVariable.Name))
                            {
                                program.Variables.Add(systemVariable.Name, new NumberOperand(address));
                            }
                            Program.ReplaceAddressWithSymbolInProgramLine(line, paramIndex, false, systemVariable.Name);
                            program.AppendCommentToProgramLine(line.LineAddress, systemVariable.Name + " : " + systemVariable.Description);
                        }
                    }
                }
            }
        }

        class CharDecoder
        {
            private int atCounter = 0;

            public string DecodeChar(byte charCode)
            {
                if (atCounter == 0)
                {
                    // AT control code - 2 bytes following : line / column 
                    if (charCode == 22)
                    {
                        atCounter = 2;
                    }
                    return SpectrumCharSet.GetSpectrumChar(charCode).Text;
                }
                else
                {
                    atCounter--;
                    return charCode.ToString();
                }                
            }
        }
    }
}
