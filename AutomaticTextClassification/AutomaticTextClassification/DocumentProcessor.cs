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


        public static void ProcessFiles(IEnumerable<string> trainingCategories, string categoryName, List<string> wordList,
            Dictionary<string, int> categoryDictionary, List<double> categoryConProbability)
        {
            List<string> uniqueTrainingTextWords = new List<string>();
            List<double> conditionalProbabilities = new List<double>();
            foreach (var trainingCategory in trainingCategories)
            {
                if (trainingCategory.Contains(categoryName))
                {
                    var trainingFilePath = Path.Combine(CurrentDirectory, trainingCategory);
                    ProcessFileToList(trainingFilePath, out var trainingTextFile, out wordList);
                    uniqueTrainingTextWords =
                        trainingTextFile.Split(' ', ',', '.').Distinct().ToList(); 
                }
            }
            foreach (var word in wordList.Distinct())
            {
                if (word != "")
                {
                    int wordFrequency = wordList.Count(x => x == word);

                    categoryDictionary.Add(word, wordFrequency);

                    double condProbability = (wordFrequency + 1D) /
                                             (wordList.Count + uniqueTrainingTextWords.Count);
                    conditionalProbabilities.Add(condProbability);
         
                }
            }
            foreach (var conditionalProbability in conditionalProbabilities)
            {
                categoryConProbability.Add(conditionalProbability);
            }
            FileWriter.WriteToCsv(categoryName, categoryDictionary, conditionalProbabilities);
        }
    }
}