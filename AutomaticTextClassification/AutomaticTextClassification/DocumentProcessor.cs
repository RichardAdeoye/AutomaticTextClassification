using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AutomaticTextClassification.Program;
using static AutomaticTextClassification.ProcessingTools;

namespace AutomaticTextClassification
{
    class DocumentProcessor
    {
        public static List<string> CoalitionWordList= new List<string>();
        public static List<string> ConservativeWordList = new List<string>();
        public static List<string> LabourWordList = new List<string>();

        public static readonly List<double> CoalitionConProb = new List<double>();
        public static readonly List<double> ConservativeConProb = new List<double>();
        public static readonly List<double> LabourConProb = new List<double>();
        
        public static Dictionary<string, int> LabDictionary = new Dictionary<string, int>();
        public static Dictionary<string, int> ConservDictionary = new Dictionary<string, int>();
        public static Dictionary<string, int> CoalDictionary = new Dictionary<string, int>();
        //refactor into one method
        public static void ProcessCoalitionFiles(IEnumerable<string> trainingCategories)
        {
            
            List<string> uniqueTrainingTextWords = new List<string>();
            List<double> conditionalProbabilities = new List<double>();
            foreach (var trainingCategory in trainingCategories)
            {
                if (trainingCategory.Contains(Coalition))
                {
                    var trainingFilePath = Path.Combine(CurrentDirectory, trainingCategory);
                    ProcessFileToList(trainingFilePath, out var trainingTextFile, out CoalitionWordList);
                    uniqueTrainingTextWords =
                        trainingTextFile.Split(' ', ',', '.').Distinct().ToList(); //Unique words in training set
                }
            }
            foreach (var word in CoalitionWordList.Distinct())
            {
                if (word != "")
                {
                    int wordFrequency = CoalitionWordList.Count(x => x == word);

                    //wordDictionary.Add(word, wordFrequency);
                    CoalDictionary.Add(word, wordFrequency);

                    double condProbability = (wordFrequency + 1f) /
                                             (CoalitionWordList.Count + uniqueTrainingTextWords.Count);
                    conditionalProbabilities.Add(condProbability);
                    //Console.WriteLine(word + ": " + wordFrequency + " -------" + conProbability);
                }
            }
            foreach (var conditionalProbability in conditionalProbabilities)
            {
                CoalitionConProb.Add(conditionalProbability);
            }
            FileWriter.WriteToCsv(Coalition, CoalDictionary, conditionalProbabilities);

        }

        public static void ProcessConservativeFiles(IEnumerable<string> trainingCategories)
        {
           
            List<string> uniqueTrainingTextWords = new List<string>();
            List<double> conditionalProbabilities = new List<double>();
            foreach (var trainingCategory in trainingCategories)
            {
                if (trainingCategory.Contains(Conservative))
                {
                    var trainingFilePath = Path.Combine(CurrentDirectory, trainingCategory);
                    ProcessFileToList(trainingFilePath, out var trainingTextFile, out ConservativeWordList);
                    uniqueTrainingTextWords =
                        trainingTextFile.Split(' ', ',', '.').Distinct().ToList(); //Unique words in training set
                }
            }
            foreach (var word in ConservativeWordList.Distinct())
            {
                if (word != "")
                {
                    int wordFrequency = ConservativeWordList.Count(x => x == word);

                    //wordDictionary.Add(word, wordFrequency);
                    ConservDictionary.Add(word, wordFrequency);

                    double condProbability = (wordFrequency + 1f) /
                                             (ConservativeWordList.Count + uniqueTrainingTextWords.Count);
                    conditionalProbabilities.Add(condProbability);
                    //Console.WriteLine(word + ": " + wordFrequency + " -------" + conProbability);
                }
            }
            foreach (var conditionalProbability in conditionalProbabilities)
            {
                    ConservativeConProb.Add(conditionalProbability);
            }
            FileWriter.WriteToCsv(Conservative, ConservDictionary,conditionalProbabilities);

        }

        public static void ProcessLabourFiles(IEnumerable<string> trainingCategories)
        {
            
            List<string> uniqueTrainingTextWords = new List<string>();
            List<double> conditionalProbabilities = new List<double>();
            foreach (var trainingCategory in trainingCategories)
            {
                if (trainingCategory.Contains(Labour))
                {
                    var trainingFilePath = Path.Combine(CurrentDirectory, trainingCategory);
                    ProcessFileToList(trainingFilePath, out var trainingTextFile, out LabourWordList);
                    uniqueTrainingTextWords =
                        trainingTextFile.Split(' ', ',', '.').Distinct().ToList(); //Unique words in training set
                }
            }
            foreach (var word in LabourWordList.Distinct())
            {
                if (word != "")
                {
                    int wordFrequency = LabourWordList.Count(x => x == word);

                    //wordDictionary.Add(word, wordFrequency);
                    LabDictionary.Add(word, wordFrequency);

                    double condProbability = (wordFrequency + 1f) /
                                             (LabourWordList.Count + uniqueTrainingTextWords.Count);
                    conditionalProbabilities.Add(condProbability);
                    //Console.WriteLine(word + ": " + wordFrequency + " -------" + conProbability);
                }
            }
            foreach (var conditionalProbability in conditionalProbabilities)
            {
                    LabourConProb.Add(conditionalProbability);
            }
            FileWriter.WriteToCsv(Labour, LabDictionary ,conditionalProbabilities);
        }
    }
}