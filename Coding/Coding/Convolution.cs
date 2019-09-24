using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coding
{
    /// <summary>
    /// Предоставляет методы для свёртки лексем в числовой код.
    /// </summary>
    public class Convolution
    {
        /// <summary>
        /// Сворачивает лексемы заданной грамматики в их коды в соответствие с заданной таблицей. 
        /// </summary>
        /// <param name="grammaticalText">Исходная грамматика.</param>
        /// <param name="codeTable">Заданная таблица.</param>
        /// <returns>Лист кодов лексем грамматики по порядку.</returns>
        public List<int> Convert(string grammaticalText, CodeTable codeTable)
        {
            TextConverter textConverter = new TextConverter(grammaticalText, codeTable);

            List<int> lexemeCodes = new List<int>();
            while (true)
            {
                int lexemeCode = textConverter.ConvertNextLexeme();

                if (lexemeCode == -1) return lexemeCodes;
                else if (lexemeCode == 0)
                {
                    lexemeCodes.Add(lexemeCode);
                    return lexemeCodes;
                }

                lexemeCodes.Add(lexemeCode);
            }
        }


        public enum LexemeType
        {
            OperationChars,
            Nonterminals,
            Terminals,
            Semantics
        }
        /// <summary>
        /// Состояние последней конвертируемой лексемы.
        /// </summary>
        public enum ConvertStatus
        {
            StUndefined,
            StTerminal,
            StTerminalQuotes,
            StNonterminalLeft,
            StColon,
            StSemantics,
            StEofgram
        }

        /// <summary>
        /// По порядку конвертирует лексемы текста в коды заданной таблицы.
        /// </summary>
        class TextConverter
        {
            private ConvertStatus convertStatus = ConvertStatus.StNonterminalLeft;

            private string currentText;
            readonly private CodeTable codeTable;

            public TextConverter(string text, CodeTable codeTable)
            {
                this.currentText = text;
                this.codeTable = codeTable;
            }

            /// <summary>
            /// Реализует автомат для конвертации первый лексемы в тексте согласно таблице кодов.
            /// </summary>
            /// <returns> Код лексемы. </returns>
            public int ConvertNextLexeme()
            {
                if (string.IsNullOrEmpty(currentText))
                    return -1; // метка конца.

                switch (convertStatus)
                {
                    case ConvertStatus.StNonterminalLeft:
                        return ConvertOnNonterminalLeft();
                    case ConvertStatus.StColon:
                        return ConvertOnColon();
                    case ConvertStatus.StUndefined:
                        return ConvertOnUndefined();
                    case ConvertStatus.StTerminalQuotes:
                        return ConvertOnTerminalQuotes();
                    case ConvertStatus.StSemantics:
                        return ConvertOnSemantics();
                    default:
                        return 0;
                }
            }

            /// <summary>
            /// Конвертирует первое вхождение лексемы в currentText в соответствии со статусом Undefined. 
            /// </summary>
            /// <returns>Код первой лексемы в currentText или 0, если в таблице нет подходящего значения</returns>
            private int ConvertOnUndefined()
            {
                foreach (LexemeType currentLexemeType in Enum.GetValues(typeof(LexemeType)))
                {
                    int currentCode = GetFirstLexeme(currentLexemeType);
                    if (currentCode != 0)
                    {
                        // Если нашли лексему в таблице.
                        if (currentLexemeType == LexemeType.Nonterminals && Char.IsLower(codeTable.Nonterminals[currentCode - 11][0]))
                        {
                            // Здесь может быть найден нетерминал начинающийся только с заглавной буквы.
                            continue;
                        }


                        // Текущая лексема -- точка. Следующая ожидаемая -- терминал до двоеточия.
                        if (currentCode == 4) convertStatus = ConvertStatus.StNonterminalLeft;

                        return currentCode;
                    }
                }

                if (currentText.StartsWith("'"))
                {
                    currentText = currentText.Remove(0, 1);
                    convertStatus = ConvertStatus.StTerminalQuotes;

                    return ConvertNextLexeme();
                }
                if (currentText.StartsWith("$"))
                {
                    currentText = currentText.Remove(0, 1);
                    convertStatus = ConvertStatus.StSemantics;

                    return ConvertNextLexeme();
                }

                return 0; // Лексема не найдена.
            }

            /// <summary>
            /// Конвертирует первое вхождение лексемы в currentText в соответствии с массивом семантики.
            /// </summary>
            /// <returns></returns>
            private int ConvertOnSemantics()
            {
                convertStatus = ConvertStatus.StUndefined;

                return GetFirstLexeme(LexemeType.Semantics);
            }

            /// <summary>
            /// Конвертитрует первое вхождение лексемы в currentText в соответствии со статусом NonterminalLeft.
            /// </summary>
            /// <returns>Код первой лексемы из массива нетерминалов или 0.</returns>
            private int ConvertOnNonterminalLeft()
            {
                // Если после точки Eofgram
                if (currentText.StartsWith("Eofgram"))
                {
                    currentText = currentText.Remove(0, "Eofgram".Length);

                    return 1000;
                }

                // Следующая ожидаемая лексема -- двоеточие.
                convertStatus = ConvertStatus.StColon;

                return GetFirstLexeme(LexemeType.Nonterminals);
            }

            /// <summary>
            /// Проверяет, является ли следующая лексема двоеточием.
            /// </summary>
            /// <returns>Код знака ':' или 0</returns>
            private int ConvertOnColon()
            {
                if (currentText.StartsWith(":"))
                {
                    currentText = currentText.Remove(0, 1);

                    convertStatus = ConvertStatus.StUndefined;

                    return 1;
                }

                return 0;
            }

            /// <summary>
            /// Конвертирует первое вхождение лексемы в currentText в соответствии со статусом TerminalQuotes.
            /// </summary>
            /// <returns>Код первой лексемы в currentText или 0</returns>
            private int ConvertOnTerminalQuotes()
            {
                int terminalCode = GetFirstLexeme(LexemeType.Terminals);
                convertStatus = ConvertStatus.StUndefined;

                if (currentText.StartsWith("'"))
                {
                    currentText = currentText.Remove(0, 1);
                }
                else
                {
                    // Ошибка.
                    return 0;
                }

                return terminalCode;
            }

            /// <summary>
            /// Ищет первой лексеме в тексте currentText соответствие в массиве заданного типа и возвращает его код. 
            /// 0 в противном случае. В случае успеха, удаляет лексему из текста currentText.
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
                    if (currentText.StartsWith(lexemes[i]))
                    {
                        // В случае совпадения в таблице -- удалить лексему.
                        currentText = currentText.Remove(0, lexemes[i].Length);
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
