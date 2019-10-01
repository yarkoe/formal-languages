using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Coding
{
    public class ConvolutionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Исходное представление грамматики.
        /// </summary>
        public string GrammaticalText { get; set; } = string.Empty;

        private string numericalConvolution = string.Empty;
        public string NumericalConvolution
        {
            get => numericalConvolution;
            set
            {
                numericalConvolution = value;
                NotifyPropertyChanged("NumericalConvolution");
            }
        }

        private Convolution convolution = new Convolution();
        private CodeTable codeTable = new CodeTable();

        public event PropertyChangedEventHandler PropertyChanged;

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

        public void NotifyPropertyChanged([CallerMemberName]string changedProperty = "")
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(changedProperty));
        }
    }
}
