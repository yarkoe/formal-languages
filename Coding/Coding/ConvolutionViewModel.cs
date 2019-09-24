using System.Collections.Generic;

namespace Coding
{
    public class ConvolutionViewModel
    {
        /// <summary>
        /// Исходное представление грамматики.
        /// </summary>
        public string GrammaticalText { get; set; } = string.Empty;
        public string NumericalConvolution { get; set; } = string.Empty;

        private Convolution convolution = new Convolution();
        private CodeTable codeTable = new CodeTable();

        public ConvolutionViewModel()
        {
            codeTable.Nonterminals = new string[] { "expression", "formula", "operand", };
            codeTable.Terminals = new string[] { "formula", ",", "operation", "operand", "assignation", "increment", "tag",
                                                    "[", "]", "(", "expression", ")", ".", "->", "<tag>", "label", "number",
                                                    "char", "string", "?", ":" };
            codeTable.Semantics = new string[] { "opcode1", "operand", "opcode2", "incr1", "tag", "incr2", "array1", "array2",
                                                    "call1", "call2", "call3", "ident", "op1", "op2", "dot", "arrow", "field",
                                                    "number", "char", "string", "cond1", "cond2" };
        }

        public void ConvertGrammaticalText()
        {
            string[] grammaticalStrings = GrammaticalText.Split('\n');

            string tempNumericalConvolution = "";
            foreach (string grammaticalString in grammaticalStrings)
            {
                List<int> lexemeCodes = convolution.Convert(grammaticalString.Replace(" ", ""), codeTable);

                tempNumericalConvolution += string.Join(",", lexemeCodes.ToArray()) + "\n";
            }

            NumericalConvolution = tempNumericalConvolution;
        }
    }
}
