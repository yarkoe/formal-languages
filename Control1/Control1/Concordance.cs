using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Control1
{
    /// <summary>
    /// Реализует методы алфавитного указателя.
    /// </summary>
    class Concordance
    {
        /// <summary>
        /// Словарь, сопоставляющий строке его места в тексте (номера строк).
        /// </summary>
        private readonly Dictionary<string, HashSet<int>> wordDictionary = new Dictionary<string, HashSet<int>>();

        /// <summary>
        /// Записывает информацию о словаре в алфавитном порядке в поток.
        /// </summary>
        /// <param name="streamWrter">Поток, куда происходит запись.</param>
        public void WriteText(StreamWriter streamWriter)
        {
            foreach (KeyValuePair<string, HashSet<int>> entry in wordDictionary.OrderBy(key => key.Key))
            {
                string line = String.Format("{0,-8}", entry.Key + ":");
                foreach (int element in entry.Value)
                {
                    line += string.Format("{0,5}", element);
                }

                streamWriter.WriteLine(line);
            }
        }

        /// <summary>
        /// Извлечь информацию из потока и заполнить словарь.
        /// </summary>
        /// <param name="streamWriter">Поток, откуда происходит чтение данных</param>
        public void ReadText(StreamReader streamReader)
        {
            for (int i = 1; -1 != streamReader.Peek(); i++)
            {
                processLine(streamReader.ReadLine(), i);
            }
        }

        /// <summary>
        /// Разделяет строку на слова и добавляет информацию в словарь.
        /// </summary>
        /// <param name="line">Рассматриваемая строка.</param>
        /// <param name="lineNumber">Номер рассматриваемой строки.</param>
        private void processLine(string line, int lineNumber)
        {
            string[] separators = { " ", ";", ":", ",", ".", "!", "?" };
            string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (!wordDictionary.ContainsKey(word))
                {
                    wordDictionary.Add(word, new HashSet<int> { lineNumber });
                }
                else
                {
                    wordDictionary[word].Add(lineNumber);
                }
            }
        }
    }
}
