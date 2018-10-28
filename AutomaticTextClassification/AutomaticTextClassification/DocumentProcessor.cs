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

            foreach (var trainingCategory in trainingCategories)
            {//group categories 

                var textFilePath = Path.Combine(CurrentDirectory, trainingCategory);


                //Create user input for files and separate training and test file variables
                //get collection of each category

                var textFile = File.ReadAllText(textFilePath).ToLower();
                textFile = textFile.Replace("\n", "");
                textFile = textFile.Replace("\r", "");
                var trainingTextWords = textFile.Split(' ', ',', '.').Distinct().ToList(); //Unique words in training set

                var textWords = textFile.Split(' ', ',', '.').ToList();

                List<string> trainingTextList = RemoveStopWords(textWords).ToList();

                List<int> frequency = new List<int>();
                List<string> words = new List<string>();

                foreach (var word in trainingTextList
                ) // total number of unique words throughout the training documents (Frequency)
                {
                    int trainingWordFrequency;
                    if (word != "")
                    {
                        trainingWordFrequency = trainingTextList.Count(x => x == word);
                        //Console.WriteLine(word + ": " + trainingWordFrequency);
                        words.Add(word);
                        frequency.Add(trainingWordFrequency);
                    }

                    ;
                }

                var i = 0;
                Dictionary<string, int> wordDictionary = new Dictionary<string, int>();
                foreach (var word in words.Distinct())
                {
                    Console.WriteLine(word + ":" + frequency[i]); // write this to csv
                    i++;
                    wordDictionary.Add(word, frequency[i]);//fix frequency
                }


                string csv = String.Join(
                    Environment.NewLine,
                    wordDictionary.Select(d => d.Key + "," + d.Value + ","));// Add count to table
                string tableName = null;

                if (trainingCategory.Contains("Labour"))
                {
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