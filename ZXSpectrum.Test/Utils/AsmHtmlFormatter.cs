using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z80Simulator.Assembly;
using Z80Simulator.Instructions;

namespace ZXSpectrum.Test.Utils
{
    public class AsmHtmlFormatter
    {
        public static string BeautifyAsmCode(string programName, Program program)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<!DOCTYPE html ");
            output.AppendLine("     PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"");
            output.AppendLine("     \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
            output.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            output.AppendLine("<head>");
            output.AppendLine("<title>"+programName+"</title>");
            output.AppendLine("<style type=\"text/css\">");    
            output.AppendLine("table {");
            output.AppendLine("   border: none;");
            
            output.AppendLine("}");
            output.AppendLine("td {");
            output.AppendLine("   padding: 1px 5px 1px 5px;");
            output.AppendLine("   font-family:Consolas,Monaco,Lucida Console,Liberation Mono,DejaVu Sans Mono,Bitstream Vera Sans Mono,Courier New;");
            output.AppendLine("   font-size:12px;");
            output.AppendLine("}");
            output.AppendLine(".linenumber {");
            output.AppendLine("   color:#CCCCCC;");
            output.AppendLine("}");
            output.AppendLine(".lineaddress {");
            output.AppendLine("   color:#888888;");
            output.AppendLine("}");
            output.AppendLine(".addressref {");
            output.AppendLine("   color:#888888;");
            output.AppendLine("   background-color:#FFFFFF;");
            output.AppendLine("   font-size:10px;");
            output.AppendLine("   font-weight:bold;");
            output.AppendLine("   text-decoration:none;");
            output.AppendLine("   vertical-align:top;");
            output.AppendLine("}");
            // Statement structure classes
            //  s_label
            //  s_opcode
            //  s_opcodeparam
            //  s_directive
            //  s_directiveparam
            //  s_comment
            output.AppendLine(".s_label {");
            output.AppendLine("   font-weight:bold;");
            output.AppendLine("}");
            output.AppendLine(".s_directive {");
            output.AppendLine("   background-color:#DDDDDD;");
            output.AppendLine("}");            
            output.AppendLine(".s_comment {");
            output.AppendLine("   font-style:italic;");
            output.AppendLine("}");
            // Param types classes
            //  p_constant
            //  p_memaddr
            //  p_portaddr
            //  p_register
            //  p_value
            output.AppendLine(".p_memaddr {");
            output.AppendLine("   background-color:#CCFFCC;");
            output.AppendLine("}");
            output.AppendLine(".p_portaddr {");
            output.AppendLine("   background-color:#CCFFFF;");
            output.AppendLine("}");
            output.AppendLine(".p_register {");
            output.AppendLine("   background-color:#FFCCCC;");
            output.AppendLine("}");
            output.AppendLine(".p_value {");
            output.AppendLine("   background-color:#CCCCFF;");
            output.AppendLine("}");
            output.AppendLine(".p_constant {");
            output.AppendLine("   background-color:#FFFFCC;");
            output.AppendLine("}");
            // Token classes
            // -- syntax --
            //  t_separator
            //  t_operator
            //  t_whitespace
            // -- documentation --
            //  t_comment
            // -- keywords -- 
            //  t_opcode
            //  t_directive
            // -- enums --
            //  t_register
            //  t_flagcondition
            //  t_numenum
            // -- free values
            //  t_string
            //  t_number
            // -- assembler values --
            //  t_dollar 
            //  t_symbol
            output.AppendLine(".t_whitespace {");
            output.AppendLine("   background-color:#FFFFFF;");
            output.AppendLine("}");
            output.AppendLine(".t_comment {");
            output.AppendLine("   color: #448844;");
            output.AppendLine("}");
            output.AppendLine(".t_opcode {");
            output.AppendLine("   color: #DD0000;");
            output.AppendLine("   font-weight:bold;");
            output.AppendLine("}");
            output.AppendLine(".t_directive {");
            output.AppendLine("   color: #CC00CC;");
            output.AppendLine("}");
            output.AppendLine(".t_register {");
            output.AppendLine("   color: #880000;");
            output.AppendLine("}");
            output.AppendLine(".t_string, .t_number {");
            output.AppendLine("   color: #0000CC;");
            output.AppendLine("}");
            output.AppendLine(".t_symbol, .t_dollar {");
            output.AppendLine("   color: #660066;");
            output.AppendLine("}");
            output.AppendLine("</style>");
            output.AppendLine("</head>");
            output.AppendLine("<body>");
            output.AppendLine("<table>");

            foreach(ProgramLine line in program.Lines)
            {
                output.AppendLine("<tr>");                
                output.AppendLine("<td class=\"linenumber\">"+line.LineNumber+"</td>");
                output.AppendLine("<td class=\"lineaddress\">");
                if (line.Type == ProgramLineType.OpCodeInstruction ||
                    (line.Type == ProgramLineType.AssemblerDirective && line.DirectiveName.StartsWith("DEF")))
                {
                    string hexAddress = line.LineAddress.ToString("X4");
                    output.AppendLine("<a name=\"" + hexAddress + "\">" + hexAddress + "</a>");
                }
                output.AppendLine("</td>");

                output.Append("<td>");
                if (line.LabelToken != null)
                {
                    output.Append("<span class=\"s_label\">");
                    DisplayToken(output, line.LabelToken);                    
                    output.Append("</span>");
                }
                if (line.IncomingCalls != null)
                {
                    foreach (CallSource incomingCall in line.IncomingCalls)
                    {
                        string hexAddress = incomingCall.Address.ToString("X4");
                        string previousLabel = FindPreviousLabel(program, incomingCall.Line);
                        output.Append("<br/><a href=\"#" + hexAddress + "\" class=\"addressref\"> &lt;&lt;" + hexAddress + " (" + previousLabel + ")</a>");
                    }
                }
                output.AppendLine("</td>");

                output.Append("<td>");
                switch (line.Type)
                {
                    case ProgramLineType.AssemblerDirective:
                        output.Append("<span class=\"s_directive\">");
                        DisplayToken(output, line.DirectiveNameToken);
                        bool isFirstParam = true;
                        foreach(DirectiveLineParam lineParam in line.DirectiveParameters)
                        {
                            if (!isFirstParam)
                            {
                                output.Append("<span class=\"t_separator\">,</span>");
                            }
                            output.Append("<span class=\"s_directiveparam\">");
                            foreach(AsmToken token in lineParam.Tokens)
                            {
                                DisplayToken(output, token);
                            }
                            output.Append("</span>");
                            isFirstParam = false;
                        }
                        output.Append("</span>");
                        break;
                    case ProgramLineType.OpCodeInstruction:
                        output.Append("<span class=\"s_opcode\">");
                        DisplayToken(output, line.OpCodeNameToken);
                        if (line.OpCodeBytesTokens != null)
                        {
                            foreach (AsmToken token in line.OpCodeBytesTokens)
                            {
                                DisplayToken(output, token);
                            }
                        }
                        if (line.OpCodeParameters != null)
                        {
                            int paramNumber = 1;
                            foreach (InstructionLineParam lineParam in line.OpCodeParameters)
                            {
                                if (paramNumber > 1)
                                {
                                    output.Append("<span class=\"t_separator\">,</span>");
                                }
                                output.Append("<span class=\"s_opcodeparam\">");
                                AddressingMode? addressingMode = null;
                                switch (paramNumber)
                                {
                                    case 1:
                                        addressingMode = line.InstructionType.Param1Type;
                                        break;
                                    case 2:
                                        addressingMode = line.InstructionType.Param2Type;
                                        break;
                                    case 3:
                                        addressingMode = line.InstructionType.Param3Type;
                                        break;
                                }
                                if (addressingMode.HasValue)
                                {
                                    output.Append("<span class=\"" + GetClassFromAddressingMode(addressingMode.Value) + "\">");
                                }
                                foreach (AsmToken token in lineParam.Tokens)
                                {
                                    DisplayToken(output, token);
                                }
                                if (addressingMode.HasValue)
                                {
                                    output.Append("</span>");
                                }
                                output.Append("</span>");
                                paramNumber++;
                            }
                        }
                        output.Append("</span>");
                        if (line.OutgoingCall != null)
                        {
                            string hexAddress = line.OutgoingCall.Address.ToString("X4");
                            output.Append("<a href=\"#" + hexAddress + "\" class=\"addressref\"> &gt;&gt;" + hexAddress + "</a>");
                        }
                        break;
                }
                if (line.CommentToken != null)
                {
                    output.Append("<span class=\"s_comment\">");
                    DisplayToken(output, line.CommentToken);
                    output.Append("</span>");
                }
                output.AppendLine("</td>");

                output.AppendLine("</tr>");
            }
            output.AppendLine("</table>");
            output.AppendLine("</body>");
            output.AppendLine("</html>");
            return output.ToString();
        }

        private static string FindPreviousLabel(Program program, ProgramLine referenceLine)
        {
            for (int lineNumber = referenceLine.LineNumber; lineNumber >= 1; lineNumber--)            
            { 
                string candidateLabel = program.Lines[lineNumber-1].Label;
                if (!String.IsNullOrEmpty(candidateLabel))
                {
                    return candidateLabel;
                }
            }
            return null;
        }

        private static void DisplayToken(StringBuilder output, AsmToken asmToken)
        {
            if (asmToken.LeadingWhiteSpace != null)
            {
                DisplayToken(output, asmToken.LeadingWhiteSpace);
            }
            output.Append("<span class=\"" + GetClassFromTokenType(asmToken.Type) + "\">");
            output.Append(asmToken.Text);
            output.Append("</span>");
        }

        private static string GetClassFromTokenType(AsmTokenType tokenType)
        {
            switch (tokenType)
            {
                case AsmTokenType.COLON:
                case AsmTokenType.COMMA:
                case AsmTokenType.OPENINGPAR:
                case AsmTokenType.CLOSINGPAR:
                case AsmTokenType.OPENINGBRA:
                case AsmTokenType.CLOSINGBRA:
                    return "t_separator";
                case AsmTokenType.PLUS:
                case AsmTokenType.MINUS:
                case AsmTokenType.MULTIPLY:
                    return "t_operator";
                case AsmTokenType.DOLLAR:
                    return "t_dollar";
                case AsmTokenType.COMMENT:
                    return "t_comment";
                case AsmTokenType.STRING:
                    return "t_string";
                case AsmTokenType.SYMBOL:
                    return "t_symbol";
                case AsmTokenType.OPCODE:
                    return "t_opcode";
                case AsmTokenType.REGISTER:
                case AsmTokenType.FLAGS:
                case AsmTokenType.REGISTER16:
                    return "t_register";
                case AsmTokenType.FLAGCONDITION:
                    return "t_flagcondition";
                case AsmTokenType.DIRECTIVE:
                    return "t_directive";
                case AsmTokenType.NUMBER:
                    return "t_number";
                case AsmTokenType.BIT:
                case AsmTokenType.INTERRUPTMODE:
                    return "t_numenum";
                case AsmTokenType.WHITESPACE:
                    return "t_whitespace";
                default:
                    return null;
            }            
        }

        private static string GetClassFromAddressingMode(AddressingMode addressingMode)
        {
            switch(addressingMode)
            {
                case AddressingMode.Bit:
                case AddressingMode.FlagCondition:            
                case AddressingMode.InterruptMode:
                    return "p_constant";
                case AddressingMode.Extended:
                case AddressingMode.RegisterIndirect:                    
                case AddressingMode.Indexed:                    
                case AddressingMode.ModifiedPageZero:                            
                case AddressingMode.Relative:
                    return "p_memaddr";
                case AddressingMode.IOPortImmediate:
                case AddressingMode.IOPortRegister:
                    return "p_portaddr";
                case AddressingMode.Register:
                case AddressingMode.Register16:                
                case AddressingMode.Flags:
                    return "p_register";
                case AddressingMode.Immediate:
                case AddressingMode.Immediate16:
                default:
                    return "p_value";
            }
        }

        public static string GenerateHtmlForROM()
        {
            SpectrumMemoryMap spectrumMemory = new SpectrumMemoryMap();
            string htmlDocForROM = BeautifyAsmCode("ZX Spectrum ROM", spectrumMemory.ROM);

            return htmlDocForROM;
        }        
    }
}
