using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.Basic
{
    public class BasicReader
    {
        /// <summary>
        /// Source : ZXBasicManual/zxmanchap24.html
        /// </summary>
        public static BasicProgram ReadMemoryFormat(string sourcePath, Stream binaryMemoryStream, int programLength, int variablesLength)
        {
            BasicProgram program = new BasicProgram(sourcePath);

            ReadProgramLines(binaryMemoryStream, programLength, program);
            ReadProgramVariables(binaryMemoryStream, variablesLength, program);

            return program;
        }
        
        // Each line of BASIC program has the form :
        // - 2 bytes | Line number : more significant byte, less significant byte
        // - 2 bytes | Lenght of text + ENTER : less significant byte, more significant byte
        // - Text
        // - ENTER = 00001101 (13)
        private static void ReadProgramLines(Stream binaryMemoryStream, int programLength, BasicProgram program)
        {
            int programBytesCounter = 0;
            while (programBytesCounter < programLength)
            {
                int lineNumber = ReadLineNumber(binaryMemoryStream, ref programBytesCounter);
                int lengthOfTextPlusEnter = ReadInteger(binaryMemoryStream, ref programBytesCounter);

                BasicLine line = new BasicLine(lineNumber, lengthOfTextPlusEnter);

                for (int i = 0; i < lengthOfTextPlusEnter; i++)
                {
                    SpectrumChar spectrumChar = ReadSpectrumChar(binaryMemoryStream, ref programBytesCounter);

                    // A numerical constant in the program is followed by its binary form, 
                    // using the character CHR$ 14 followed by five bytes for the number itself. 
                    if (spectrumChar.Code == 14)
                    {
                        SpectrumNumber spectrumNumber = ReadSpectrumNumber(binaryMemoryStream, ref programBytesCounter);
                        i += 5;
                        spectrumChar = spectrumChar.CloneForNumber(spectrumNumber);
                    }

                    line.SpectrumChars[i] = spectrumChar;
                }

                // Check : a basic line should end with Enter char
                if (line.SpectrumChars[line.SpectrumChars.Length - 1].Code != 13)
                {
                    throw new Exception("Basic line does not end with enter char");
                }
                else
                {
                    program.Lines.Add(line);
                }
            }
        }               
        
        // The variables have different formats according to their different features. 
        // The letters in the names should be imagined as starting off in lower case.
        private static void ReadProgramVariables(Stream binaryMemoryStream, int variablesLength, BasicProgram program)
        {
            int variablesBytesCounter = 0;
            while(variablesBytesCounter < variablesLength)
            {
                byte varTypeAndFirstLetter = ReadByte(binaryMemoryStream, ref variablesBytesCounter);
                byte varType = (byte)(varTypeAndFirstLetter & 0xE0);
                byte firstLetterCode = (byte)((varTypeAndFirstLetter & 0x1F) + 0x60);
                
                string name = SpectrumCharSet.GetSpectrumChar(firstLetterCode).Text;

                switch (varType)
                {
                    // Number with one letter name
                    // - 0 1 1 _ _ _ _ _ : letter - 60H
                    // - 5 bytes         : value
                    case 96:
                        NumberVariable numVariable = new NumberVariable() { Name = name };
                        numVariable.Value = ReadSpectrumNumber(binaryMemoryStream, ref variablesBytesCounter);
                        program.Variables.Add(numVariable);
                        break;
                    // Number whose name is longer than one letter
                    // - 1 0 1 _ _ _ _ _ : letter - 60H
                    // - 0 _ _ _ _ _ _ _ : 2nd character
                    //    ...
                    // - 1 _ _ _ _ _ _ _ : last character
                    // - 5 bytes         : value
                    case 160:
                        byte charCode;
                        while(((charCode = ReadByte(binaryMemoryStream, ref variablesBytesCounter)) & 0x80) == 0)
                        {
                            name += SpectrumCharSet.GetSpectrumChar(charCode).Text;
                        }
                        name += SpectrumCharSet.GetSpectrumChar((byte)(charCode-128)).Text;
                        numVariable = new NumberVariable() { Name = name };
                        numVariable.Value = ReadSpectrumNumber(binaryMemoryStream, ref variablesBytesCounter);
                        program.Variables.Add(numVariable);                        
                        break;
                    // Array of numbers
                    // - 1 0 0 _ _ _ _ _ : letter - 60H
                    // - 2 bytes         : total lenght of elements & dimensions + 1 for no. of dimensions
                    // - 1 byte          : no. of dimensions
                    // - 2 bytes         : first dimension
                    //    ...
                    // - 2 bytes         : last dimension
                    // - 5 bytes each    : values
                    // The order of the elements is: 
                    // first, the elements for which the first subscript is 1;
                    // next, the elements for which the first subscript is 2;
                    // next, the elements for which the first subscript is 3;
                    // and so on for all possible values of the first subscript.
                    // The elements with a given first subscript are ordered in the same way using the second subscript, and so on down to the last. 
                    // As an example, the elements of the 3*6 array b in Chapter 12 are stored in the order b(1,1) b(1,2) b(1,3) b(1,4) b(1,5) b(1,6) b(2,1) b(2,2) .... b(2,6) b(3,1) b(3,2) ... b(3,6). 
                    case 128:
                        NumberArrayVariable numArrayVariable = new NumberArrayVariable() { Name = name };
                            ReadInteger(binaryMemoryStream, ref variablesBytesCounter);
                        numArrayVariable.NumberOfDimensions = ReadByte(binaryMemoryStream, ref variablesBytesCounter);
                        numArrayVariable.DimensionsSizes = new int[numArrayVariable.NumberOfDimensions];
                        int valuesCount = 1;
                        for (int i = 0; i < numArrayVariable.NumberOfDimensions ; i++)
                        {
                            numArrayVariable.DimensionsSizes[i] = ReadInteger(binaryMemoryStream, ref variablesBytesCounter);
                            valuesCount *= numArrayVariable.DimensionsSizes[i];
                        }
                        for (int i = 0; i < valuesCount; i++)
                        {
                            SpectrumNumber value = ReadSpectrumNumber(binaryMemoryStream, ref variablesBytesCounter);
                            numArrayVariable.Values.Add(value);
                        }
                        program.Variables.Add(numArrayVariable); 
                        break;
                    // Control variable of a FOR-NEXT loop
                    // - 1 1 1 _ _ _ _ _ : letter - 60H
                    // - 5 bytes         : value
                    // - 5 bytes         : limit
                    // - 5 bytes         : step
                    // - 2 bytes         : looping line, lsb | msb
                    // - 1 byte          : statement number whitin line
                    case 224:
                        ForLoopControlVariable forVariable = new ForLoopControlVariable() { Name = name };
                        forVariable.Value = ReadSpectrumNumber(binaryMemoryStream, ref variablesBytesCounter);
                        forVariable.Limit = ReadSpectrumNumber(binaryMemoryStream, ref variablesBytesCounter);
                        forVariable.Step = ReadSpectrumNumber(binaryMemoryStream, ref variablesBytesCounter);
                        forVariable.LoopingLine = ReadInteger(binaryMemoryStream, ref variablesBytesCounter);
                        forVariable.StatementNumberWithinLine = ReadByte(binaryMemoryStream, ref variablesBytesCounter);
                        program.Variables.Add(forVariable);        
                        break;
                    // String
                    // - 0 1 0 _ _ _ _ _ : letter - 60H
                    // - 2 bytes         : number of characters
                    // - text of string (may be empty)
                    case 64:
                        StringVariable strVariable = new StringVariable() { Name = name + "$" };
                        strVariable.CharsCount = ReadInteger(binaryMemoryStream, ref variablesBytesCounter);
                        strVariable.Chars = new SpectrumChar[strVariable.CharsCount];
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < strVariable.CharsCount; i++)
                        {
                            strVariable.Chars[i] = ReadSpectrumChar(binaryMemoryStream, ref variablesBytesCounter);
                            sb.Append(strVariable.Chars[i].Text);
                        }
                        strVariable.Text = sb.ToString();
                        program.Variables.Add(strVariable);
                        break;
                    // Array of characters
                    // - 1 1 0 _ _ _ _ _ : letter - 60H
                    // - 2 bytes         : total no. elements & dims + 1 for no. of dims
                    // - 1 byte          : no. of dimensions
                    // - 2 bytes         : first dimension
                    //    ...
                    // - 2 bytes         : last dimension
                    // - 1 byte each     : elements
                    case 192:
                        CharArrayVariable charArrayVariable = new CharArrayVariable() { Name = name + "$" };
                            ReadInteger(binaryMemoryStream, ref variablesBytesCounter);
                        charArrayVariable.NumberOfDimensions = ReadByte(binaryMemoryStream, ref variablesBytesCounter);
                        charArrayVariable.DimensionsSizes = new int[charArrayVariable.NumberOfDimensions];
                        valuesCount = 1;
                        for (int i = 0; i < charArrayVariable.NumberOfDimensions; i++)
                        {
                            charArrayVariable.DimensionsSizes[i] = ReadInteger(binaryMemoryStream, ref variablesBytesCounter);
                            valuesCount *= charArrayVariable.DimensionsSizes[i];
                        }
                        for (int i = 0; i < valuesCount; i++)
                        {
                            SpectrumChar value = ReadSpectrumChar(binaryMemoryStream, ref variablesBytesCounter);
                            charArrayVariable.Values.Add(value);
                        }
                        program.Variables.Add(charArrayVariable); 
                        break;
                    default:
                        throw new Exception("Uknown variable type");
                }
            }
        }

        private static byte ReadByte(Stream binaryMemoryStream, ref int variablesBytesCounter)
        {
            byte varTypeAndFirstLetter = (byte)binaryMemoryStream.ReadByte();
            variablesBytesCounter++;
            return varTypeAndFirstLetter;
        }

        private static int ReadLineNumber(Stream binaryMemoryStream, ref int bytesCounter)
        {
            int lineNumber = binaryMemoryStream.ReadByte() * 256 + binaryMemoryStream.ReadByte();
            bytesCounter += 2;
            return lineNumber;
        }

        private static int ReadInteger(Stream binaryMemoryStream, ref int bytesCounter)
        {
            int lengthOfTextPlusEnter = binaryMemoryStream.ReadByte() + binaryMemoryStream.ReadByte() * 256;
            bytesCounter += 2;
            return lengthOfTextPlusEnter;
        }

        private static SpectrumChar ReadSpectrumChar(Stream binaryMemoryStream, ref int bytesCounter)
        {
            byte spectrumCharCode = (byte)binaryMemoryStream.ReadByte();
            bytesCounter++;
            SpectrumChar spectrumChar = SpectrumCharSet.GetSpectrumChar(spectrumCharCode);
            return spectrumChar;
        }

        private static SpectrumNumber ReadSpectrumNumber(Stream binaryMemoryStream, ref int bytesCounter)
        {
            byte[] numBytes = new byte[5];
            for (int j = 0; j < 5; j++)
            {
                numBytes[j] = (byte)binaryMemoryStream.ReadByte();
                bytesCounter++;
            }
            SpectrumNumber spectrumNumber = new SpectrumNumber(numBytes);
            return spectrumNumber;
        }       
    }

    // --- Spectrum basic numbers ---

    // Spectrum basic 5 bytes number format
    // (source : MASTERING MACHINE CODE ON YOUR ZX SPECTRUM by Toni Baker)

    // Arithmetic in the Spectrum ROM falls into two classes: integer and floating point. 
    // Both of these have one thing in common – numbers in both classesare represented in five bytes.

    // INTEGERS

    // For a positive number which may be written in two bytes in hex as “mmnn”, the five byte form is : 00 00 nn mm 00.
    // Example : 1000d => 00 00 E8 03 00 (Note that 1000d = 03E8h).
    // For a negative number you must add 65536d, and then the five byte form is : 00 FF nn mm 00.
    // Example : –1000d => 00 FF 18 FC 00 (Note that –1000d = FC18h).

    // DECIMALS

    // 0. First of all you need to check the size of such a number. 
    // The largest number which may be stored in five byte form is 1.7014118E38. 
    // The smallest number is, of course, minus that quantity. 
    // No number outside this range may be handled by the Spectrum.

    // 1. Step one is to ignore the sign of the number (ie LET X1 = ABS X).
    // Then compute a quantity called the EXPONENT by the following formula:
    // LET EXPONENT = 1 + INT (LOG X1/LOG 2).
    // The first byte of the five byte form is 80 plus this exponent.
    // Examples : 
    // Number   Absolute Value  Exponent                Five Byte Form
    // X        X1 = ABS X      1 + INT(LOG X1/LOG 2)   EXPONENT + 80H
    // 0.375    0.375           –1                      7F
    // PI       3.1415927       2                       82
    // –1.5E20  1.5E20          68                      C4

    // 2. Step two is to compute a quantity called the MANTISSA.
    // The following formula will work it out for you:
    // LET MANTISSA = X1 / 2 ↑ EXPONENT.
    // Examples :
    // Number   Exponent    Mantissa
    // 0.375    –1          0.75
    // PI       2           0.7853982
    // –1.5E20  68          0.5082198

    // 3. The last four bytes of the Sinclair form, so somehow are derived from the MANTISSA with the following routine :
    // LET A = MANTISSA
    // FOR I = 1 TO 4
    // LET A = 256*A
    // Next byte of five byte form is INT A written in hex.
    // LET A = A – INT A
    // NEXT I
    // Note : the final value of A will be used in the next step.
    // Examples :
    // Number   Five byte form      Final “A”
    // 0.375    7F C0 00 00 00      0
    // PI       82 C9 0F DA A1      0.6314368
    // –1.5E20  C4 82 1A B0 D4      0.2842112

    // 4. The next stage is called “rounding” and works as follows. 
    // If the final value of A is greater than or equal to zero point five then you must increment, as a whole, the last four bytes of the five byte form 
    // (eg 00 00 00 29 increments to 00 00 00 2A, 23 29 FF FF increments to 23 2A 00 00, and so on). 
    // There is one point you do have to watch out for though.
    // If the four bytes started their life as FF FF FF FF then you don’t “increment” them to 00 00 00 00 – 
    // what in fact happens is that these four bytes actually become 80 00 00 00, but the first byte of the five byte form is incremented. 
    // Examples :
    // Number   Five byte form
    // 0.375    7F C0 00 00 00
    // PI       82 C9 0F DA A2
    // –1.5E20  C4 82 1A B0 D4

    // 5. The final step is to re-introduce the sign of the original number X. 
    // The rule is very simple: if X was positive then subtract 80 from the second byte and
    // if X was negative then do nothing.
    // Examples :
    // Number   Five byte form
    // 0.375    7F 40 00 00 00
    // PI       82 49 0F DA A2
    // –1.5E20  C4 82 1A B0 D4

    public enum SpectrumNumberType
    {
        Integer,
        FloatingPoint
    }

    public class SpectrumNumber
    {
        public byte[] Bytes { get; set; }

        public SpectrumNumberType Type { get; set; }
        public int IntValue { get; set; }
        public double FloatValue { get; set; }

        public string Text { get; set; }     

        public SpectrumNumber(byte[] bytes)
        {
            Bytes = bytes;
            if (bytes[0] == 0)
            {
                Type = SpectrumNumberType.Integer;
                IntValue = ConvertBytesToInt(bytes);
            }
            else
            {
                Type = SpectrumNumberType.FloatingPoint;
                FloatValue = ConvertBytesToFloat(bytes);
            }
            Text = ConvertBytesToText(Bytes);
        }

        public SpectrumNumber(string text)
        {
            Text = text;
            if (text.Contains('.'))
            {
                Type = SpectrumNumberType.FloatingPoint;
                FloatValue = double.Parse(text, CultureInfo.InvariantCulture);
                Bytes = ConvertFloatToBytes(FloatValue);
            }
            else
            {
                Type = SpectrumNumberType.Integer;
                IntValue = int.Parse(text, CultureInfo.InvariantCulture);
                Bytes = ConvertIntToBytes(IntValue);
            }
        }

        public SpectrumNumber(int value)
        {
            Type = SpectrumNumberType.Integer;
            IntValue = value;
            Bytes = ConvertIntToBytes(IntValue);
            Text = ConvertBytesToText(Bytes);
        }

        public SpectrumNumber(double value)
        {
            Type = SpectrumNumberType.FloatingPoint;
            FloatValue = value;
            Bytes = ConvertFloatToBytes(FloatValue);
            Text = ConvertBytesToText(Bytes);
        }

        public static SpectrumNumberType CheckNumberType(byte[] bytes)
        {
            if (bytes[0] == 0)
            {
                return SpectrumNumberType.Integer;
            }
            else
            {
                return SpectrumNumberType.FloatingPoint;
            }
        }

        public static int ConvertBytesToInt(byte[] bytes)
        {
            if (bytes[0] != 0)
            {
                throw new InvalidOperationException("Bytes represent a floating point number");
            }
            int value = bytes[3] * 256 + bytes[2];
            if (bytes[1] == 0)
            {
                return value;
            }
            else
            {
                return value - 65536;
            }
        }
        
        public static double ConvertBytesToFloat(byte[] bytes)
        {
            if (bytes[0] == 0)
            {
                throw new InvalidOperationException("Bytes represent an integer");
            }

            // 5. The final step is to re-introduce the sign of the original number X. 
            // The rule is very simple: if X was positive then subtract 80 from the second byte and
            // if X was negative then do nothing.
            int sign;
            byte workbytes1;
            if ((bytes[1] & 0x80) > 0)
            {
                sign = -1;
                workbytes1 = bytes[1];
            }
            else
            {
                sign = 1;
                workbytes1 = (byte)(bytes[1] + 0x80);
            }

            // 3. The last four bytes of the Sinclair form, so somehow are derived from the MANTISSA with the following routine :
            // LET A = MANTISSA
            // FOR I = 1 TO 4
            // LET A = 256*A
            // Next byte of five byte form is INT A written in hex.
            // LET A = A – INT A
            // NEXT I
            double mantissa = workbytes1 / 256.0 + bytes[2] / 65536.0 + bytes[3] / 16777216.0 + bytes[4] / 4294967296.0;

            // 1. The first byte of the five byte form is 80 plus this exponent.
            int exponent = bytes[0] - 0x80;

            // 2. Step two is to compute a quantity called the MANTISSA.
            // LET MANTISSA = X1 / 2 ↑ EXPONENT.
            double absValue = mantissa * Math.Pow(2, exponent);

            return absValue * sign;
        }

        public static byte[] ConvertIntToBytes(int value)
        {
            if (Math.Abs(value) > MAX_INT_VALUE)
            {
                throw new InvalidOperationException("Number too big, max integer value is " + MAX_INT_VALUE);
            }
            byte[] bytes = new byte[5];
            bytes[0] = 0;
            if(value >= 0)
            {
                bytes[1] = 0;
            }
            else
            {
                bytes[1] = 0xFF;
            }
            bytes[2] = (byte)(value % 256);
            bytes[3] = (byte)((value - bytes[2]) / 256);
            bytes[4] = 0;
            return bytes;
        }

        public static byte[] ConvertFloatToBytes(double value)
        {
            if (Math.Abs(value) > MAX_FLOAT_VALUE)
            {
                throw new InvalidOperationException("Number too big, max floating point value is " + MAX_FLOAT_VALUE);
            }

            byte[] bytes = new byte[5];
            
            // 1. Step one is to ignore the sign of the number (ie LET X1 = ABS X).
            double absValue = Math.Abs(value);
            // LET EXPONENT = 1 + INT (LOG X1/LOG 2).
            int exponent = 1 + (int)Math.Floor(Math.Log(absValue, 2));
            // The first byte of the five byte form is 80 plus this exponent.
            bytes[0] = (byte)(exponent + 0x80);
            
            // 2. Step two is to compute a quantity called the MANTISSA.
            // LET MANTISSA = X1 / 2 ↑ EXPONENT.
            double mantissa = absValue / Math.Pow(2, exponent);

            // 3. The last four bytes of the Sinclair form, so somehow are derived from the MANTISSA with the following routine :
            // LET A = MANTISSA
            // FOR I = 1 TO 4
            // LET A = 256*A
            // Next byte of five byte form is INT A written in hex.
            // LET A = A – INT A
            // NEXT I
            double remainder = mantissa;
            for (int i = 1; i <= 4; i++)
            {
                remainder *= 256;
                bytes[i] = (byte)remainder;
                remainder -= bytes[i]; 
            }            
            // Note : the final value of A will be used in the next step.

            // 4. The next stage is called “rounding” and works as follows. 
            // If the final value of A is greater than or equal to zero point five then you must increment, as a whole, the last four bytes of the five byte form 
            // There is one point you do have to watch out for though.
            // If the four bytes started their life as FF FF FF FF then you don’t “increment” them to 00 00 00 00 – 
            // what in fact happens is that these four bytes actually become 80 00 00 00, but the first byte of the five byte form is incremented. 
            if (remainder >= 0.5)
            {
                long mantissaBytes = bytes[1] * 16777216L + bytes[2] * 65536L + bytes[3] * 256L + bytes[4];
                if (mantissaBytes == 4294967295 /* FF FF FF FF */)
                {
                    bytes[0]++;
                    bytes[1] = 0x80;
                    bytes[2] = 0;
                    bytes[3] = 0;
                    bytes[4] = 0;
                }
                else
                {
                    mantissaBytes += 1;
                    for (int i = 4; i >= 1; i--)
                    {
                        bytes[i] = (byte)(mantissaBytes % 256);
                        mantissaBytes = mantissaBytes / 256;
                    }
                }
            }

            // 5. The final step is to re-introduce the sign of the original number X. 
            // The rule is very simple: if X was positive then subtract 80 from the second byte and
            // if X was negative then do nothing.
            if (value >= 0)
            {
                bytes[1] -= 0x80;
            }

            return bytes;
        }

        public static string ConvertBytesToText(byte[] bytes)
        {
            if (bytes[0] == 0)
            {
                // SpectrumNumberType.Integer;
                int value = ConvertBytesToInt(bytes);
                if (Math.Abs(value) < 1E9)
                {
                    return value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    return value.ToString("E8", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                // SpectrumNumberType.FloatingPoint;
                double value = ConvertBytesToFloat(bytes);
                if (Math.Abs(value) < 1E9)
                {
                    return value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    return value.ToString("E8", CultureInfo.InvariantCulture);
                }
            }
        }
        
        public static int MAX_INT_VALUE = 65535;
        public static double MAX_FLOAT_VALUE = 1.7014118E38;
    }

    // --- Spectrum Basic charset ---

    public enum SpectrumCharType
    {
        Undefined,
        ControlCode,
        PrintableChar,
        BlockGraphics,
        UserDefinedGraphics,
        BasicToken,
        BasicFunctionCode,
        BasicCommandCode
    }

    public class SpectrumChar
    {
        public int Code { get; set; }
        public SpectrumCharType Type { get; set; }
        public string Text { get; set; }
        public SpectrumNumber Number { get; set; }

        public SpectrumChar CloneForNumber(SpectrumNumber number)
        {
            return new SpectrumChar() { Code = this.Code, Type = this.Type, Text = this.Text, Number = number };
        }
    }

    public static class SpectrumCharSet
    {
        public static SpectrumChar GetSpectrumChar(byte spectrumCharCode)
        {
            return chars[spectrumCharCode];
        }

        private static SpectrumChar[] chars = new SpectrumChar[] {
            new SpectrumChar() { Code = 0, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 1, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 2, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 3, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 4, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 5, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 6, Type = SpectrumCharType.ControlCode, Text = "comma" },
            new SpectrumChar() { Code = 7, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 8, Type = SpectrumCharType.ControlCode, Text = "left" },
            new SpectrumChar() { Code = 9, Type = SpectrumCharType.ControlCode, Text = "right" },
            new SpectrumChar() { Code = 10, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 11, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 12, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 13, Type = SpectrumCharType.ControlCode, Text = "enter" },
            new SpectrumChar() { Code = 14, Type = SpectrumCharType.ControlCode, Text = "number" },
            new SpectrumChar() { Code = 15, Type = SpectrumCharType.Undefined, Text = null },

            new SpectrumChar() { Code = 16, Type = SpectrumCharType.ControlCode, Text = "INK" },
            new SpectrumChar() { Code = 17, Type = SpectrumCharType.ControlCode, Text = "PAPER" },
            new SpectrumChar() { Code = 18, Type = SpectrumCharType.ControlCode, Text = "FLASH" },
            new SpectrumChar() { Code = 19, Type = SpectrumCharType.ControlCode, Text = "BRIGHT" },
            new SpectrumChar() { Code = 20, Type = SpectrumCharType.ControlCode, Text = "INVERSE" },
            new SpectrumChar() { Code = 21, Type = SpectrumCharType.ControlCode, Text = "OVER" },
            new SpectrumChar() { Code = 22, Type = SpectrumCharType.ControlCode, Text = "AT" },
            new SpectrumChar() { Code = 23, Type = SpectrumCharType.ControlCode, Text = "TAB" },
            new SpectrumChar() { Code = 24, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 25, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 26, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 27, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 28, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 29, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 30, Type = SpectrumCharType.Undefined, Text = null },
            new SpectrumChar() { Code = 31, Type = SpectrumCharType.Undefined, Text = null },

            new SpectrumChar() { Code = 32, Type = SpectrumCharType.PrintableChar, Text = " " },
            new SpectrumChar() { Code = 33, Type = SpectrumCharType.PrintableChar, Text = "!" },
            new SpectrumChar() { Code = 34, Type = SpectrumCharType.PrintableChar, Text = "\"" },
            new SpectrumChar() { Code = 35, Type = SpectrumCharType.PrintableChar, Text = "#" },
            new SpectrumChar() { Code = 36, Type = SpectrumCharType.PrintableChar, Text = "$" },
            new SpectrumChar() { Code = 37, Type = SpectrumCharType.PrintableChar, Text = "%" },
            new SpectrumChar() { Code = 38, Type = SpectrumCharType.PrintableChar, Text = "&" },
            new SpectrumChar() { Code = 39, Type = SpectrumCharType.PrintableChar, Text = "'" },
            new SpectrumChar() { Code = 40, Type = SpectrumCharType.PrintableChar, Text = "(" },
            new SpectrumChar() { Code = 41, Type = SpectrumCharType.PrintableChar, Text = ")" },
            new SpectrumChar() { Code = 42, Type = SpectrumCharType.PrintableChar, Text = "*" },
            new SpectrumChar() { Code = 43, Type = SpectrumCharType.PrintableChar, Text = "+" },
            new SpectrumChar() { Code = 44, Type = SpectrumCharType.PrintableChar, Text = "," },
            new SpectrumChar() { Code = 45, Type = SpectrumCharType.PrintableChar, Text = "-" },
            new SpectrumChar() { Code = 46, Type = SpectrumCharType.PrintableChar, Text = "." },
            new SpectrumChar() { Code = 47, Type = SpectrumCharType.PrintableChar, Text = "/" },

            new SpectrumChar() { Code = 48, Type = SpectrumCharType.PrintableChar, Text = "0" },
            new SpectrumChar() { Code = 49, Type = SpectrumCharType.PrintableChar, Text = "1" },
            new SpectrumChar() { Code = 50, Type = SpectrumCharType.PrintableChar, Text = "2" },
            new SpectrumChar() { Code = 51, Type = SpectrumCharType.PrintableChar, Text = "3" },
            new SpectrumChar() { Code = 52, Type = SpectrumCharType.PrintableChar, Text = "4" },
            new SpectrumChar() { Code = 53, Type = SpectrumCharType.PrintableChar, Text = "5" },
            new SpectrumChar() { Code = 54, Type = SpectrumCharType.PrintableChar, Text = "6" },
            new SpectrumChar() { Code = 55, Type = SpectrumCharType.PrintableChar, Text = "7" },
            new SpectrumChar() { Code = 56, Type = SpectrumCharType.PrintableChar, Text = "8" },
            new SpectrumChar() { Code = 57, Type = SpectrumCharType.PrintableChar, Text = "9" },
            new SpectrumChar() { Code = 58, Type = SpectrumCharType.PrintableChar, Text = ":" },
            new SpectrumChar() { Code = 59, Type = SpectrumCharType.PrintableChar, Text = ";" },
            new SpectrumChar() { Code = 60, Type = SpectrumCharType.PrintableChar, Text = "<" },
            new SpectrumChar() { Code = 61, Type = SpectrumCharType.PrintableChar, Text = "=" },
            new SpectrumChar() { Code = 62, Type = SpectrumCharType.PrintableChar, Text = ">" },
            new SpectrumChar() { Code = 63, Type = SpectrumCharType.PrintableChar, Text = "?" },

            new SpectrumChar() { Code = 64, Type = SpectrumCharType.PrintableChar, Text = "@" },
            new SpectrumChar() { Code = 65, Type = SpectrumCharType.PrintableChar, Text = "A" },
            new SpectrumChar() { Code = 66, Type = SpectrumCharType.PrintableChar, Text = "B" },
            new SpectrumChar() { Code = 67, Type = SpectrumCharType.PrintableChar, Text = "C" },
            new SpectrumChar() { Code = 68, Type = SpectrumCharType.PrintableChar, Text = "D" },
            new SpectrumChar() { Code = 69, Type = SpectrumCharType.PrintableChar, Text = "E" },
            new SpectrumChar() { Code = 70, Type = SpectrumCharType.PrintableChar, Text = "F" },
            new SpectrumChar() { Code = 71, Type = SpectrumCharType.PrintableChar, Text = "G" },
            new SpectrumChar() { Code = 72, Type = SpectrumCharType.PrintableChar, Text = "H" },
            new SpectrumChar() { Code = 73, Type = SpectrumCharType.PrintableChar, Text = "I" },
            new SpectrumChar() { Code = 74, Type = SpectrumCharType.PrintableChar, Text = "J" },
            new SpectrumChar() { Code = 75, Type = SpectrumCharType.PrintableChar, Text = "K" },
            new SpectrumChar() { Code = 76, Type = SpectrumCharType.PrintableChar, Text = "L" },
            new SpectrumChar() { Code = 77, Type = SpectrumCharType.PrintableChar, Text = "M" },
            new SpectrumChar() { Code = 78, Type = SpectrumCharType.PrintableChar, Text = "N" },
            new SpectrumChar() { Code = 79, Type = SpectrumCharType.PrintableChar, Text = "O" },

            new SpectrumChar() { Code = 80, Type = SpectrumCharType.PrintableChar, Text = "P" },
            new SpectrumChar() { Code = 81, Type = SpectrumCharType.PrintableChar, Text = "Q" },
            new SpectrumChar() { Code = 82, Type = SpectrumCharType.PrintableChar, Text = "R" },
            new SpectrumChar() { Code = 83, Type = SpectrumCharType.PrintableChar, Text = "S" },
            new SpectrumChar() { Code = 84, Type = SpectrumCharType.PrintableChar, Text = "T" },
            new SpectrumChar() { Code = 85, Type = SpectrumCharType.PrintableChar, Text = "U" },
            new SpectrumChar() { Code = 86, Type = SpectrumCharType.PrintableChar, Text = "V" },
            new SpectrumChar() { Code = 87, Type = SpectrumCharType.PrintableChar, Text = "W" },
            new SpectrumChar() { Code = 88, Type = SpectrumCharType.PrintableChar, Text = "X" },
            new SpectrumChar() { Code = 89, Type = SpectrumCharType.PrintableChar, Text = "Y" },
            new SpectrumChar() { Code = 90, Type = SpectrumCharType.PrintableChar, Text = "Z" },
            new SpectrumChar() { Code = 91, Type = SpectrumCharType.PrintableChar, Text = "[" },
            new SpectrumChar() { Code = 92, Type = SpectrumCharType.PrintableChar, Text = "\\" },
            new SpectrumChar() { Code = 93, Type = SpectrumCharType.PrintableChar, Text = "]" },
            new SpectrumChar() { Code = 94, Type = SpectrumCharType.PrintableChar, Text = "^" },
            new SpectrumChar() { Code = 95, Type = SpectrumCharType.PrintableChar, Text = "_" },

            new SpectrumChar() { Code = 96, Type = SpectrumCharType.PrintableChar, Text = "£" },
            new SpectrumChar() { Code = 97, Type = SpectrumCharType.PrintableChar, Text = "a" },
            new SpectrumChar() { Code = 98, Type = SpectrumCharType.PrintableChar, Text = "b" },
            new SpectrumChar() { Code = 99, Type = SpectrumCharType.PrintableChar, Text = "c" },
            new SpectrumChar() { Code = 100, Type = SpectrumCharType.PrintableChar, Text = "d" },
            new SpectrumChar() { Code = 101, Type = SpectrumCharType.PrintableChar, Text = "e" },
            new SpectrumChar() { Code = 102, Type = SpectrumCharType.PrintableChar, Text = "f" },
            new SpectrumChar() { Code = 103, Type = SpectrumCharType.PrintableChar, Text = "g" },
            new SpectrumChar() { Code = 104, Type = SpectrumCharType.PrintableChar, Text = "h" },
            new SpectrumChar() { Code = 105, Type = SpectrumCharType.PrintableChar, Text = "i" },
            new SpectrumChar() { Code = 106, Type = SpectrumCharType.PrintableChar, Text = "j" },
            new SpectrumChar() { Code = 107, Type = SpectrumCharType.PrintableChar, Text = "k" },
            new SpectrumChar() { Code = 108, Type = SpectrumCharType.PrintableChar, Text = "l" },
            new SpectrumChar() { Code = 109, Type = SpectrumCharType.PrintableChar, Text = "m" },
            new SpectrumChar() { Code = 110, Type = SpectrumCharType.PrintableChar, Text = "n" },
            new SpectrumChar() { Code = 111, Type = SpectrumCharType.PrintableChar, Text = "o" },

            new SpectrumChar() { Code = 112, Type = SpectrumCharType.PrintableChar, Text = "p" },
            new SpectrumChar() { Code = 113, Type = SpectrumCharType.PrintableChar, Text = "q" },
            new SpectrumChar() { Code = 114, Type = SpectrumCharType.PrintableChar, Text = "r" },
            new SpectrumChar() { Code = 115, Type = SpectrumCharType.PrintableChar, Text = "s" },
            new SpectrumChar() { Code = 116, Type = SpectrumCharType.PrintableChar, Text = "t" },
            new SpectrumChar() { Code = 117, Type = SpectrumCharType.PrintableChar, Text = "u" },
            new SpectrumChar() { Code = 118, Type = SpectrumCharType.PrintableChar, Text = "v" },
            new SpectrumChar() { Code = 119, Type = SpectrumCharType.PrintableChar, Text = "w" },
            new SpectrumChar() { Code = 120, Type = SpectrumCharType.PrintableChar, Text = "x" },
            new SpectrumChar() { Code = 121, Type = SpectrumCharType.PrintableChar, Text = "y" },
            new SpectrumChar() { Code = 122, Type = SpectrumCharType.PrintableChar, Text = "z" },
            new SpectrumChar() { Code = 123, Type = SpectrumCharType.PrintableChar, Text = "{" },
            new SpectrumChar() { Code = 124, Type = SpectrumCharType.PrintableChar, Text = "|" },
            new SpectrumChar() { Code = 125, Type = SpectrumCharType.PrintableChar, Text = "}" },
            new SpectrumChar() { Code = 126, Type = SpectrumCharType.PrintableChar, Text = "~" },
            new SpectrumChar() { Code = 127, Type = SpectrumCharType.PrintableChar, Text = "©" },

            new SpectrumChar() { Code = 128, Type = SpectrumCharType.BlockGraphics, Text = "[BG-0]" },
            new SpectrumChar() { Code = 129, Type = SpectrumCharType.BlockGraphics, Text = "[BG-1]" },
            new SpectrumChar() { Code = 130, Type = SpectrumCharType.BlockGraphics, Text = "[BG-2]" },
            new SpectrumChar() { Code = 131, Type = SpectrumCharType.BlockGraphics, Text = "[BG-3]" },
            new SpectrumChar() { Code = 132, Type = SpectrumCharType.BlockGraphics, Text = "[BG-4]" },
            new SpectrumChar() { Code = 133, Type = SpectrumCharType.BlockGraphics, Text = "[BG-5]" },
            new SpectrumChar() { Code = 134, Type = SpectrumCharType.BlockGraphics, Text = "[BG-6]" },
            new SpectrumChar() { Code = 135, Type = SpectrumCharType.BlockGraphics, Text = "[BG-7]" },
            new SpectrumChar() { Code = 136, Type = SpectrumCharType.BlockGraphics, Text = "[BG-8]" },
            new SpectrumChar() { Code = 137, Type = SpectrumCharType.BlockGraphics, Text = "[BG-9]" },
            new SpectrumChar() { Code = 138, Type = SpectrumCharType.BlockGraphics, Text = "[BG-10]" },
            new SpectrumChar() { Code = 139, Type = SpectrumCharType.BlockGraphics, Text = "[BG-11]" },
            new SpectrumChar() { Code = 140, Type = SpectrumCharType.BlockGraphics, Text = "[BG-12]" },
            new SpectrumChar() { Code = 141, Type = SpectrumCharType.BlockGraphics, Text = "[BG-13]" },
            new SpectrumChar() { Code = 142, Type = SpectrumCharType.BlockGraphics, Text = "[BG-14]" },
            new SpectrumChar() { Code = 143, Type = SpectrumCharType.BlockGraphics, Text = "[BG-15]" },

            new SpectrumChar() { Code = 144, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-A]" },
            new SpectrumChar() { Code = 145, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-B]" },
            new SpectrumChar() { Code = 146, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-C]" },
            new SpectrumChar() { Code = 147, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-D]" },
            new SpectrumChar() { Code = 148, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-E]" },
            new SpectrumChar() { Code = 149, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-F]" },
            new SpectrumChar() { Code = 150, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-G]" },
            new SpectrumChar() { Code = 151, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-H]" },
            new SpectrumChar() { Code = 152, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-I]" },
            new SpectrumChar() { Code = 153, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-J]" },
            new SpectrumChar() { Code = 154, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-K]" },
            new SpectrumChar() { Code = 155, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-L]" },
            new SpectrumChar() { Code = 156, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-M]" },
            new SpectrumChar() { Code = 157, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-N]" },
            new SpectrumChar() { Code = 158, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-O]" },
            new SpectrumChar() { Code = 159, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-P]" },

            new SpectrumChar() { Code = 160, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-Q]" },
            new SpectrumChar() { Code = 161, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-R]" },
            new SpectrumChar() { Code = 162, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-S]" },
            new SpectrumChar() { Code = 163, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-T]" },
            new SpectrumChar() { Code = 164, Type = SpectrumCharType.UserDefinedGraphics, Text = "[UDG-U]" },
            new SpectrumChar() { Code = 165, Type = SpectrumCharType.BasicFunctionCode, Text = "RND" },
            new SpectrumChar() { Code = 166, Type = SpectrumCharType.BasicFunctionCode, Text = "INKEY$" },
            new SpectrumChar() { Code = 167, Type = SpectrumCharType.BasicFunctionCode, Text = "PI" },
            new SpectrumChar() { Code = 168, Type = SpectrumCharType.BasicFunctionCode, Text = "FN" },
            new SpectrumChar() { Code = 169, Type = SpectrumCharType.BasicFunctionCode, Text = "POINT" },
            new SpectrumChar() { Code = 170, Type = SpectrumCharType.BasicFunctionCode, Text = "SCREEN$" },
            new SpectrumChar() { Code = 171, Type = SpectrumCharType.BasicFunctionCode, Text = "ATTR" },
            new SpectrumChar() { Code = 172, Type = SpectrumCharType.BasicToken, Text = "AT" },
            new SpectrumChar() { Code = 173, Type = SpectrumCharType.BasicToken, Text = "TAB" },
            new SpectrumChar() { Code = 174, Type = SpectrumCharType.BasicFunctionCode, Text = "VAL$" },
            new SpectrumChar() { Code = 175, Type = SpectrumCharType.BasicFunctionCode, Text = "CODE" },

            new SpectrumChar() { Code = 176, Type = SpectrumCharType.BasicFunctionCode, Text = "VAL" },
            new SpectrumChar() { Code = 177, Type = SpectrumCharType.BasicFunctionCode, Text = "LEN" },
            new SpectrumChar() { Code = 178, Type = SpectrumCharType.BasicFunctionCode, Text = "SIN" },
            new SpectrumChar() { Code = 179, Type = SpectrumCharType.BasicFunctionCode, Text = "COS" },
            new SpectrumChar() { Code = 180, Type = SpectrumCharType.BasicFunctionCode, Text = "TAN" },
            new SpectrumChar() { Code = 181, Type = SpectrumCharType.BasicFunctionCode, Text = "ASN" },
            new SpectrumChar() { Code = 182, Type = SpectrumCharType.BasicFunctionCode, Text = "ACS" },
            new SpectrumChar() { Code = 183, Type = SpectrumCharType.BasicFunctionCode, Text = "ATN" },
            new SpectrumChar() { Code = 184, Type = SpectrumCharType.BasicFunctionCode, Text = "LN" },
            new SpectrumChar() { Code = 185, Type = SpectrumCharType.BasicFunctionCode, Text = "EXP" },
            new SpectrumChar() { Code = 186, Type = SpectrumCharType.BasicFunctionCode, Text = "INT" },
            new SpectrumChar() { Code = 187, Type = SpectrumCharType.BasicFunctionCode, Text = "SQR" },
            new SpectrumChar() { Code = 188, Type = SpectrumCharType.BasicFunctionCode, Text = "SGN" },
            new SpectrumChar() { Code = 189, Type = SpectrumCharType.BasicFunctionCode, Text = "ABS" },
            new SpectrumChar() { Code = 190, Type = SpectrumCharType.BasicFunctionCode, Text = "PEEK" },
            new SpectrumChar() { Code = 191, Type = SpectrumCharType.BasicFunctionCode, Text = "IN" },

            new SpectrumChar() { Code = 192, Type = SpectrumCharType.BasicFunctionCode, Text = "USR" },
            new SpectrumChar() { Code = 193, Type = SpectrumCharType.BasicFunctionCode, Text = "STR$" },
            new SpectrumChar() { Code = 194, Type = SpectrumCharType.BasicFunctionCode, Text = "CHR$" },
            new SpectrumChar() { Code = 195, Type = SpectrumCharType.BasicToken, Text = "NOT" },
            new SpectrumChar() { Code = 196, Type = SpectrumCharType.BasicFunctionCode, Text = "BIN" },
            new SpectrumChar() { Code = 197, Type = SpectrumCharType.BasicToken, Text = "OR" },
            new SpectrumChar() { Code = 198, Type = SpectrumCharType.BasicToken, Text = "AND" },
            new SpectrumChar() { Code = 199, Type = SpectrumCharType.BasicToken, Text = "<=" },
            new SpectrumChar() { Code = 200, Type = SpectrumCharType.BasicToken, Text = ">=" },
            new SpectrumChar() { Code = 201, Type = SpectrumCharType.BasicToken, Text = "<>" },
            new SpectrumChar() { Code = 202, Type = SpectrumCharType.BasicToken, Text = "LINE" },
            new SpectrumChar() { Code = 203, Type = SpectrumCharType.BasicToken, Text = "THEN" },
            new SpectrumChar() { Code = 204, Type = SpectrumCharType.BasicToken, Text = "TO" },
            new SpectrumChar() { Code = 205, Type = SpectrumCharType.BasicToken, Text = "STEP" },
            new SpectrumChar() { Code = 206, Type = SpectrumCharType.BasicCommandCode, Text = "DEF FN" },
            new SpectrumChar() { Code = 207, Type = SpectrumCharType.BasicCommandCode, Text = "CAT" },

            new SpectrumChar() { Code = 208, Type = SpectrumCharType.BasicCommandCode, Text = "FORMAT" },
            new SpectrumChar() { Code = 209, Type = SpectrumCharType.BasicCommandCode, Text = "MOVE" },
            new SpectrumChar() { Code = 210, Type = SpectrumCharType.BasicCommandCode, Text = "ERASE" },
            new SpectrumChar() { Code = 211, Type = SpectrumCharType.BasicCommandCode, Text = "OPEN #" },
            new SpectrumChar() { Code = 212, Type = SpectrumCharType.BasicCommandCode, Text = "CLOSE #" },
            new SpectrumChar() { Code = 213, Type = SpectrumCharType.BasicCommandCode, Text = "MERGE" },
            new SpectrumChar() { Code = 214, Type = SpectrumCharType.BasicCommandCode, Text = "VERIFY" },
            new SpectrumChar() { Code = 215, Type = SpectrumCharType.BasicCommandCode, Text = "BEEP" },
            new SpectrumChar() { Code = 216, Type = SpectrumCharType.BasicCommandCode, Text = "CIRCLE" },
            new SpectrumChar() { Code = 217, Type = SpectrumCharType.BasicCommandCode, Text = "INK" },
            new SpectrumChar() { Code = 218, Type = SpectrumCharType.BasicCommandCode, Text = "PAPER" },
            new SpectrumChar() { Code = 219, Type = SpectrumCharType.BasicCommandCode, Text = "FLASH" },
            new SpectrumChar() { Code = 220, Type = SpectrumCharType.BasicCommandCode, Text = "BRIGHT" },
            new SpectrumChar() { Code = 221, Type = SpectrumCharType.BasicCommandCode, Text = "INVERSE" },
            new SpectrumChar() { Code = 222, Type = SpectrumCharType.BasicCommandCode, Text = "OVER" },
            new SpectrumChar() { Code = 223, Type = SpectrumCharType.BasicCommandCode, Text = "OUT" },

            new SpectrumChar() { Code = 224, Type = SpectrumCharType.BasicCommandCode, Text = "LPRINT" },
            new SpectrumChar() { Code = 225, Type = SpectrumCharType.BasicCommandCode, Text = "LLIST" },
            new SpectrumChar() { Code = 226, Type = SpectrumCharType.BasicCommandCode, Text = "STOP" },
            new SpectrumChar() { Code = 227, Type = SpectrumCharType.BasicCommandCode, Text = "READ" },
            new SpectrumChar() { Code = 228, Type = SpectrumCharType.BasicCommandCode, Text = "DATA" },
            new SpectrumChar() { Code = 229, Type = SpectrumCharType.BasicCommandCode, Text = "RESTORE" },
            new SpectrumChar() { Code = 230, Type = SpectrumCharType.BasicCommandCode, Text = "NEW" },
            new SpectrumChar() { Code = 231, Type = SpectrumCharType.BasicCommandCode, Text = "BORDER" },
            new SpectrumChar() { Code = 232, Type = SpectrumCharType.BasicCommandCode, Text = "CONTINUE" },
            new SpectrumChar() { Code = 233, Type = SpectrumCharType.BasicCommandCode, Text = "DIM" },
            new SpectrumChar() { Code = 234, Type = SpectrumCharType.BasicCommandCode, Text = "REM" },
            new SpectrumChar() { Code = 235, Type = SpectrumCharType.BasicCommandCode, Text = "FOR" },
            new SpectrumChar() { Code = 236, Type = SpectrumCharType.BasicCommandCode, Text = "GO TO" },
            new SpectrumChar() { Code = 237, Type = SpectrumCharType.BasicCommandCode, Text = "GO SUB" },
            new SpectrumChar() { Code = 238, Type = SpectrumCharType.BasicCommandCode, Text = "INPUT" },
            new SpectrumChar() { Code = 239, Type = SpectrumCharType.BasicCommandCode, Text = "LOAD" },

            new SpectrumChar() { Code = 240, Type = SpectrumCharType.BasicCommandCode, Text = "LIST" },
            new SpectrumChar() { Code = 241, Type = SpectrumCharType.BasicCommandCode, Text = "LET" },
            new SpectrumChar() { Code = 242, Type = SpectrumCharType.BasicCommandCode, Text = "PAUSE" },
            new SpectrumChar() { Code = 243, Type = SpectrumCharType.BasicCommandCode, Text = "NEXT" },
            new SpectrumChar() { Code = 244, Type = SpectrumCharType.BasicCommandCode, Text = "POKE" },
            new SpectrumChar() { Code = 245, Type = SpectrumCharType.BasicCommandCode, Text = "PRINT" },
            new SpectrumChar() { Code = 246, Type = SpectrumCharType.BasicCommandCode, Text = "PLOT" },
            new SpectrumChar() { Code = 247, Type = SpectrumCharType.BasicCommandCode, Text = "RUN" },
            new SpectrumChar() { Code = 248, Type = SpectrumCharType.BasicCommandCode, Text = "SAVE" },
            new SpectrumChar() { Code = 249, Type = SpectrumCharType.BasicCommandCode, Text = "RANDOMIZE" },
            new SpectrumChar() { Code = 250, Type = SpectrumCharType.BasicCommandCode, Text = "IF" },
            new SpectrumChar() { Code = 251, Type = SpectrumCharType.BasicCommandCode, Text = "CLS" },
            new SpectrumChar() { Code = 252, Type = SpectrumCharType.BasicCommandCode, Text = "DRAW" },
            new SpectrumChar() { Code = 253, Type = SpectrumCharType.BasicCommandCode, Text = "CLEAR" },
            new SpectrumChar() { Code = 254, Type = SpectrumCharType.BasicCommandCode, Text = "RETURN" },
            new SpectrumChar() { Code = 255, Type = SpectrumCharType.BasicCommandCode, Text = "COPY" }
        };
    }
}
