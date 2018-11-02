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

                List<string> trainingTextList = RemoveStopWords(textWords).ToList();//get count from this
                Dictionary<string, int> wordDictionary = new Dictionary<string, int>();
                List<double> conditionalProbabilities = new List<double>();
                foreach (var word in trainingTextList.Distinct()) // total number of unique words throughout the training documents (Frequency)
                {
                    if (word != "")
                    {
                        int wordFrequency = trainingTextList.Count(x => x == word);
                    
                        wordDictionary.Add(word, wordFrequency);

                        double conProbability = (wordFrequency + 1f) / (trainingTextList.Count + uniqueTrainingTextWords.Count);
                        conditionalProbabilities.Add(conProbability);
                        Console.WriteLine(word + ": " + wordFrequency + " -------" + conProbability); // write this to csv
                    };
                }
                //string csv = String.Join(
                //    Environment.NewLine,
                //    wordDictionary.Select(d => d.Key + "," + d.Value + "," + conditionalProbabilities.Select(n => n)));// Add count to table
                string tableName = null;
                
                
                if (trainingCategory.Contains("Conservative"))
                {
                    tableName = string.Format(@"{0}Table.csv", "Conservative");
                }
                else if (trainingCategory.Contains("Coalition"))
                {
                    tableName = string.Format(@"{0}Table.csv", "Coalition");
                }
                else if (trainingCategory.Contains("Labour"))
                {
                    tableName = string.Format(@"{0}Table.csv", "Labour");
                }

                var writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""), tableName));
                int i = 0;
                foreach (var data in wordDictionary)
                {
                    writer.WriteLine("{0},{1},{2}", data.Key, data.Value, conditionalProbabilities[i]);
                    i++;
                }
                //string csvPath =
                //    Path.Combine(Directory.GetCurrentDirectory().Replace("\\AutomaticTextClassification\\bin\\Debug", ""),
                //        tableName);

               // File.AppendAllText(csvPath, csv);
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