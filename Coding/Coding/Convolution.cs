using System;
using System.Threading.Tasks;

namespace Coding
{
    /// <summary>
    /// Предоставляет методы для свёртки лексем в числовой код.
    /// </summary>
    class Convolution
    {
        public string Convert(string grammaticalText, CodeTable codeTable)
        {
            return "pass";
        }


        public enum LexemeType
        {
            OperationChars,
            Terminals,
            Nonterminals,
            Semantics
        }
        /// <summary>
        /// Состояние последней конвертируемой лексемы.
        /// </summary>
        public enum ConvertStatus
        {
            StUndefined,
            StPreTerminal,
            StTerminal,
            StPostTerminal,
            StNonterminal,
            StPresemantics,
            StSemantics,
            StEofgram
        }

        /// <summary>
        /// По порядку конвертирует лексемы текста в коды заданной таблицы.
        /// </summary>
        class TextConverter
        {
            private ConvertStatus convertStatus = ConvertStatus.StUndefined;

            private string currentText;
            readonly private CodeTable codeTable;

            public TextConverter(string text, CodeTable codeTable)
            {
                // Уберём все пробелы из текста.
                this.currentText = text.Replace(" ", string.Empty); 
                this.codeTable = codeTable;
            }

            /// <summary>
            /// Реализует автомат для конвертации первый лексемы в тексте согласно таблице кодов.
            /// </summary>
            /// <returns> Код лексемы. </returns>
            public int ConvertNextLexeme()
            {
                switch (convertStatus)
                {
                    case ConvertStatus.StUndefined:

                    default:
                        return 0;
                }
            }

            /// <summary>
            /// Ищет первой лексеме в тексте currentText соответствие в массиве заданного типа и возвращает его код. 
            /// 0 в противном случае. В случае успеха, удаляет лексему из текста.
            /// </summary>
            /// <returns>Код знака лексемы или 0</returns>
            private int GetFirstLexeme(LexemeType lexemeType)
            {
                // Массив лексем заданного типа.
                string[] lexemes = GetLexemes(lexemeType);

                int absoluteLength = 1;
                foreach (LexemeType currentLexemeType in Enum.GetValues(typeof(LexemeType)))
                {
                    if (lexemeType == currentLexemeType) break;

                    absoluteLength += GetLexemes(currentLexemeType).Length;
                }

                for (int i = 0; i < lexemes.Length; i++)
                {
                    if (currentText.StartsWith(lexemes[i].ToString()))
                    {
                        // код знака операций.
                        return (i + absoluteLength);
                    }
                }

                return 0;
            }

            /// <summary>
            /// Возвращает массив из таблицы кодов, соответствующий заданному типу лексем.
            /// </summary>
            /// <param name="lexemeType">Тип лексемы из таблицы кодов</param>
            /// <returns>Лексемы выбранного типа</returns>
            private string[] GetLexemes(LexemeType lexemeType)
            {
                switch (lexemeType)
                {
                    case LexemeType.OperationChars:
                        return codeTable.OperationChars;

                    case LexemeType.Nonterminals:
                        return codeTable.Nonterminals;

                    case LexemeType.Terminals:
                        return codeTable.Terminals;

                    case LexemeType.Semantics:
                        return codeTable.Semantics;

                    default:
                        return codeTable.Semantics;
                }

            }
        }
    }
}
