using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomaticTextClassification
{
    class DocumentProcessor
    {
        public static string CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory()
            .Replace("\\AutomaticTextClassification\\bin\\Debug", ""));

        public static void ProcessTextFiles(IEnumerable<string> trainingCategories)
        {
            RefreshCsvFiles();
            //if category maatch ...for each catgory where names match join text and extract it
            foreach (var trainingCategory in trainingCategories)
            {//group categories 

                var textFilePath = Path.Combine(CurrentDirectory, trainingCategory);


                //Create user input for files and separate training and test file variables
                //get collection of each category

                var textFile = File.ReadAllText(textFilePath).ToLower();
                textFile = textFile.Replace("\n", "");
                textFile = textFile.Replace("\r", "");
                var uniqueTrainingTextWords = textFile.Split(' ', ',', '.').Distinct().ToList(); //Unique words in training set

                var textWords = textFile.Split(' ', ',', '.').ToList();

                List<string> trainingTextList = RemoveStopWords(textWords).ToList();
                Dictionary<string, int> wordDictionary = new Dictionary<string, int>();

                foreach (var word in trainingTextList.Distinct()) // total number of unique words throughout the training documents (Frequency)
                {
                    if (word != "")
                    {
                        int wordFrequency = trainingTextList.Count(x => x == word);
                        Console.WriteLine(word + ":" + wordFrequency); // write this to csv
                        wordDictionary.Add(word, wordFrequency);
                    };
                }

                string csv = String.Join(
                    Environment.NewLine,
                    wordDictionary.Select(d => d.Key + "," + d.Value + ","));// Add count to table
                string tableName = null;

                if (trainingCategory.Contains("Labour"))
                {
                    //join the text in identical categories here somehow!
                    
                    tableName = string.Format(@"{0}Table.csv", "Labour");
                }
                else if (trainingCategory.Contains("Conservative"))
                {
                    tableName = string.Format(@"{0}Table.csv", "Conservative");
                }
                else if (trainingCategory.Contains("Coalition"))
                {
                    tableName = string.Format(@"{0}Table.csv", "Coalition");
                }


                string csvPath =
                    Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""),
                        tableName);

                File.AppendAllText(csvPath, csv);
            }

        }

        private static void RefreshCsvFiles()
        {
            var directory = Directory.GetFiles(CurrentDirectory);
            foreach (var f in directory)
            {
                if (f.Contains(".csv"))
                {
                    File.Delete(f);
                }
            }
        }

        private static List<string> RemoveStopWords(List<string> trainingTextWords)
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