using System;
using System.IO;

namespace Control1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path to read the text");
            string fileReaderPath = Console.ReadLine();
            if (!File.Exists(fileReaderPath))
            {
                Console.WriteLine(String.Format("File does not exist: {0}", fileReaderPath));

                return;
            }

            Console.WriteLine("Enter the file path to write the dictionary");
            string fileWriterPath = Console.ReadLine();

            Concordance concordance = new Concordance();

            using (StreamReader fileReader = new StreamReader(fileReaderPath))
            {
                concordance.ReadText(fileReader);
            }

            using (StreamWriter fileWriter = new StreamWriter(fileWriterPath))
            {
                concordance.WriteText(fileWriter);
            }

        }
    }
}
