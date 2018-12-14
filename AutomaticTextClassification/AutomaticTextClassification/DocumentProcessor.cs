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
        
        public static Dictionary<string, double> LabDictionary = new Dictionary<string, double>();
        public static Dictionary<string, double> ConservDictionary = new Dictionary<string, double>();
        public static Dictionary<string, double> CoalDictionary = new Dictionary<string, double>();


        public static void ProcessFiles(IEnumerable<string> trainingDocuments, string categoryName, List<string> wordList,
            Dictionary<string, double> categoryDictionary, List<double> categoryConProbability)
        {
            List<string> uniqueTrainingTextWords = new List<string>();
            List<double> conditionalProbabilities = new List<double>();
            foreach (var trainingCategory in trainingDocuments.Distinct())// loops through all the training documents
            {
                if (trainingCategory.Contains(categoryName))// checks for given category name 
                {
                    var trainingFilePath = Path.Combine(CurrentDirectory, trainingCategory);// gets document directory path
                    ProcessFileToList(trainingFilePath, out var trainingTextFile, out wordList);// coverts text document to list of words
                    uniqueTrainingTextWords = trainingTextFile.Split(' ', ',', '.').Distinct().ToList();// gets unique list of words from original list
                    
                    foreach (var word in wordList.Distinct())// loops through list of training document word
                    {
                        if (word != "")
                        {
                            int wordFrequency = wordList.Count(x => x == word);// gets the frequency of which a word occurs in that list

                            if (categoryDictionary.ContainsKey(word))// checks category dictionary for duplicate dictionary pairs
                            {
                                categoryDictionary[word] = categoryDictionary[word] + wordFrequency;// Merges duplicated dictionary pairs and their values
                                double condProbability = (wordFrequency + 1D) /
                                                         (wordList.Count + uniqueTrainingTextWords.Count);//calculates conditional probability of word
                                conditionalProbabilities.Add(condProbability);// adds conditonal probablility to list
                            }
                            else // if no duplicate keys
                            {
                                categoryDictionary.Add(word, wordFrequency);//calculates conditional probability of word
                                double condProbability = (wordFrequency + 1D) /
                                                         (wordList.Count + uniqueTrainingTextWords.Count);

                                conditionalProbabilities.Add(condProbability);// adds conditonal probablility to list
                            }
                      

                        }
                    }
                  
                        foreach (var conditionalProbability in conditionalProbabilities)
                        {
                            categoryConProbability.Add(conditionalProbability);// adds all conditional probablities of category to a new list
                        }
                   
                  
                }
                
            }
         
            FileWriter.WriteToCsv(categoryName, categoryDictionary, conditionalProbabilities);// collected values to a csv document
        }
    }
}