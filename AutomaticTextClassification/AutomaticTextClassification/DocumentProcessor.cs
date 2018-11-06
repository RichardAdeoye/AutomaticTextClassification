using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AutomaticTextClassification.Program;

namespace AutomaticTextClassification
{
    class DocumentProcessor
    {
        public static readonly List<double> CoalitionConProb = new List<double>();
        public static readonly List<double> ConservativeConProb = new List<double>();
        public static readonly List<double> LabourConProb = new List<double>();


        public static Dictionary<string, int> LabDictionary { get; set; }

        public static Dictionary<string, int> ConDictionary { get; set; }

        public static Dictionary<string, int> CoalDictionary { get; set; }

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
                CoalDictionary = new Dictionary<string, int>();
                ConDictionary = new Dictionary<string, int>();
                LabDictionary = new Dictionary<string, int>();
                foreach (var i in wordDictionary)
                {
                    if (trainingCategory.Contains(Coalition))
                    {
                        CoalDictionary.Add(i.Key, i.Value);
                    }
                    else if (trainingCategory.Contains(Conservative))
                    {
                        ConDictionary.Add(i.Key, i.Value);
                    }
                    else if (trainingCategory.Contains(Labour))
                    {
                       LabDictionary.Add(i.Key, i.Value);
                    }
                }

                foreach (var conditionalProbability in conditionalProbabilities)
                {
                    if (trainingCategory.Contains(Coalition))
                    {
                        CoalitionConProb.Add(conditionalProbability);
                    }
                    if (trainingCategory.Contains(Conservative))
                    {
                        ConservativeConProb.Add(conditionalProbability);
                    }
                    if (trainingCategory.Contains(Labour))
                    {
                        LabourConProb.Add(conditionalProbability);
                    }
                }
                FileWriter.WriteToCsv(trainingCategory, wordDictionary, conditionalProbabilities);
            }
            DocumentClassifier.Classify();
        }


        public static void ProcessFileToList(string textFilePath, out string textFile, out List<string> textList)
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
            var directory = Directory.GetFiles(Program.CurrentDirectory);
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