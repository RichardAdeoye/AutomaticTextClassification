using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomaticTextClassification
{
    class ProcessingTools
    {
        public static void ProcessFileToList(string textFilePath, out string textFile, out List<string> textList)
        {
            textFile = ReFormatTextFile(textFilePath);
            var textWords = textFile.Split(' ', ',', '.').ToList();

            textList = RemoveStopWords(textWords).ToList();
            //get count from this
        }
        public static string ReFormatTextFile(string textFilePath)
        {
            var textFile = File.ReadAllText(textFilePath).ToLower();
            textFile = textFile.Replace("\n", "");
            textFile = textFile.Replace("\r", "");
            return textFile;
        }

        public static void RefreshCsvFiles()
        {
            var directory = Directory.GetFiles(Program.CurrentDirectory);
            foreach (var f in directory)
            {
                if (f.Contains(".csv"))
                {
                    File.Delete(f);
                }
            }
        }

        public static List<string> RemoveStopWords(List<string> trainingTextWords)
        {
            var stopWordsFilePath = Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), "stopwords.txt");
            var stopWordsFile = File.ReadAllText(stopWordsFilePath);
            stopWordsFile = stopWordsFile.Replace("\n", "");
            string[] x = stopWordsFile.Split();

            var trainingTextList = trainingTextWords.Where(i => !x.Contains(i)).ToList();
            return trainingTextList;
        }
    }

}