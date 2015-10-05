using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.Basic
{
    public class BasicProgram
    {
        public BasicProgram(string sourcePath)
        {
            SourcePath = sourcePath;
            Lines = new List<BasicLine>();
            Variables = new List<BasicVariable>();
        }

        public string SourcePath { get; private set; }

        public IList<BasicLine> Lines { get; private set; }
        public IList<BasicVariable> Variables { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (BasicLine line in Lines)
            {
                sb.AppendLine(line.Text);
            }
            if(Variables.Count >  0)
            {
                sb.AppendLine();
                sb.AppendLine("--- PROGRAM VARIABLES ---");
                sb.AppendLine();
                foreach(BasicVariable variable in Variables)
                {
                    sb.AppendLine(variable.ToString());
                }
            }
            return sb.ToString();
        }
    }

    public class BasicLine
    {
        internal BasicLine(int lineNumber, int spectrumCharLength)
        {
            LineNumber = lineNumber;
            SpectrumChars = new SpectrumChar[spectrumCharLength];
        }

        public int LineNumber { get; private set; }

        public SpectrumChar[] SpectrumChars { get; private set; }

        public string Text 
        {
            get
            {
                StringBuilder lineText = new StringBuilder(LineNumber.ToString());
                for(int i = 0 ; i < SpectrumChars.Length - 1 /* do not print enter */ ; i++)
                {
                    SpectrumChar spectrumChar = SpectrumChars[i];
                    if (spectrumChar != null)
                    {
                        if (spectrumChar.Code != 14)
                        {
                            if (spectrumChar.Type == SpectrumCharType.BasicCommandCode)
                            {
                                lineText.Append(' ');
                            }
                            lineText.Append(spectrumChar.Text);
                            if (spectrumChar.Type == SpectrumCharType.BasicCommandCode ||
                                spectrumChar.Type == SpectrumCharType.BasicFunctionCode || 
                                spectrumChar.Type == SpectrumCharType.BasicToken)
                            {
                                lineText.Append(' ');
                            }
                        }
                        else // Numbers
                        {
                            lineText.Append("|"+spectrumChar.Number.Text);
                        }
                    }
                }
                return lineText.ToString();
            }
        }
    }

    public enum BasicVariableType
    {
        Number,
        NumberArray,
        ForLoopControl, 
        String,
        CharArray
    }

    public abstract class BasicVariable
    {
        public string Name { get; set; }
        public BasicVariableType Type { get; set; }

        public override string ToString()
        {
 	         return "#" + Type.ToString() + "# " + Name + " = ";
        }
    }

    public class NumberVariable : BasicVariable
    {
        public NumberVariable()
        {
            Type = BasicVariableType.Number;
        }

        public SpectrumNumber Value { get; set; }

        public override string ToString()
        {
 	         return base.ToString() + Value.Text;
        }
    }

    public class NumberArrayVariable : BasicVariable
    {
        public NumberArrayVariable()
        {
            Type = BasicVariableType.NumberArray;
            Values = new List<SpectrumNumber>();
        }

        public int NumberOfDimensions { get; set; }
        public int[] DimensionsSizes { get; set; }

        public IList<SpectrumNumber> Values { get; private set; }

        // DimensionsSizes = 2 3 4
        // 0   1,1,1 1,1,2 1,1,3 1,1,4
        // 4   1,2,1 1,2,2 1,2,3 1,2,4
        // 8   1,3,1 1,3,2 1,3,3 1,3,4 
        // 12  2,1,1 2,1,2 2,1,3 2,1,4
        // 16  2,2,1 2,2,2 2,2,3 2,2,4
        // 20  2,3,1 2,3,2 2,3,3 2,3,4
        public SpectrumNumber GetValue(int[] subscripts)
        {
            int index = 0;
            int dimension = 1;
            foreach(int subscript in subscripts)
            {
                int dimensionIncrement = 1;
                for(int d = NumberOfDimensions ; d > dimension ; d++)
                {
                    dimensionIncrement *= DimensionsSizes[d-1];
                }
                index += dimensionIncrement * (subscript - 1);
                dimension++;                
            }
            return Values[index];
        }

        public override string ToString()
        {
 	        StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach(SpectrumNumber number in Values)
            {
                sb.Append(number.Text);
                sb.Append(" ");
            }
            sb.Append("]");
            return base.ToString() + sb.ToString();
        }
    }
            
    public class ForLoopControlVariable : BasicVariable
    {
        public ForLoopControlVariable()
        {
            Type = BasicVariableType.ForLoopControl;
        }

        public SpectrumNumber Value { get; set; }
        public SpectrumNumber Limit { get; set; }
        public SpectrumNumber Step { get; set; }
        public int LoopingLine { get; set; }
        public int StatementNumberWithinLine { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("value:");
            sb.Append(Value.Text);
            sb.Append(", limit:");
            sb.Append(Limit.Text);
            sb.Append(", step:");
            sb.Append(Step.Text);
            sb.Append(", loopingline:");
            sb.Append(LoopingLine);
            sb.Append(", statementinline:");
            sb.Append(StatementNumberWithinLine);

 	         return base.ToString() + sb.ToString();
        }
    }
    
    public class StringVariable : BasicVariable
    {
        public StringVariable()
        {
            Type = BasicVariableType.String;
        }

        public int CharsCount { get; set; }
        public SpectrumChar[] Chars { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
 	         return base.ToString() + Text;
        }
    }

    public class CharArrayVariable : BasicVariable
    {
        public CharArrayVariable()
        {
            Type = BasicVariableType.CharArray;
            Values = new List<SpectrumChar>();
        }

        public int NumberOfDimensions { get; set; }
        public int[] DimensionsSizes { get; set; }

        public IList<SpectrumChar> Values { get; private set; }

        // DimensionsSizes = 2 3 4
        // 0   1,1,1 1,1,2 1,1,3 1,1,4
        // 4   1,2,1 1,2,2 1,2,3 1,2,4
        // 8   1,3,1 1,3,2 1,3,3 1,3,4 
        // 12  2,1,1 2,1,2 2,1,3 2,1,4
        // 16  2,2,1 2,2,2 2,2,3 2,2,4
        // 20  2,3,1 2,3,2 2,3,3 2,3,4
        public SpectrumChar GetValue(int[] subscripts)
        {
            int index = 0;
            int dimension = 1;
            foreach (int subscript in subscripts)
            {
                int dimensionIncrement = 1;
                for (int d = NumberOfDimensions; d > dimension; d++)
                {
                    dimensionIncrement *= DimensionsSizes[d - 1];
                }
                index += dimensionIncrement * (subscript - 1);
                dimension++;
            }
            return Values[index];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (SpectrumChar schar in Values)
            {
                sb.Append(schar.Text);
                sb.Append(" ");
            }
            sb.Append("]");
            return base.ToString() + sb.ToString();
        }
    }  
}
