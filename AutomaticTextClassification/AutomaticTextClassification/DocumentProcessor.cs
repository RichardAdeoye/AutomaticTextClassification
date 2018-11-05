using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

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

                var trainingFilePath = Path.Combine(CurrentDirectory, trainingCategory);


                //Create user input for files and separate training and test file variables
                //get collection of each category

                string trainingTextFile;
                List<string> trainingTextList;
                ProcessFileToList(trainingFilePath, out trainingTextFile, out trainingTextList);

                var uniqueTrainingTextWords = trainingTextFile.Split(' ', ',', '.').Distinct().ToList(); //Unique words in training set
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
                        //Console.WriteLine(word + ": " + wordFrequency + " -------" + conProbability);
                    };
                }
                Console.WriteLine(conditionalProbabilities.Where(x=>x.Equals("Coalition")).ToString());//split up conditional probablities 
                FileWriter.WriteToCsv(trainingCategory, wordDictionary, conditionalProbabilities);
            }
            var testFilePath = Path.Combine(CurrentDirectory, "test1.txt");
            string testFile;
            List<string> testList;
            ProcessFileToList(testFilePath, out testFile, out testList);

            foreach(var word in testList)
            {
                if(word != "")
                {
                   // Console.WriteLine(word);
                }
            }
        }
        
        
        private static void ProcessFileToList(string textFilePath, out string textFile, out List<string> textList)
        {
            textFile = ReFormatTextFile(textFilePath);
            var textWords = textFile.Split(' ', ',', '.').ToList();

            textList = RemoveStopWords(textWords).ToList();
            //get count from this
        }

        private static string ReFormatTextFile(string textFilePath)
        {
            var textFile = File.ReadAllText(textFilePath).ToLower();
            textFile = textFile.Replace("\n", "");
            textFile = textFile.Replace("\r", "");
            return textFile;
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