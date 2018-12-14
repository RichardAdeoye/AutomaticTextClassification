using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomaticTextClassification
{
    class ProcessingTools
    {
        public static void ProcessFileToList(string textFilePath, out string textFile, out List<string> textList)
        {
            textFile = ReFormatTextFile(textFilePath); // Calls Reformating Function to be used on text file
            var textWords = textFile.Split(' ', ',', '.').ToList(); //Splits text file into a list of words

           
            textList = RemoveStopWords(textWords).ToList(); // Calls Function to remove stop words from the list

        }
        public static string ReFormatTextFile(string textFilePath)
        {
            var textFile = File.ReadAllText(textFilePath).ToLower(); // Stores text file to string and converts all text to lowercase
            
            textFile = textFile.Replace("\n", ""); //Remove new line from text and spaces
            textFile = textFile.Replace("\r", ""); //Remove new line from text and spaces
            textFile = textFile.Replace('\"', ' ');
            return textFile;
        }

        public static void RefreshCsvFiles()
        {
            var directory = Directory.GetFiles(Program.CurrentDirectory);
            foreach (var f in directory)
            {
                if (f.Contains(".csv"))
                {
                    File.Delete(f); //Deletes any CSV files in current user directory
                }
            }
        }

        public static List<string> RemoveStopWords(List<string> trainingTextWords)
        {
            var stopWordsFilePath = Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), "stopwords.txt"); //Gets stop words file
            var stopWordsFile = File.ReadAllText(stopWordsFilePath); // Stores stop word file to string
            stopWordsFile = stopWordsFile.Replace("\n", ""); // Removes all spaces and new lines
            string[] x = stopWordsFile.Split(); // Seperates stop words into an array of words

            var trainingTextList = trainingTextWords.Where(i => !x.Contains(i)).ToList(); //Puts training words from a text file that are not equal to a stop word into a list
            foreach (var textWord in trainingTextList)
            {
                textWord.TrimStart('\"'); // Removes any quotation marks from the beginning of words
            }

            return trainingTextList;
        }
    }

}